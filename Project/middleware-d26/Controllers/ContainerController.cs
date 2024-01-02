using middleware_d26.DataContext;
using System.Web.Http;
using System;
using middleware_d26.Models;
using System.Linq;
using System.Net;
using System.Web.Http.Results;

[RoutePrefix("api/somoid")]
public class ContainerController : ApiController
{
    private readonly MiddlewareDbContext dbContext;

    public ContainerController(MiddlewareDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    [HttpGet]
    [Route("{applicationName}")]
    public IHttpActionResult GetContainers(string applicationName)
    {
        try
        {
            // You need to retrieve the parent application ID based on the given applicationName
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName);

            if (parentApplication == null)
            {
                return Content(HttpStatusCode.NotFound, "Parent application not found");
            }

            // Filter containers based on the parent application ID
            var containers = dbContext.Containers.Where(c => c.Parent == parentApplication.Id).ToList();

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
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName);

            if (parentApplication == null)
            {
                return Content(HttpStatusCode.NotFound, "Parent application not found");
            }

            var container = dbContext.Containers.FirstOrDefault(c =>
                c.Parent == parentApplication.Id && c.Name == containerName);

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
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName);

            if (parentApplication == null)
            {
                return Content(HttpStatusCode.NotFound, "Parent application not found");
            }

            container.Creation_Dt = DateTime.Now;
            container.Parent = parentApplication.Id;

            dbContext.Containers.Add(container);
            dbContext.SaveChanges();
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
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName);

            if (parentApplication == null)
            {
                return Content(HttpStatusCode.NotFound, "Parent application not found");
            }

            var container = dbContext.Containers.FirstOrDefault(c =>
                c.Parent == parentApplication.Id && c.Name == containerName);

            if (container == null)
            {
                return Content(HttpStatusCode.NotFound, "Container not found");
            }

            container.Name = updatedContainer.Name;

            dbContext.SaveChanges();
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
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName);

            if (parentApplication == null)
            {
                return Content(HttpStatusCode.NotFound, "Parent application not found");
            }

            var container = dbContext.Containers.FirstOrDefault(c =>
                c.Parent == parentApplication.Id && c.Name == containerName);

            if (container == null)
            {
                return Content(HttpStatusCode.NotFound, "Container not found");
            }

            dbContext.Containers.Remove(container);
            dbContext.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }
}
