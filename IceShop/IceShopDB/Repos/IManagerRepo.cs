using IceShopDB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceShopDB.Repos
{
    public interface IManagerRepo
    {


        /// <summary>
        /// This adds a manager entry to the data storage place.
        /// </summary>
        /// <param name="manager"></param>
        void AddManager(Manager manager);

        /// <summary>
        /// This gets all managers from data storage place asynchronously. 
        /// Should not be used while doing anything else with the manager repo.
        /// </summary>
        /// <returns></returns>
        Task<List<Manager>> GetAllManagersAsync();

        /// <summary>
        /// This gets all managers from data storage place.
        /// </summary>
        /// <returns></returns>
        List<Manager> GetAllManagers();

        /// <summary>
        /// This gets a manager by their email from data storage place asynchronously. 
        /// Should not be used while doing anything else with the manager repo.
        /// </summary>
        /// <returns></returns>
        Task<Manager> GetManagerByEmailAsync(string email);

        /// <summary>
        /// This gets a manager by their email from data storage place.
        /// </summary>
        /// <returns></returns>
        Manager GetManagerByEmail(string email);

    }
}
