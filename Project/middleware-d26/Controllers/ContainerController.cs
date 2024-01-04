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

        // 
        [HttpPost]
        [Route("{applicationName}")]
        public async Task<IHttpActionResult> CreateContainer(string applicationName, string containerName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(containerName))
                {
                    return BadRequest("Container name is required");
                }

                await containerService.CreateContainer(applicationName, containerName);
                return Created(Request.RequestUri + containerName, containerName);
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

        //[HttpGet]
        //[Route("{applicationName}")]
        //public IHttpActionResult GetContainers(string applicationName)
        //{
        //    try
        //    {
        //        var containers = containerService.GetContainers(applicationName);
        //        return Ok(containers);
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}

        //[HttpGet]
        //[Route("{applicationName}/{containerName}")]
        //public IHttpActionResult GetContainer(string applicationName, string containerName)
        //{
        //    try
        //    {
        //        var container = containerService.GetContainer(applicationName, containerName);
        //        if (container == null)
        //        {
        //            return Content(HttpStatusCode.NotFound, "Container not found");
        //        }

        //        return Ok(container);
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}
    }
}
