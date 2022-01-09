using System.Threading.Tasks;

namespace GeospatialLocation.Infrastructure.Redis
{
    public interface IRedisClient
    {
        //TODO: Remove unnecessary methods

        Task<long> GenerateIdAsync(string key);
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T data);
        Task AddToSetAsync(string key, long id);
        Task RemoveFromSetAsync(string key, long id);
        Task SetStringAsync(string key, string value);
        Task<bool> ExistsAsync(string key);
        Task HashSetAsync(string key, string field, long value);
        Task<long?> HashGetAsync(string key, string field);
        Task<long[]> GetSetAsIntAsync(string key);


        Task HashSetScalarAsync<T>(string key, string field, T value);
        Task HashDeleteAsync(string key, string field);
    }
}