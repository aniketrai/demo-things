using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Things.Domain;

namespace Things.ThingsList
{
    /// <summary>
    /// Things list repository interface.
    /// </summary>
    public interface IThingsListRepository
    {
        /// <summary>
        /// Method to get all things.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Thing>> GetAllThings();

        /// <summary>
        /// Method to add a new thing.
        /// </summary>
        /// <param name="name">Things name</param>
        /// <returns></returns>
        Task AddThing(string name);

        /// <summary>
        /// Method to update to thing's name.
        /// </summary>
        /// <param name="id">Thing id</param>
        /// <param name="name">Thing name</param>
        /// <returns></returns>
        Task UpdateThing(Guid id, string name);
    }
}
