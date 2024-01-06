using middleware_d26.Services;
using System;
using System.Threading.Tasks;
using System.Web.Http;


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

        [HttpGet]
        [Route("{applicationName}/{containerName}/data/{dataName}")]
        public IHttpActionResult GetData(string applicationName, string containerName, string dataName)
        {
            try
            {
                var data = dataService.GetData(applicationName, containerName, dataName);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("{applicationName}/{containerName}/data/{dataName}")]
        public async Task<IHttpActionResult> DeleteData(string applicationName, string containerName, string dataName)
        {
            try
            {
                await dataService.DeleteData(applicationName, containerName, dataName);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
