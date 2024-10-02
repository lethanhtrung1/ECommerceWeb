using DomainLayer.Entities;
using DomainLayer.Repositories;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;

namespace InfrastructrureLayer.Repositories {
	public class BrandRepository : Repository<Brand>, IBrandRepository {
		public BrandRepository(AppDbContext dbContext) : base(dbContext) {
		}
	}
}
