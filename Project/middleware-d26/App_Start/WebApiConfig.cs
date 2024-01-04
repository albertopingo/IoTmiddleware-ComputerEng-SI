using middleware_d26.DataContext;
using middleware_d26.Services;
using System.Web.Http;
using Unity;
using Unity.AspNet.WebApi;
using Unity.Injection;
using Unity.Lifetime;

namespace middleware_d26
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new UnityContainer();
            RegisterTypes(container);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            config.MessageHandlers.Add(new SomiodMessageHandler(container.Resolve<DiscoverService>()));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );            
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            // Register your types (services, repositories, etc.) with Unity here
            // For example:
            container.RegisterType<MiddlewareDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<MqttService>(new HierarchicalLifetimeManager());
            container.RegisterType<DiscoverService>(new HierarchicalLifetimeManager());

            container.RegisterType<ContainerService>(new HierarchicalLifetimeManager());
            container.RegisterType<SubscriptionService>(new HierarchicalLifetimeManager());
            container.RegisterType<DataService>(new HierarchicalLifetimeManager());
        }
    }
}
