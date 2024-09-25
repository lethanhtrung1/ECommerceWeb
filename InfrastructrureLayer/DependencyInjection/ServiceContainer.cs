using ApplicationLayer.EmailService;
using ApplicationLayer.Interfaces;
using ApplicationLayer.Logging;
using ApplicationLayer.Settings;
using DomainLayer.Entities.Auth;
using DomainLayer.Repositories;
using InfrastructrureLayer.Authentication;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;
using InfrastructrureLayer.Services;
using InfrastructrureLayer.Logs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace InfrastructrureLayer.DependencyInjection {
	public static class ServiceContainer {
		public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration) {
			// Add database context
			services.AddDbContext<AppDbContext>(options => {
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
			});

			services.AddIdentity<ApplicationUser, IdentityRole>(options => {
				// Confirmation
				options.SignIn.RequireConfirmedEmail = true;
				options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
				// // 2FA
				options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultEmailProvider;
				// Reset password
				options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;

				options.Password.RequireDigit = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequiredUniqueChars = 2;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = true;

				// User lockout
				options.Lockout.AllowedForNewUsers = true;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
				options.Lockout.MaxFailedAccessAttempts = 3;
			})
				.AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultTokenProviders()
				.AddSignInManager();

			// Add Jwt authentication scheme
			services.AddAuthentication(options => {
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options => { // Add Jwt Bearer
										 // allow store the token inside header for the authentication properties
				options.SaveToken = true;
				// allow verify token
				options.TokenValidationParameters = new TokenValidationParameters {
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = configuration["Authentication:Issuer"],
					ValidAudience = configuration["Authentication:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:Key"]!)),
					ClockSkew = TimeSpan.Zero, // Messes with expiry
				};
			});

			// configure serilog logging
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.WriteTo.Debug()
				.WriteTo.Console()
				.WriteTo.File(
					path: "Logs/log.txt",
					restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
					outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
					rollingInterval: RollingInterval.Day
				)
				.CreateLogger();

			//services.AddAuthentication();
			services.AddAuthorization();

			// Dependency Injection (DI)
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<ILogException, LogException>();
			services.AddScoped<IEmailSender, EmailSender>();

			var emailSetting = configuration.GetSection("EmailSetting").Get<EmailSetting>();
			services.AddSingleton(emailSetting!);

			return services;
		}
	}
}
