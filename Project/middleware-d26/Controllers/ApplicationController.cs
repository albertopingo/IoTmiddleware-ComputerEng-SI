using middleware_d26.Models.DTOs;
using middleware_d26.Services;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace middleware_d26.Controllers.Application
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


        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateApplication([FromBody] EntityRequestDTO createDTO)
        {
            if (createDTO == null || createDTO.ResType.ToLower() != "application")
            {
                return BadRequest($"Invalid or missing res_type: {createDTO?.ResType}");
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


        [Route("{applicationName}")]
        [HttpGet]
        [ActionName("GetApplication")]
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



        [Route("{applicationName}")]
        [HttpPut]
        public async Task<IHttpActionResult> ModifyApplication(string applicationName, [FromBody] EntityRequestDTO modifyDTO)
        {
            if (modifyDTO == null || modifyDTO.ResType.ToLower() != "application")
            {
                return BadRequest("Invalid or missing res_type (App)");
            }

            if (string.IsNullOrWhiteSpace(modifyDTO.Name))
            {
                return BadRequest("New application name required");
            }

            try
            {
                await applicationService.UpdateApplication(applicationName, modifyDTO.Name);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [Route("{applicationName}")]
        [HttpDelete]
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