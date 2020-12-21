using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Things.Domain;
using Things.ThingsActor.Interfaces;

namespace Things.ThingsList
{
    /// <summary>
    /// Things list repository class.
    /// </summary>
    public class ThingsListRepository : IThingsListRepository
    {
        private readonly IReliableStateManager _stateManager;
        private readonly string _thingsActorUri = "fabric:/Things.Application/ThingsActorService";

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="stateManager">StateManager</param>
        public ThingsListRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        /// <summary>
        /// Get all things name.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Thing>> GetAllThings()
        {
            var products = await _stateManager.GetOrAddAsync<IReliableDictionary<string, Guid>>("thingsIds");
            var result = new List<Thing>();

            using (var transaction = _stateManager.CreateTransaction())
            {
                var allProducts = await products.CreateEnumerableAsync(transaction, EnumerationMode.Unordered);

                using (var enumerator = allProducts.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        var (key, value) = enumerator.Current;
                        var actorService = GetActorInstance(value);
                        result.Add(new Thing()
                        {
                            Id = value,
                            Name = await actorService.GetThingsName(key)
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Add new thing.
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns></returns>
        public async Task AddThing(string name)
        {
            var counter = await _stateManager.GetOrAddAsync<IReliableDictionary<string, int>>("counter");
            var thingsIds = await _stateManager.GetOrAddAsync<IReliableDictionary<string, Guid>>("thingsIds");

            using (var transaction = _stateManager.CreateTransaction())
            {
                var indexCounter = await counter.AddOrUpdateAsync(transaction, "index", 1, (id, value) => value + 1);

                var actorService = GetActorInstance(Guid.NewGuid());

                await actorService.AddOrUpdateThing(actorService.GetActorId().GetGuidId(), $"{name}-{indexCounter}");

                await thingsIds.AddAsync(transaction, actorService.GetActorId().ToString(), actorService.GetActorId().GetGuidId());

                await transaction.CommitAsync();
            }
        }

        /// <summary>
        /// Update a thing.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="name">Name</param>
        /// <returns></returns>
        public async Task UpdateThing(Guid id, string name)
        {
            using (var transaction = _stateManager.CreateTransaction())
            {
                var actorService = GetActorInstance(id);

                await actorService.AddOrUpdateThing(id, name);

                await transaction.CommitAsync();
            }
        }

        /// <summary>
        /// Get an instance of thing actor service.
        /// </summary>
        /// <param name="id">Actor Id</param>
        /// <returns></returns>
        private IThingsActor GetActorInstance(Guid id)
        {
            return ActorProxy.Create<IThingsActor>(
                new ActorId(id),
                new Uri(_thingsActorUri));
        }

    }
}
