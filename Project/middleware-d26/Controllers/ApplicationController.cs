using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using middleware_d26.DataContext;
using middleware_d26.Models;
using middleware_d26.Services;

namespace middleware_d26.Controllers
{
    [RoutePrefix("api/somiod")]
    public class ApplicationController : ApiController
    {
        private readonly ApplicationService applicationService;

        public ApplicationController(ApplicationService applicationService)
        {
            this.applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
        }
        public ApplicationController()
        {
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateApplication(string applicationName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(applicationName))
                {
                    return BadRequest("Application name is required");
                }

                await applicationService.CreateApplication(applicationName);
                return Created(Request.RequestUri + applicationName, applicationName);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("{applicationName}")]
        public async Task<IHttpActionResult> ModifyApplication(string applicationName,string newApplicationName)
        {
            try
            {
                await applicationService.UpdateApplication(applicationName,newApplicationName);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpDelete]
        [Route("{applicationName}")]
        public async Task<IHttpActionResult> DeleteApplication(string applicationName)
        {
            try
            {
                await applicationService.DeleteApplication(applicationName);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("{applicationName}")]
        public IHttpActionResult GetApplication(string applicationName)
        {
            try
            {
                var application = applicationService.GetApplication(applicationName);
                if (application == null)
                {
                    return Content(HttpStatusCode.NotFound, "Application not found");
                }

               return Ok(application);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}