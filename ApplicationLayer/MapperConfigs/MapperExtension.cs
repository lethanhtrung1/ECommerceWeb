using ApplicationLayer.DTOs.Request.Category;
using ApplicationLayer.DTOs.Request.Product;
using ApplicationLayer.DTOs.Response.Category;
using ApplicationLayer.DTOs.Response.Product;
using AutoMapper;
using DomainLayer.Entities;

namespace ApplicationLayer.MapperConfigs {
	public class MapperExtension {
		public static MapperConfiguration RegisterMaps() {
			var mappingConfig = new MapperConfiguration(config => {
				#region Domain to Response

				config.CreateMap<Product, ProductResponseDto>();
				config.CreateMap<Category, CategoryResponseDto>();

				#endregion


				#region Request to Domain

				config.CreateMap<CreateProductRequestDto, Product>()
					.ForMember(
						dest => dest.Published,
						opt => opt.MapFrom(src => true))
					.ForMember(
						dest => dest.CreatedAt,
						opt => opt.MapFrom(src => DateTime.Now));

				config.CreateMap<CreateCategoryRequestDto, Category>()
					.ForMember(
						dest => dest.Published,
						opt => opt.MapFrom(src => true))
					.ForMember(
						dest => dest.CreatedAt,
						opt => opt.MapFrom(src => DateTime.Now));

				#endregion
			});

			return mappingConfig;
		}
	}
}
