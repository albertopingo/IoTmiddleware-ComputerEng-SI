using middleware_d26.DataContext;
using middleware_d26.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Unity;
using Unity.Lifetime;
using Unity.AspNet.WebApi;

namespace middleware_d26
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Configure Web API
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
