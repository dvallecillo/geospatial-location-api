using GeospatialLocation.Infrastructure.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeospatialLocation.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Add(
            this IServiceCollection services, IConfiguration configuration)
        {
            var redisSection = configuration.GetSection("Redis");
            var redisSettings = redisSection.Get<RedisSettings>();

            services.Configure<RedisSettings>(redisSection);

            RedisConnectionFactory.SetConnection(redisSettings);

            return services;
        }
    }
}