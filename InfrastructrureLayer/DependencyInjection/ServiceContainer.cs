using ApplicationLayer.Interfaces;
using ApplicationLayer.Logging;
using DomainLayer.Entities.Auth;
using DomainLayer.Repositories;
using InfrastructrureLayer.Authentication;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;
using InfrastructrureLayer.Logs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace InfrastructrureLayer.DependencyInjection
{
    public static class ServiceContainer {
		public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration) {
			// Add database context
			services.AddDbContext<AppDbContext>(options => {
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
			});

			services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddSignInManager();

			// Add Jwt authentication scheme
			services.AddAuthentication(options => {
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options => { // Add Jwt Bearer
				// allow store the token inside header for the authentication properties
				options.SaveToken = true;
				// allow verify token
				options.TokenValidationParameters = new TokenValidationParameters {
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,
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
			services.AddScoped<ILogException, LogException>();

			return services;
		}
	}
}
