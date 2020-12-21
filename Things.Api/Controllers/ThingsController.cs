using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using Things.Domain;
using Things.ThingsList.Interfaces;

namespace Things.Api.Controllers
{
    /// <summary>
    /// Things controller class. .. ... 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ThingsController : Controller
    {
        private readonly IThingsListService _thingsService;

        /// <summary>
        /// Ctor
        /// </summary>
        public ThingsController()
        {
            var proxyFactory = new ServiceProxyFactory(c => new FabricTransportServiceRemotingClientFactory());

            _thingsService = proxyFactory.CreateServiceProxy<IThingsListService>(
                new Uri("fabric:/Things.Application/Things.ThingsList"),
                new ServicePartitionKey(0));
        }

        /// <summary>
        /// GET: Fetch list of things.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Thing[]> GetAll()
        {
            return await _thingsService.GetThings();
        }

        /// <summary>
        /// POST: Add a new thing.
        /// </summary>
        /// <param name="thing"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Create([FromBody] Thing thing)
        {
            await _thingsService.AddThing(thing.Name);
        }

        /// <summary>
        /// PUT: Update an existing thing.
        /// </summary>
        /// <param name="id">Thing Id</param>
        /// <param name="thing">Thing</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task Update(Guid id, [FromBody] Thing thing)
        {
            await _thingsService.UpdateThing(id, thing.Name);
        }
    }
}