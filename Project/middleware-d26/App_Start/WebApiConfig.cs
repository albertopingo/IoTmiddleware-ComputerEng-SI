using middleware_d26.DataContext;
using middleware_d26.Services;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
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

            // header: application/xml
            config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/xml"));
            config.Formatters.XmlFormatter.UseXmlSerializer = true;
            //remove json formatter
            config.Formatters.Remove(config.Formatters.JsonFormatter);

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            config.MessageHandlers.Add(new SomiodMessageHandler(container.Resolve<DiscoverService>()));

            #region Routes
            // Web API routes

            // Enable attribute routing
            //config.MapHttpAttributeRoutes();
            //config.MapHttpAttributeRoutes(container.Resolve<DefaultDirectRouteProvider>());

            #region Application Routes
            config.Routes.MapHttpRoute(
               name: "GetApplication",
               routeTemplate: "api/somiod/{applicationName}",
               defaults: new { controller = "Application", action = "GetApplication" },
               constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
            );

            // POST route
            config.Routes.MapHttpRoute(
                name: "CreateApplication",
                routeTemplate: "api/somiod",
                defaults: new { controller = "Application", action = "CreateApplication" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
            );

            // PUT route
            config.Routes.MapHttpRoute(
                name: "ModifyApplication",
                routeTemplate: "api/somiod/{applicationName}",
                defaults: new { controller = "Application", action = "ModifyApplication" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Put) }
            );

            // DELETE route
            config.Routes.MapHttpRoute(
                name: "DeleteApplication",
                routeTemplate: "api/somiod/{applicationName}",
                defaults: new { controller = "Application", action = "DeleteApplication" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Delete) }
            );
            #endregion

            #region Container Routes
            // Routes for ContainerController
            config.Routes.MapHttpRoute(
                name: "CreateContainer",
                routeTemplate: "api/somiod/{applicationName}",
                defaults: new { controller = "Container", action = "CreateContainer" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
            );

            config.Routes.MapHttpRoute(
                name: "GetContainer",
                routeTemplate: "api/somiod/{applicationName}/{containerName}",
                defaults: new { controller = "Container", action = "GetContainer" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
            );

            config.Routes.MapHttpRoute(
                name: "ModifyContainer",
                routeTemplate: "api/somiod/{applicationName}/{containerName}",
                defaults: new { controller = "Container", action = "ModifyContainer" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Put) }
            );

            config.Routes.MapHttpRoute(
                name: "DeleteContainer",
                routeTemplate: "api/somiod/{applicationName}/{containerName}",
                defaults: new { controller = "Container", action = "DeleteContainer" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Delete) }
            );
            #endregion

            #region Data Routes
            // Routes for DataController
            config.Routes.MapHttpRoute(
                name: "CreateData",
                routeTemplate: "api/somiod/{applicationName}/{containerName}/data/{dataName}",
                defaults: new { controller = "Resource", action = "CreateResource" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
            );

            config.Routes.MapHttpRoute(
                name: "GetData",
                routeTemplate: "api/somiod/{applicationName}/{containerName}/data/{dataName}",
                defaults: new { controller = "Data", action = "GetData" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
                );

            config.Routes.MapHttpRoute(
                name: "DeleteData",
                routeTemplate: "api/somiod/{applicationName}/{containerName}/data/{dataName}",
                defaults: new { controller = "Data", action = "DeleteData" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Delete) }
            );
            #endregion

            #region Subscription Routes
            // Routes for SubscriptionController
            config.Routes.MapHttpRoute(
            name: "CreateSubscription",
            routeTemplate: "api/somiod/{applicationName}/{containerName}",
            defaults: new { controller = "Resource", action = "CreateResource" },
            constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
            );

            config.Routes.MapHttpRoute(
                name: "GetSubscription",
                routeTemplate: "api/somiod/{applicationName}/{containerName}/sub/{subscriptionName}",
                defaults: new { controller = "Subscription", action = "GetSubscription" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
                );

            config.Routes.MapHttpRoute(
                name: "DeleteSubscription",
                routeTemplate: "api/somiod/{applicationName}/{containerName}/sub/{subscriptionName}",
                defaults: new { controller = "Subscription", action = "DeleteSubscription" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Delete) }
            );
            #endregion

            #endregion

            config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/somiod/{applicationName}/{containerName}/{subscriptionName}",
            defaults: new
            {
                applicationName = RouteParameter.Optional,
                containerName = RouteParameter.Optional,
                subscriptionName = RouteParameter.Optional
            }
            );
        }
    }
}


/*
config.Routes.MapHttpRoute(
    name: "DefaultApi",
    routeTemplate: "api/{controller}/{id}",
    defaults: new 
        { 
            id = RouteParameter.Optional
        }
    );
 */