using System.Web.Http;
using middleware_d26.Models.DTOs;
using middleware_d26.Services;
using System;
using System.Net;
using System.Threading.Tasks;

namespace middleware_d26.Controllers.Container
{
    [RoutePrefix("api/somiod/{applicationName:regex([a-zA-Z0-9]+)}")]
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

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateContainer(string applicationName, [FromBody] EntityRequestDTO createDTO)
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

        [Route("{containerName}")]
        [HttpGet]        
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

        [Route("{containerName}")]
        [HttpPut]
        public async Task<IHttpActionResult> ModifyContainer(string applicationName, string containerName, [FromBody] EntityRequestDTO modifyDTO)
        {
            if (modifyDTO == null || modifyDTO.ResType.ToLower() != "container")
            {
                return BadRequest("Invalid or missing res_type");
            }

            if (string.IsNullOrWhiteSpace(modifyDTO.Name))
            {
                return BadRequest("Container name required");
            }

            try
            {
                await containerService.UpdateContainer(applicationName, containerName, modifyDTO.Name);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{containerName}")]
        [HttpDelete]
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
