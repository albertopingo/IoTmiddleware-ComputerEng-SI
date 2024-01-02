using System.Collections.Generic;
using System.Threading.Tasks;
using middleware_d26.Models;

namespace middleware_d26.Services
{
    public interface IContainerService
    {
        Task<IEnumerable<Container>> GetContainers(string applicationName);
        Task<Container> GetContainer(string applicationName, string containerName);
        Task CreateContainer(string applicationName, Container container);
        Task UpdateContainer(string applicationName, string containerName, Container updatedContainer);
        Task DeleteContainer(string applicationName, string containerName);
    }
}
