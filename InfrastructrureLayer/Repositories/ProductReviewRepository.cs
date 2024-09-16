using DomainLayer.Entities;
using DomainLayer.Repositories;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;

namespace InfrastructrureLayer.Repositories {
	public class ProductReviewRepository : Repository<ProductReview>, IProductReviewRepository {
		public ProductReviewRepository(AppDbContext dbContext) : base(dbContext) {
		}
	}
}
