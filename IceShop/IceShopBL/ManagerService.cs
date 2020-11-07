using IceShopDB.Models;
using IceShopDB.Repos;
using Serilog;
using System.Collections.Generic;

namespace IceShopBL
{
    /// <summary>
    /// This class handles manager-specific business logic for the IceShop using a repository that implements IRepository.
    /// This includes adding new managers and getting manager info by email.
    /// </summary>
    public class ManagerService
    {
        private readonly IRepository repo;
        public ManagerService(IRepository repo)
        {
            this.repo = repo;
        }

        public ManagerService(ref IRepository repo)
        {
            this.repo = repo;
        }

        public List<Manager> GetAllManagers()
        {
            Log.Logger.Information("Retrieving a list of all managers from the repository..");
            List<Manager> getManagers = repo.GetAllManagers();

            return getManagers;

        }

        public void AddManager(Manager newManager)
        {
            Log.Logger.Information("Retrieving a list of all managers from the repository..");
            repo.AddManager(newManager);
            repo.SaveChanges();
        }

        public Manager GetManagerByEmail(string newEmail)
        {
            Log.Logger.Information("Retrieving a manager by email address from the repository..");
            return repo.GetManagerByEmail(newEmail);
        }


    }
}
