using ApplicationLayer.DTOs.Email;
using ApplicationLayer.DTOs.Request.Account;
using ApplicationLayer.DTOs.Response;
using ApplicationLayer.DTOs.Response.Account;
using ApplicationLayer.EmailService;
using ApplicationLayer.Interfaces;
using ApplicationLayer.Logging;
using DomainLayer.Entities.Auth;
using InfrastructrureLayer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace InfrastructrureLayer.Authentication {
	public class AuthService : IAuthService {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly AppDbContext _dbContext;
		private readonly IConfiguration _configuration;
		private readonly ILogException _logger;
		private readonly IEmailSender _emailSender;
		private readonly ITokenService _tokenService;

		public AuthService(
				UserManager<ApplicationUser> userManager,
				RoleManager<IdentityRole> roleManager,
				AppDbContext dbContext,
				IConfiguration configuration,
				ILogException logger,
				IEmailSender emailSender,
				ITokenService tokenService) {
			_dbContext = dbContext;
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_logger = logger;
			_emailSender = emailSender;
			_tokenService = tokenService;
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

		public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request) {
			try {
				var user = await _userManager.FindByEmailAsync(request.Email);
				if (user is null) {
					return new AuthResponseDto(false, "User is not exist");
				}

				// Check email is confirmed
				if (!await _userManager.IsEmailConfirmedAsync(user)) {
					return new AuthResponseDto(false, "Email is not confirmed.");
				}

				// Check account is lockout
				if (await _userManager.IsLockedOutAsync(user)) {
					return new AuthResponseDto(false, "The account is locked out.");
				}

				// Check password is incorrect
				if (!await _userManager.CheckPasswordAsync(user, request.Password)) {
					// Increase failed accesses
					await _userManager.AccessFailedAsync(user);

					if (await _userManager.IsLockedOutAsync(user)) {
						var content = $"Your account is locked out. If you want to reset the password, " +
							$"you can use the forgot password link on the login page.";

						var message = new Message(request.Email!, "Locked out account information", content);
						_emailSender.SendEmail(message);

						return new AuthResponseDto(false, "The account is locked out.");
					}

					return new AuthResponseDto(false, "Your account and/or password is incorrect, please try again");
				}

				// Check 2-FA
				if (await _userManager.GetTwoFactorEnabledAsync(user)) {
					return await GenerateOTPFor2Factor(user);
				}

				// Generate token
				var jwtToken = await _tokenService.GenerateToken(user, populateExp: true);
				var tokenDto = new TokenRequest() {
					AccessToken = jwtToken.AccessToken,
					RefreshToken = jwtToken.RefreshToken
				};
				// Set token to inside Cookie
				_tokenService.SetTokensInsideCookie(tokenDto);

				// Reset access failed
				await _userManager.ResetAccessFailedCountAsync(user);

				return new AuthResponseDto {
					IsSuccess = true,
					Message = "Login successful"
				};
			} catch (Exception ex) {
				// Log the original exception
				_logger.LogExceptions(ex);
				return new AuthResponseDto(false, ex.Message);
			}
		}

		public async Task<AuthResponseDto> RefreshTokenAsync(TokenRequest request) {
			try {
				if (request is not null) {
					var token = await _tokenService.RefreshToken(request);
					var tokenDto = new TokenRequest() {
						AccessToken = token.AccessToken,
						RefreshToken = token.RefreshToken
					};
					// Set token to inside Cookie
					_tokenService.SetTokensInsideCookie(tokenDto);

					return new AuthResponseDto {
						IsSuccess = true,
					};
				}
				return new AuthResponseDto(false, "Invalid payload");
			} catch (Exception ex) {
				// Log the original exception
				_logger.LogExceptions(ex);
				return new AuthResponseDto(false, ex.Message);
			}
		}

		private async Task<AuthResponseDto> GenerateOTPFor2Factor(ApplicationUser user) {
			var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);

			if (!providers.Contains("Email")) {
				return new AuthResponseDto() {
					IsSuccess = false,
					Message = "Invalid 2-Factor Provider"
				};
			}

			var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

			var message = new Message(user.Email!, "Authentication token", token);

			_emailSender.SendEmail(message);

			return new AuthResponseDto {
				IsSuccess = true,
				Is2FactorRequired = true,
				Provider = "Email"
			};
		}

		public async Task<GeneralResponse> EmailConfirmation(VerifyEmailRequestDto request) {
			if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Token)) {
				return new GeneralResponse(false, "Invalid code provided");
			}

			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user is null) {
				return new GeneralResponse(false, "Invalid identity provided");
			}

			var confirmResult = await _userManager.ConfirmEmailAsync(user, request.Token);
			if (!confirmResult.Succeeded) {
				return new GeneralResponse(false, "Invalid email confirmation request");
			}

			return new GeneralResponse(true, "Email confirmed successfully, you can proceed to login");
		}

		public async Task<GeneralResponse> ForgotPassword(ForgotPasswordRequestDto request) {
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user is null) {
				return new GeneralResponse(false, "Invalid email request");
			}

			var token = await _userManager.GeneratePasswordResetTokenAsync(user);

			var message = new Message(user.Email!, "Reset password token", token);
			_emailSender.SendEmail(message);

			return new GeneralResponse(true, "Code has been sent to your email, please check your email.");
		}

		public async Task<GeneralResponse> ResetPassword(ResetPasswordRequestDto request) {
			var user = await _userManager.FindByEmailAsync(request.Email!);
			if (user is null)
				return new GeneralResponse(false, "Invalid request");

			// reset password
			var result = await _userManager.ResetPasswordAsync(user, request.Token!, request.Password!);
			if (!result.Succeeded) {
				var errors = result.Errors.Select(x => x.Description);
				return new GeneralResponse(false, errors.First());
			}

			// if account is locked out
			await _userManager.SetLockoutEndDateAsync(user, null);

			return new GeneralResponse(true, "Reset password successful");
		}

		public async Task<GeneralResponse> EnableTwoFactor(string email) {
			var user = await _userManager.FindByEmailAsync(email);
			if (user is null) {
				return new GeneralResponse(false, "Invalid request");
			}

			string content = "We are writing to inform you that Two-Factor Authentication (2FA) has been successfully enabled for your account.";
			Message message = new Message(user.Email!, "Two-Factor Authentication Enabled on Your Account", content);
			_emailSender.SendEmail(message);

			await _userManager.SetTwoFactorEnabledAsync(user, true);

			return new GeneralResponse(true, "Enable 2-FA successful");
		}

		public async Task<GeneralResponse> DisableTwoFactor(string email) {
			var user = await _userManager.FindByEmailAsync(email);
			if (user is null) {
				return new GeneralResponse(false, "Invalid request");
			}

			string content = "We are writing to inform you that Two-Factor Authentication (2FA) has been successfully disabled for your account.";
			Message message = new Message(user.Email!, "Two-Factor Authentication Disabled on Your Account", content);
			_emailSender.SendEmail(message);

			await _userManager.SetTwoFactorEnabledAsync(user, false);

			return new GeneralResponse(true, "Disable 2-FA successful");
		}
	}
}
