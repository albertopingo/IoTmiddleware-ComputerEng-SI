using middleware_d26.DataContext;
using middleware_d26.Services;
using System.Web.Http;
using Unity;
using Unity.AspNet.WebApi;
using Unity.Lifetime;

namespace middleware_d26
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = UnityConfig.Container;

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
    }
}
