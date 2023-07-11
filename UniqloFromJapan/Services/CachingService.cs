using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using UniqloFromJapan.Data;
using UniqloFromJapan.Models;

namespace UniqloFromJapan.Services {
    public class CachingService {
        private readonly DataRepository _dataRepository;
        private readonly IMemoryCache _memoryCache;

        public CachingService(DataRepository dataRepository, IMemoryCache memoryCache) {
            _dataRepository = dataRepository;
            _memoryCache = memoryCache;
        }

        public async Task AddObjectFromCache(Product product) {
            _dataRepository.Products.Add(product);
            int n = await _dataRepository.SaveChangesAsync();
            if (n > 0) {
                _memoryCache.Set(product.Id, product, new MemoryCacheEntryOptions {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
        }

        public async Task<Product?> GetObjectFromCache(int id) {
            Product? product;
            if (!_memoryCache.TryGetValue(id, out product)) {
                product = await _dataRepository.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
                if (product != null) {

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                    _memoryCache.Set(product.Id, product, cacheEntryOptions);
                }
            }
            return product;
        }

        public async Task RemoveObjectFromCache(int id) {
            var product = await _dataRepository.Products.Where(x => x.Id == id).FirstAsync();
            if(product != null) {
                _dataRepository.Products.Remove(product);
                _memoryCache.Remove(product);
            }
        }
    }
}