using ApplicationLayer.DTOs.Request.Product;
using AutoMapper;
using DomainLayer.Entities;

namespace ApplicationLayer.MapperConfigs {
	public class MapperExtension {
		public static MapperConfiguration RegisterMaps() {
			var mappingConfig = new MapperConfiguration(config => {
				#region Domain to Response

				#endregion


				#region Request to Domain
				
				config.CreateMap<CreateProductRequestDto, Product>()
					.ForMember(
						dest => dest.CreatedAt,
						opt => opt.MapFrom(src => DateTime.Now))
					.ForMember(
						dest => dest.UpdatedAt,
						opt => opt.MapFrom(src => DateTime.Now));

				#endregion
			});

			return mappingConfig;
		}
	}
}
