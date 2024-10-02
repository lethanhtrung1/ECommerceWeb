using ApplicationLayer.DTOs.Request.Product;
using ApplicationLayer.DTOs.Response.Product;
using AutoMapper;
using DomainLayer.Entities;

namespace ApplicationLayer.MapperConfigs {
	public class MapperExtension {
		public static MapperConfiguration RegisterMaps() {
			var mappingConfig = new MapperConfiguration(config => {
				#region Domain to Response

				config.CreateMap<Product, ProductResponseDto>();

				#endregion


				#region Request to Domain
				
				config.CreateMap<CreateProductRequestDto, Product>()
					.ForMember(
						dest => dest.CreatedAt,
						opt => opt.MapFrom(src => DateTime.Now))
					.ForMember(
						dest => dest.UpdatedAt,
						opt => opt.MapFrom(src => DateTime.Now));

				config.CreateMap<UpdateProductRequestDto, Product>()
					.ForMember(
						dest => dest.UpdatedAt,
						opt => opt.MapFrom(src => DateTime.Now));

				#endregion
			});

			return mappingConfig;
		}
	}
}
