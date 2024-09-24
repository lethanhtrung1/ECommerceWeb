using ApplicationLayer.DTOs.Request.Account;
using ApplicationLayer.DTOs.Response.Account;
using DomainLayer.Entities.Auth;
using Microsoft.AspNetCore.Http;

namespace ApplicationLayer.Interfaces
{
    public interface ITokenService {
		Task<TokenResponseDto> GenerateToken(ApplicationUser user, bool populateExp);
		Task<TokenResponseDto> RefreshToken(TokenRequest tokenDto);
		void SetTokensInsideCookie(TokenRequest token, HttpContext context);
		Task RevokeRefreshToken(Guid userId, string token);
	}
}
