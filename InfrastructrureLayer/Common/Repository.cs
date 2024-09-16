using DomainLayer.Entities;
using DomainLayer.Repositories;
using InfrastructrureLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace InfrastructrureLayer.Common {
	public class Repository<T> : IRepository<T> where T : BaseEntity {
		private readonly AppDbContext _dbContext;
		private DbSet<T> dbSet;

		public Repository(AppDbContext dbContext) {
			_dbContext = dbContext;
			dbSet = _dbContext.Set<T>();
		}

		public async Task AddAsync(T entity) {
			await dbSet.AddAsync(entity);
		}

		public async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>>? filter = null) {
			IQueryable<T> query = dbSet;
			if (filter != null) {
				query = query.Where(filter);
			}

			return await query.ToListAsync();
		}

		public async Task<T> GetAsync(Expression<Func<T, bool>> filter) {
			IQueryable<T> query = dbSet.Where(filter);

			return await query.FirstAsync();
		}

		public IQueryable<T> GetQueryable()
			=> dbSet.AsQueryable().AsNoTracking();

		public Task RemoveAsync(T entity) {
			_dbContext.Set<T>().Remove(entity);
			return Task.CompletedTask;
		}

		public Task RemoveRangeAsync(IEnumerable<T> entities) {
			_dbContext.Set<T>().RemoveRange(entities);
			return Task.CompletedTask;
		}

		public Task UpdateAsync(T entity) {
			if (_dbContext.Entry(entity).State == EntityState.Unchanged) return Task.CompletedTask;

			T exist = _dbContext.Set<T>().Find(entity.Id)!;
			_dbContext.Entry(exist).CurrentValues.SetValues(entity);

			return Task.CompletedTask;

			//return Task.FromResult(dbSet.Update(entity));
		}

		public Task<IDbContextTransaction> BeginTransactionAsync()
			=> _dbContext.Database.BeginTransactionAsync();

		public async Task EndTransactionAsync() {
			await _dbContext.SaveChangesAsync();
			await _dbContext.Database.CommitTransactionAsync();
		}

		public Task RollBackTransactionAsync()
			=> _dbContext.Database.RollbackTransactionAsync();
	}
}
