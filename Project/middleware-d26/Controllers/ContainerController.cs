using middleware_d26.Models.DTOs;
using middleware_d26.Services;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace middleware_d26.Controllers
{
    [RoutePrefix("api/somiod")]
    public class ContainerController : ApiController
    {
        private readonly ContainerService containerService;

        public ContainerController(ContainerService containerService)
        {
            this.containerService = containerService ?? throw new ArgumentNullException(nameof(containerService));
        }

        public ContainerController()
        {
        }

        [HttpPost]
        [Route("{applicationName}")]
        public async Task<IHttpActionResult> CreateContainer(string applicationName, [FromBody] CreateDTO createDTO)
        {
            if (createDTO == null || createDTO.ResType.ToLower() != "container")
            {
                return BadRequest("Invalid or missing res_type");
            }

            if (string.IsNullOrWhiteSpace(createDTO.Name))
            {
                return BadRequest("Container name required");
            }

            try
            {
                await containerService.CreateContainer(applicationName, createDTO.Name);
                return Created(Request.RequestUri + createDTO.Name, createDTO.Name);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{applicationName}/{containerName}")]
        public IHttpActionResult GetContainer(string applicationName, string containerName)
        {
            try
            {
                var container = containerService.GetContainer(applicationName, containerName);
                if (container == null)
                {
                    return Content(HttpStatusCode.NotFound, "Container not found");
                }

                return Ok(container);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("{applicationName}/{containerName}")]
        public async Task<IHttpActionResult> ModifyContainer(string applicationName, string containerName, string newContainerName)
        {
            try
            {
                await containerService.UpdateContainer(applicationName, containerName, newContainerName);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("{applicationName}/{containerName}")]
        public async Task<IHttpActionResult> DeleteContainer(string applicationName, string containerName)
        {
            try
            {
                await containerService.DeleteContainer(applicationName, containerName);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
