using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;
using Things.Domain;

namespace Things.ThingsList.Interfaces
{
    /// <summary>
    /// Things list interface.
    /// </summary>
    public interface IThingsListService : IService
    {
        /// <summary>
        /// Method to create and add a new thing in thing actor.
        /// </summary>
        /// <param name="name">Name of the thing.</param>
        /// <returns></returns>
        Task AddThing(string name);

        /// <summary>
        /// Method to retrieve list of things.
        /// </summary>
        /// <returns></returns>
        Task<Thing[]> GetThings();

        /// <summary>
        /// Method to update an existing thing.
        /// </summary>
        /// <param name="id">Thing's Id</param>
        /// <param name="name">Thing's Name</param>
        /// <returns></returns>
        Task UpdateThing(Guid id, string name);
    }
}
