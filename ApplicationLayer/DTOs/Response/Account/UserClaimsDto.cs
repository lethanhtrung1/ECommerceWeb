namespace ApplicationLayer.DTOs.Response.Account {
	public record UserClaimsDto(
		string Fullname = null!,
		string UserName = null!,
		string Email = null!,
		string Role = null!
	);
}
