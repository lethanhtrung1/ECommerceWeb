using ApplicationLayer.Interfaces;
using ApplicationLayer.MapperConfigs;
using ApplicationLayer.Middleware;
using ApplicationLayer.Services;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationLayer.DependencyInjection {
	public static class ServiceContainer {
		public static IServiceCollection AddApplicationService(this IServiceCollection services) {
			services.AddScoped<IProduct, ProductService>();

			// Config auto mapper
			IMapper mapper = MapperExtension.RegisterMaps().CreateMapper();
			services.AddSingleton(mapper);
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			return services;
		}

		public static IApplicationBuilder UseApplicationPolicy(this IApplicationBuilder app) {
			app.UseMiddleware<CustomExceptionMiddleware>();

			return app;
		}
	}
}
