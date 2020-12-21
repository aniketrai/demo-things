using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Things.ThingsActor.Interfaces;

namespace Things.ThingsActor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class ThingsActor : Actor, IThingsActor
    {
        /// <summary>
        /// Initializes a new instance of ThingsActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public ThingsActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            // The StateManager is this actor's private state store.
            // Data stored in the StateManager will be replicated for high-availability for actors that use volatile or persisted state storage.
            // Any serializable object can be saved in the StateManager.
            // For more information, see https://aka.ms/servicefabricactorsstateserialization

            return this.StateManager.TryAddStateAsync("count", 0);
        }

        /// <summary>
        /// Method to get thing's name by id
        /// </summary>
        /// <param name="thingsId">Thing Id</param>
        /// <returns></returns>
        public async Task<string> GetThingsName(string thingsId)
        {
            return await StateManager.GetStateAsync<string>(thingsId);
        }

        /// <summary>
        /// Method to add or update thing
        /// </summary>
        /// <param name="thingId">Thing Id</param>
        /// <param name="thingName">Thing Name</param>
        /// <returns></returns>
        public async Task AddOrUpdateThing(Guid thingId, string thingName)
        {
            await StateManager.AddOrUpdateStateAsync(thingId.ToString(), thingName,
                (id, oldName) => thingName);
        }
    }
}
