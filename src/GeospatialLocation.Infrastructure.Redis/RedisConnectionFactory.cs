using System;
using StackExchange.Redis;

namespace GeospatialLocation.Infrastructure.Redis
{
    public static class RedisConnectionFactory
    {
        private static RedisSettings Settings = null!;

        public static void SetConnection(RedisSettings redisSettings)
        {
            Settings = redisSettings;
        }

        public static ConnectionMultiplexer CreateConnection()
        {
            if (Settings == null)
            {
                throw new ArgumentNullException(nameof(Settings));
            }

            var options = ConfigurationOptions.Parse(Settings.Host);

            // Add timeout, releasing can cause connecting issues otherwise
            options.AbortOnConnectFail = false;
            options.SyncTimeout = 20000;

            if (!string.IsNullOrWhiteSpace(Settings.Pass))
            {
                options.Password = Settings.Pass;
            }

            return ConnectionMultiplexer.Connect(options);
        }
    }
}