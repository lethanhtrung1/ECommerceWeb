using ApplicationLayer.Common;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.DTOs.Request.Account {
	public class RegistrationRequestDto : LoginRequestDto {
		[Required]
		public string Name { get; set; } = string.Empty;
		public string Role { get; set; } = Constant.Role.User;
	}
}
