using ApplicationLayer.DTOs.Email;
using ApplicationLayer.DTOs.Pagination;
using ApplicationLayer.DTOs.Request.Account;
using ApplicationLayer.DTOs.Response;
using ApplicationLayer.DTOs.Response.Account;
using ApplicationLayer.EmailService;
using ApplicationLayer.Interfaces;
using ApplicationLayer.Logging;
using DomainLayer.Entities.Auth;
using InfrastructrureLayer.Data;
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
    public class AuthService : IAuthService {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly AppDbContext _dbContext;
		private readonly IConfiguration _configuration;
		private readonly ILogException _logger;
		private readonly IEmailSender _emailSender;

		public AuthService(
				UserManager<ApplicationUser> userManager,
				RoleManager<IdentityRole> roleManager,
				AppDbContext dbContext,
				IConfiguration configuration,
				ILogException logger,
				IEmailSender emailSender
			) {
			_dbContext = dbContext;
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_logger = logger;
			_emailSender = emailSender;
		}

		public async Task<GeneralResponse> CreateRoleAsync(CreateRoleRequestDto request) {
			try {
				if ((await _roleManager.FindByNameAsync(request.Name)) is null) {
					var response = await _roleManager.CreateAsync(new IdentityRole(request.Name));
					if (!response.Succeeded) {
						return new GeneralResponse(false, "Error occured while creating new role");
					}
					return new GeneralResponse(true, $"{request.Name} created");
				}
				return new GeneralResponse(false, $"{request.Name} already exist");
			} catch (Exception ex) {
				// Log the original exception
				_logger.LogExceptions(ex);
				return new GeneralResponse(false, ex.Message);
			}
		}

		public async Task<GeneralResponse> ChangePasswordAsync(ChangePasswordRequestDto request) {
			try {
				var user = await _userManager.FindByEmailAsync(request.Email);

				if (user is null) {
					return new GeneralResponse(false, "Invalid email");
				}

				// Check current password
				if (!(await _userManager.CheckPasswordAsync(user, request.CurrentPassword))) {
					return new GeneralResponse(false, "Invalid current password");
				}

				var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
				if (!result.Succeeded) {
					return new GeneralResponse(false, "Change password fails, Internal server error occurred");
				}

				return new GeneralResponse(true, "Password has changed successfully");
			} catch (Exception ex) {
				// Log the original exception
				_logger.LogExceptions(ex);
				return new GeneralResponse(false, ex.Message);
			}
		}

		public async Task<GeneralResponse> ChangeUserRoleAsync(ChangeUserRoleRequestDto request) {
			try {
				if ((await _roleManager.FindByNameAsync(request.RoleName)) is null) {
					return new GeneralResponse(false, "Role not found");
				}

				if ((await _userManager.FindByEmailAsync(request.Email)) is null) {
					return new GeneralResponse(false, "User not found");
				}

				var user = await _userManager.FindByEmailAsync(request.Email);

				// Get old role
				var previousRole = (await _userManager.GetRolesAsync(user!)).FirstOrDefault();

				// Remove old role
				var removeOldRoleResult = await _userManager.RemoveFromRoleAsync(user!, previousRole!);
				if (!removeOldRoleResult.Succeeded) {
					return new GeneralResponse(false, "Internal server error occurred");
				}

				// Add new role
				var changeRoleResult = await _userManager.AddToRoleAsync(user!, request.RoleName);
				if (!changeRoleResult.Succeeded) {
					return new GeneralResponse(false, "Internal server error occurred");
				}

				return new GeneralResponse(true, "Role changed");
			} catch (Exception ex) {
				// Log the original exception
				_logger.LogExceptions(ex);
				return new GeneralResponse(false, ex.Message);
			}
		}

		public async Task<GeneralResponse> CreateAccountAsync(RegistrationRequestDto request) {
			try {
				// Check email already exist
				var userExist = await _userManager.FindByEmailAsync(request.Email);
				if (userExist is not null) {
					return new GeneralResponse(false, "Email already exist.");
				}

				// create user
				var newUser = new ApplicationUser() {
					Name = request.Name,
					Email = request.Email,
					UserName = request.Email,
					PasswordHash = request.Password
				};

				var isCreated = await _userManager.CreateAsync(newUser, request.Password);
				if (!isCreated.Succeeded) {
					return new GeneralResponse(false, "Error occured while creating account.");
				}

				var user = await _userManager.FindByEmailAsync(newUser.Email);
				var token = await _userManager.GenerateEmailConfirmationTokenAsync(user!);
				// Send email confirmation account
				var message = new Message(user!.Email!, "Email confirmation token", token);
				_emailSender.SendEmail(message);

				// Assign role for user
				if (await _roleManager.FindByNameAsync(request.Role) is null) {
					return new GeneralResponse(false, "Role not found.");
				}

				IdentityResult assignRoleResult = await _userManager.AddToRoleAsync(newUser, request.Role);
				if (!assignRoleResult.Succeeded) {
					return new GeneralResponse(false, "Error occured while creating account.");
				}

				// 2FA
				//await _userManager.SetTwoFactorEnabledAsync(user, true);

				return new GeneralResponse(true, "Registration successful.");
			} catch (Exception ex) {
				// Log the original exception
				_logger.LogExceptions(ex);
				return new GeneralResponse(false, ex.Message);
			}
		}

		public async Task<IEnumerable<GetRoleDto>> GetRolesAsync() {
			var roles = new List<GetRoleDto>();
			var listRole = await _roleManager.Roles.ToListAsync();
			foreach (var role in listRole) {
				var item = new GetRoleDto {
					RoleName = role.Name,
				};
				roles.Add(item);
			}
			return roles;
		}

		public async Task<ApiResponse<PagedList<GetUserResponseDto>>> GetUsersAsync(PagingRequest request) {
			try {
				var users = await _userManager.Users.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

				if (users is null || users.Count == 0) {
					return new ApiResponse<PagedList<GetUserResponseDto>>(false, "Users not found");
				}

				var usersWithRole = new List<GetUserResponseDto>();
				foreach (var user in users) {
					var getUserRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
					var getRoleInfo = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Name!.ToLower() == getUserRole!.ToLower());
					usersWithRole.Add(new GetUserResponseDto {
						Id = user.Id,
						Name = user.Name,
						Email = user.Email,
						RoleId = getRoleInfo!.Id,
						RoleName = getRoleInfo.Name,
					});
				}

				int totalUser = await _userManager.Users.CountAsync();

				return new ApiResponse<PagedList<GetUserResponseDto>>(
						new PagedList<GetUserResponseDto>(
								usersWithRole, request.PageNumber, request.PageSize, totalUser
							),
						true,
						""
					);
			} catch (Exception ex) {
				// Log the original exception
				_logger.LogExceptions(ex);
				return new ApiResponse<PagedList<GetUserResponseDto>>(false, ex.Message);
			}
		}

		public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request) {
			try {
				var user = await _userManager.FindByEmailAsync(request.Email);
				if (user is null) {
					return new AuthResponseDto(false, "User is not exist");
				}

				var checkEmailConfirmation = await _userManager.IsEmailConfirmedAsync(user);
				if (!checkEmailConfirmation) {
					// Increase failed accesses
					await _userManager.AccessFailedAsync(user);

					var checkUserLockout = await _userManager.IsLockedOutAsync(user);
					if (checkUserLockout) {
						var content = $"Your account is locked out. If you want to reset the password, " +
							$"you can use the forgot password link on the login page.";

						var message = new Message(request.Email!, "Locked out account information", content);
						_emailSender.SendEmail(message);

						return new AuthResponseDto(false, "The account is locked out.");
					}

					return new AuthResponseDto(false, "Email or password is incorrect.");
				}

				var checkCorrectPassword = await _userManager.CheckPasswordAsync(user, request.Password);
				if (!checkCorrectPassword) {
					return new AuthResponseDto(false, "Your account and/or password is incorrect, please try again");
				}

				var jwtToken = await GenerateJwtToken(user);

				// Reset access failed
				await _userManager.ResetAccessFailedCountAsync(user);

				return jwtToken;
			} catch (Exception ex) {
				// Log the original exception
				_logger.LogExceptions(ex);
				return new AuthResponseDto(false, ex.Message);
			}
		}

		public async Task<AuthResponseDto> RefreshTokenAsync(TokenRequest request) {
			try {
				if (request is not null) {
					var result = await VerifyAndGenerateToken(request);

					if (result is null) {
						return new AuthResponseDto(false, "Invalid Token");
					}

					return result;
				}
				return new AuthResponseDto(false, "Invalid payload");
			} catch (Exception ex) {
				// Log the original exception
				_logger.LogExceptions(ex);
				return new AuthResponseDto(false, ex.Message);
			}
		}

		private async Task<AuthResponseDto> VerifyAndGenerateToken(TokenRequest request) {
			var tokenValidationParameters = new TokenValidationParameters {
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Key"]!)),
				ValidateLifetime = false
			};

			var jwtTokenHandler = new JwtSecurityTokenHandler();

			try {
				var tokenInVerification = jwtTokenHandler.ValidateToken(request.AccessToken, tokenValidationParameters, out var validatedToken);
				if (validatedToken is JwtSecurityToken jwtSecurityToken) {
					var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
					if (result == false) {
						return null!;
					}
				}

				// Validate expiry time of token
				var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)!.Value);
				// Convert long to DateTime
				var expityDate = UnixTimeStampToDateTime(utcExpiryDate);
				if (expityDate > DateTime.Now) {
					return new AuthResponseDto(false, "Expired token");
				}

				// Check token in db
				var storedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == request.RefreshToken);
				if (storedToken is null || storedToken.IsUsed || storedToken.IsRevoked) {
					return new AuthResponseDto(false, "Invalid Token");
				}

				var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)!.Value;
				if (storedToken.JwtId == jti) {
					return new AuthResponseDto(false, "Invalid Token");
				}

				if (storedToken.ExpiryDate < DateTime.UtcNow) {
					return new AuthResponseDto(false, "Invalid Token");
				}

				storedToken.IsUsed = true;
				_dbContext.RefreshTokens.Update(storedToken);
				await _dbContext.SaveChangesAsync();

				var userFromDb = await _userManager.FindByIdAsync(storedToken.UserId);

				// Generate new Token
				return await GenerateJwtToken(userFromDb!);
			} catch (Exception ex) {
				// Log the original exception
				_logger.LogExceptions(ex);
				return new AuthResponseDto(false, ex.Message);
			}
		}

		// Func: convert long to DateTime
		private static DateTime UnixTimeStampToDateTime(long unixTimeStamp) {
			var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();
			return dateTimeVal;
		}

		private async Task<AuthResponseDto> GenerateJwtToken(ApplicationUser user) {
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
			var jwtToken = jwtTokenHandler.WriteToken(token);

			// RefreshToken
			var refreshToken = new RefreshToken() {
				JwtId = token.Id,
				Token = RandomStringGeneration(), // Generate a refresh token
				AddedDate = DateTime.UtcNow,
				ExpiryDate = DateTime.UtcNow.AddMonths(6),
				IsUsed = false,
				IsRevoked = false,
				UserId = user.Id,
			};

			_dbContext.RefreshTokens.Add(refreshToken);
			await _dbContext.SaveChangesAsync();

			return new AuthResponseDto(true, null!, jwtToken, refreshToken.Token);
		}

		private string RandomStringGeneration() {
			var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

			// Check if the table does not exist or has no records
			if (_dbContext.RefreshTokens == null || !_dbContext.RefreshTokens.Any()) {
				return token; // Return the token if the table has no records
			}

			// ensure token is unique by checking against db
			var tokenIsUnique = _dbContext.RefreshTokens.Any(x => x.Token == token);
			if (!tokenIsUnique) {
				return RandomStringGeneration();
			}

			return token;
		}
	}
}
