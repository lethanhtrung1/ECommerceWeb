using DomainLayer.Entities;
using DomainLayer.Repositories;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;

namespace InfrastructrureLayer.Repositories {
	public class PaymentRepository : Repository<Payment>, IPaymentRepository {
		public PaymentRepository(AppDbContext dbContext) : base(dbContext) {
		}
	}
}
