using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace DomainLayer.Repositories {
	public interface IRepository<T> where T : BaseEntity {
		IQueryable<T> GetQueryable();
		Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>>? filter = null);
		Task<T> GetAsync(Expression<Func<T, bool>> filter);
		Task AddAsync(T entity);
		Task RemoveAsync(T entity);
		Task RemoveRangeAsync(IEnumerable<T> entities);
		Task UpdateAsync(T entity);

		Task<IDbContextTransaction> BeginTransactionAsync();
		Task EndTransactionAsync();
		Task RollBackTransactionAsync();
	}
}
