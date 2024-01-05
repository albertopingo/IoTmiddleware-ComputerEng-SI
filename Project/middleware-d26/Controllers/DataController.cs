using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using middleware_d26.Services;
using middleware_d26.Models;
using System.Threading.Tasks;


namespace middleware_d26.Controllers
{
    [RoutePrefix("api/somiod")]
    public class DataController : ApiController
    {
        private readonly DataService dataService;

        public DataController() { }

        public DataController(DataService dataService)
        {
            this.dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        [HttpPost]
        [Route("{applicationName}/{containerName}")]
        public async Task<IHttpActionResult> CreateData(string applicationName, string containerName, [FromBody] string content)
        {
            try
            {
                await dataService.CreateData(applicationName, containerName, content);
                return Created(Request.RequestUri, content);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
      

        [HttpDelete]
        [Route("{applicationName}/{containerName}/data/{dataId}")]
        public async Task<IHttpActionResult> DeleteData(string applicationName, string containerName, int dataId)
        {
            try
            {
                await dataService.DeleteData(applicationName, containerName, dataId);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
