using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeospatialLocation.Infrastructure.Redis
{
    public interface IRedisClient
    {
        Task SetAsync<T>(string key, T data);
        Task<IEnumerable<T>> GetSortedAsync<T>(string key, string get);
        void AddToSet(string key, ICollection<long> collection);
    }
}