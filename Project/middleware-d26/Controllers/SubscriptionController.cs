using middleware_d26.Services;
using System;
using System.Threading.Tasks;
using System.Web.Http;


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

        [HttpGet]
        [Route("{applicationName}/{containerName}/sub/{subscriptionName}")] // No conflicts
        public async Task<IHttpActionResult> GetSubscription(string applicationName, string containerName, string subscriptionName)
        {
            try
            {
                var subscription = await subscriptionService.GetSubscription(applicationName, containerName, subscriptionName);
                return Ok(subscription);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("{applicationName}/{containerName}/sub/{subscriptionName}")] // No conflicts
        public async Task<IHttpActionResult> DeleteSubscription(string applicationName, string containerName, string subscriptionName)
        {
            try
            {
                await subscriptionService.DeleteSubscription(applicationName, containerName, subscriptionName);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
