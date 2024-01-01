using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using middleware_d26.Models;
using middleware_d26.DataContext;

namespace middleware_d26.Controllers
{
    [RoutePrefix("api/container")]
    public class ContainerController : ApiController
    {
        private MiddlewareDbContext dbContext = new MiddlewareDbContext();

        // GET api/container
        [HttpGet]
        [Route("")]
        public IEnumerable<Container> Get()
        {
            return dbContext.Containers.ToList();
        }

        // GET api/container/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetContainer(int id)
        {
            var container = dbContext.Containers.Find(id);
            if (container == null)
            {
                return NotFound();
            }

            return Ok(container);
        }

        // POST api/container
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateContainer([FromBody] Container container)
        {
            container.Creation_Dt = DateTime.Now; // Set the creation date
            dbContext.Containers.Add(container);
            dbContext.SaveChanges();
            return Ok();
        }

        // PUT api/container/5
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult ModifyContainer(int id, [FromBody] Container updatedContainer)
        {
            var container = dbContext.Containers.Find(id);
            if (container == null)
            {
                return NotFound();
            }

            container.Name = updatedContainer.Name;
            container.Parent = updatedContainer.Parent;

            dbContext.SaveChanges();
            return Ok();
        }

        // DELETE api/container/5
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteContainer(int id)
        {
            var container = dbContext.Containers.Find(id);
            if (container == null)
            {
                return NotFound();
            }

            dbContext.Containers.Remove(container);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
