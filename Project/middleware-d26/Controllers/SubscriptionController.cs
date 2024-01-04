using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using middleware_d26.Services;
using middleware_d26.Models;
using System.Threading.Tasks;


namespace middleware_d26.Controllers
{
    [RoutePrefix("api/somiod")]
    public class SubscriptionController : ApiController
    {
        private readonly SubscriptionService subscriptionService;

        public SubscriptionController() { }

        public SubscriptionController(SubscriptionService subscriptionService)
        {
            this.subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
        }

        [HttpPost]
        [Route("{applicationName}/{containerName}")]
        public async Task<IHttpActionResult> CreateSubscription(string applicationName, string containerName, [FromBody] Subscription subscription)
        {
            try
            {
                await subscriptionService.CreateSubscription(applicationName, containerName, subscription);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
      

        [HttpDelete]
        [Route("{applicationName}/{subscriptionName}")]
        public async Task<IHttpActionResult> DeleteSubscription(string applicationName, string subscriptionName)
        {
            try
            {
                await subscriptionService.DeleteSubscription(applicationName, subscriptionName);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
