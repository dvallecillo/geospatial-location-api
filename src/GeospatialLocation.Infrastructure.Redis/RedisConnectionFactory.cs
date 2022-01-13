using System;
using StackExchange.Redis;

namespace GeospatialLocation.Infrastructure.Redis
{
    public static class RedisConnectionFactory
    {
        private static RedisSettings _settings = null!;

        public static void SetConnection(RedisSettings redisSettings)
        {
            _settings = redisSettings;
        }

        public static ConnectionMultiplexer CreateConnection()
        {
            if (_settings == null)
            {
                throw new ArgumentNullException(nameof(_settings));
            }

            var options = ConfigurationOptions.Parse(_settings.Host);

            // Add timeout, releasing can cause connecting issues otherwise
            options.AbortOnConnectFail = false;
            options.SyncTimeout = 20000;

            if (!string.IsNullOrWhiteSpace(_settings.Pass))
            {
                options.Password = _settings.Pass;
            }

            return ConnectionMultiplexer.Connect(options);
        }
    }
}