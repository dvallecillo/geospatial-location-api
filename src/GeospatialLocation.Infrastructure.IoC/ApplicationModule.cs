using Autofac;
using GeospatialLocation.Application.Queries;
using GeospatialLocation.Domain.Repositories;
using GeospatialLocation.Domain.SeedWork;
using GeospatialLocation.Infrastructure.Redis;
using GeospatialLocation.Infrastructure.Redis.Queries;
using GeospatialLocation.Infrastructure.Redis.Repositories;

namespace GeospatialLocation.Infrastructure.IoC
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            LoadRedis(builder);

            builder.RegisterType<LocationRepository>()
                .As<ILocationRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<LocationQueries>()
                .As<ILocationQueries>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RedisUnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
        }

        private static void LoadRedis(ContainerBuilder builder)
        {
            builder.RegisterType<RedisClient>()
                .As<IRedisClient>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RedisDataClient>()
                .As<IRedisDataClient>()
                .InstancePerLifetimeScope();

            builder.RegisterType<JsonNetSerializer>()
                .As<ISerializer>()
                .InstancePerDependency();
        }
    }
}