using middleware_d26.Models.DTOs;
using middleware_d26.Services;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

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
        public async Task<IHttpActionResult> CreateApplication([FromBody] CreateDTO createDTO)
        {
            if (createDTO == null || createDTO.ResType.ToLower() != "application")
            {
                return BadRequest("Invalid or missing res_type");
            }

            if (string.IsNullOrWhiteSpace(createDTO.Name))
            {
                return BadRequest("Application name required");
            }

            try
            {
                await applicationService.CreateApplication(createDTO.Name);
                return Created(Request.RequestUri + createDTO.Name, createDTO.Name);
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

        [HttpPut]
        [Route("{applicationName}")]
        public async Task<IHttpActionResult> ModifyApplication(string applicationName, string newApplicationName)
        {
            try
            {
                await applicationService.UpdateApplication(applicationName, newApplicationName);
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
    }
}