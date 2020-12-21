using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Things.Domain;
using Things.ThingsList.Interfaces;

namespace Things.ThingsList
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class ThingsListService : StatefulService, IThingsListService
    {
        private readonly IThingsListRepository _repository;

        public ThingsListService(StatefulServiceContext context)
            : base(context)
        {
            _repository = new ThingsListRepository(StateManager);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener( context => new FabricTransportServiceRemotingListener(context,this))
            };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {

        }
        
        /// <summary>
        /// Add a new thing.
        /// </summary>
        /// <param name="name">Name</param>
        /// <exception cref="ArgumentNullException">If name is empty or null</exception>
        /// <returns></returns>
        public async Task AddThing(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            await _repository.AddThing(name);
        }

        /// <summary>
        /// Get all things.
        /// </summary>
        /// <returns></returns>
        public async Task<Thing[]> GetThings()
        {
            return (await _repository.GetAllThings()).ToArray();
        }

        /// <summary>
        /// Update a thing.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="name">Name</param>
        /// <exception cref="ArgumentNullException">If id is empty or null</exception>
        /// <exception cref="ArgumentNullException">If name is empty or null</exception>
        /// <returns></returns>
        public async Task UpdateThing(Guid id, string name)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            await _repository.UpdateThing(id, name);
        }
    }
}
