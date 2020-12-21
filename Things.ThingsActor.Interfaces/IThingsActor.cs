using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;

[assembly: FabricTransportActorRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2_1, RemotingClientVersion = RemotingClientVersion.V2_1)]
namespace Things.ThingsActor.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IThingsActor : IActor
    {
        /// <summary>
        /// Method to get names of all 
        /// </summary>
        /// <param name="thingsId">Thing Id</param>
        /// <returns></returns>
        Task<string> GetThingsName(string thingsId);

        /// <summary>
        /// Method to add or update thing.
        /// </summary>
        /// <param name="thingId">Thing Id</param>
        /// <param name="thingName">Thing Name</param>
        /// <returns></returns>
        Task AddOrUpdateThing(Guid thingId, string thingName);

    }
}
