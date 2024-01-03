using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using middleware_d26.DataContext;
using middleware_d26.Models;
using middleware_d26.Services;

[RoutePrefix("api/somoid")]
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

    [HttpGet]
    [Route("{applicationName}")]
    public IHttpActionResult GetContainers(string applicationName)
    {
        try
        {
            var containers = containerService.GetContainers(applicationName);
            return Ok(containers);
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

    [HttpPost]
    [Route("{applicationName}")]
    public IHttpActionResult CreateContainer(string applicationName, [FromBody] Container container)
    {
        try
        {
            containerService.CreateContainer(applicationName, container);
            return Created(Request.RequestUri + container.Id.ToString(), container);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpPut]
    [Route("{applicationName}/{containerName}")]
    public IHttpActionResult ModifyContainer(string applicationName, string containerName, [FromBody] Container updatedContainer)
    {
        try
        {
            containerService.UpdateContainer(applicationName, containerName, updatedContainer);
            return Ok();
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    [HttpDelete]
    [Route("{applicationName}/{containerName}")]
    public IHttpActionResult DeleteContainer(string applicationName, string containerName)
    {
        try
        {
            containerService.DeleteContainer(applicationName, containerName);
            return Ok();
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }
}
