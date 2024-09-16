using DomainLayer.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace InfrastructrureLayer.Common {
	public class CacheService<T> : ICacheService<T> where T : class {
		private readonly IDistributedCache _redisCacheService;

		public CacheService(IDistributedCache redisCacheService) {
			_redisCacheService = redisCacheService;
		}

		public Task<bool> DeleteFromKey(string nameCache) {
			throw new NotImplementedException();
		}

		public Task<T> GetByKey(string nameCache) {
			throw new NotImplementedException();
		}

		public Task<T> Update(string nameCache, T data, DistributedCacheEntryOptions options = null) {
			throw new NotImplementedException();
		}
	}
}
