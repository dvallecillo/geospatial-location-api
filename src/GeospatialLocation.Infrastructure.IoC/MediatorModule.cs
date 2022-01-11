using System.Reflection;
using Autofac;
using GeospatialLocation.Application.Behaviors;
using GeospatialLocation.Application.Commands;
using GeospatialLocation.Application.Queries;
using MediatR;
using Module = Autofac.Module;

namespace GeospatialLocation.Infrastructure.IoC
{
    public class MediatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(
                    typeof(GetLocationsQuery).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(
                    typeof(CreateLocationInitialLoadCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));


            builder.Register<ServiceFactory>(
                context =>
                {
                    var componentContext = context.Resolve<IComponentContext>();
                    return t => (componentContext.TryResolve(t, out var o) ? o : null)!;
                });

            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}