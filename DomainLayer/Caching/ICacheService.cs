using Microsoft.Extensions.Caching.Distributed;

namespace DomainLayer.Caching {
	public interface ICacheService<T> {
		Task<T> GetByKey(string nameCache);
		Task<T> Update(string nameCache, T data, DistributedCacheEntryOptions options = null);
		Task<bool> DeleteFromKey(string nameCache);
	}
}
