using ApplicationLayer.DTOs.Request.Account;
using ApplicationLayer.DTOs.Response.Account;
using ApplicationLayer.Interfaces;
using DomainLayer.Entities.Auth;
using InfrastructrureLayer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InfrastructrureLayer.Authentication
{
    public class TokenService : ITokenService {
		private readonly AppDbContext _dbContext;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IConfiguration _configuration;
		private readonly IConfigurationSection _jwtSettings;

		public TokenService(AppDbContext dbContext, UserManager<ApplicationUser> userManager, IConfiguration configuration) {
			_dbContext = dbContext;
			_userManager = userManager;
			_configuration = configuration;
			_jwtSettings = _configuration.GetSection("JwtSetting");
		}

		public async Task<TokenResponseDto> GenerateToken(ApplicationUser user, bool populateExp) {
			var jwtTokenHandler = new JwtSecurityTokenHandler();

			var key = Encoding.UTF8.GetBytes(_configuration.GetSection("Authentication:Key").Value!); // byte[]

			// Token descriptor
			var tokenDescriptor = new SecurityTokenDescriptor() {
				Audience = _configuration.GetSection("Authentication:Audience").Value,
				Issuer = _configuration.GetSection("Authentication:Issuer").Value,

				Subject = new ClaimsIdentity(new[] {
					new Claim("Id", user.Id),
					new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
					new Claim(JwtRegisteredClaimNames.Email, user.Email!),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
					new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
					new Claim(ClaimTypes.Role, (await _userManager.GetRolesAsync(user)).FirstOrDefault()!.ToString())
				}),

				// how long will this token lives
				Expires = DateTime.UtcNow.Add(TimeSpan.Parse(_configuration.GetSection("Authentication:ExpiryTimeFrame").Value!)),

				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
			};

			var token = jwtTokenHandler.CreateToken(tokenDescriptor);

			// RefreshToken
			var refreshToken = GenerateRefreshToken();
			var refreshTokenToDb = new RefreshToken() {
				JwtId = token.Id,
				Token = refreshToken,
				AddedDate = DateTime.UtcNow,
				//ExpiryDate = DateTime.Now.AddDays(7),
				IsUsed = false,
				IsRevoked = false,
				UserId = user.Id,
			};

			if (populateExp) {
				refreshTokenToDb.ExpiryDate = DateTime.Now.AddDays(7);
			}
			await _dbContext.RefreshTokens.AddAsync(refreshTokenToDb);
			await _dbContext.SaveChangesAsync();

			var accessToken = jwtTokenHandler.WriteToken(token);

			return new TokenResponseDto(accessToken, refreshToken);
		}


		public async Task<TokenResponseDto> RefreshToken(TokenRequest tokenDto) {
			var jwtSettings = _configuration.GetSection("JwtSettings");
			var tokenValidationParameters = new TokenValidationParameters {
				ValidateAudience = true,
				ValidateIssuer = true,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecurityKey"]!)),
				ValidateLifetime = true,
				ValidAudience = jwtSettings["Audience"],
				ValidIssuer = jwtSettings["Issuer"]
			};

			var tokenHandler = new JwtSecurityTokenHandler();

			var tokenInVerification = tokenHandler.ValidateToken(tokenDto.AccessToken, tokenValidationParameters, out var validatedToken);
			if (validatedToken is JwtSecurityToken jwtSecurityToken) {
				var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
				if (result == false) {
					throw new SecurityTokenException("Invalid token");
				}
			}

			// Validate expiry time of token
			var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)!.Value);
			// Convert long to DateTime
			var expityDate = UnixTimeStampToDateTime(utcExpiryDate);
			if (expityDate <= DateTime.Now) {
				throw new SecurityTokenException("Invalid token");
			}

			// Check token in db
			var storedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenDto.RefreshToken);
			if (storedToken is null || storedToken.IsUsed || storedToken.IsRevoked) {
				throw new SecurityTokenException("Invalid token");
			}

			var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)!.Value;
			if (storedToken.JwtId == jti) {
				throw new SecurityTokenException("Invalid token");
			}

			// Check refresh token is expired
			if (storedToken.ExpiryDate < DateTime.UtcNow) {
				throw new SecurityTokenException("Invalid token");
			}

			storedToken.IsUsed = true;
			_dbContext.RefreshTokens.Update(storedToken);
			await _dbContext.SaveChangesAsync();

			var userFromDb = await _userManager.FindByIdAsync(storedToken.UserId);

			return await GenerateToken(userFromDb!, populateExp: false);
		}


		public void SetTokensInsideCookie(TokenRequest token, HttpContext context) {
			context.Response.Cookies.Append("accessToken", token.AccessToken,
				new CookieOptions {
					Expires = DateTimeOffset.UtcNow.AddMinutes(5),
					HttpOnly = true,
					IsEssential = true,
					Secure = true,
					SameSite = SameSiteMode.None
				}
			);

			context.Response.Cookies.Append("refreshToken", token.RefreshToken,
				new CookieOptions {
					Expires = DateTimeOffset.UtcNow.AddDays(7),
					HttpOnly = true,
					IsEssential = true,
					Secure = true,
					SameSite = SameSiteMode.None
				}
			);
		}


		public async Task RevokeRefreshToken(Guid userId, string token) {
			var refreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
			if (refreshToken == null || refreshToken.UserId != userId.ToString()) {
				throw new SecurityTokenException();
			}
			refreshToken.IsRevoked = true;
			_dbContext.RefreshTokens.Update(refreshToken);
			await _dbContext.SaveChangesAsync();
		}

		private string GenerateRefreshToken() {
			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create()) {
				rng.GetBytes(randomNumber);

				return Convert.ToBase64String(randomNumber);
			}
		}

		private static DateTime UnixTimeStampToDateTime(long unixTimeStamp) {
			var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();
			return dateTimeVal;
		}
	}
}
