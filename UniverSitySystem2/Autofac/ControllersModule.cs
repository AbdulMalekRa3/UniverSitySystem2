using Autofac;
using UniverSity.Api.Helpers;

namespace UniverSity.Api.Autofac
{
    public class ControllersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ControllersModule).Assembly)
            .Where(t => t.Name.EndsWith("Controller"))
            .PropertiesAutowired();
            builder.RegisterType<JwtTokenHelper>()
           .As<IJwtTokenHelper>()
           .InstancePerLifetimeScope();
        }
    }
}
