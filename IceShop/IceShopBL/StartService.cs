using IceShopDB.Models;
using IceShopDB.Repos;
using IceShopLib.Validation;
using Serilog;
using System;

namespace IceShopBL
{
    /// <summary>
    /// This class handles sign-in and log-in business logic for the IceShop using a repository that implements IRepository.
    /// This is where all authentication requests, sign-ups, and sign-ins are born and die.
    /// </summary>
    public sealed class StartService
    {
        private readonly IRepository Repo;
        private readonly CustomerService customerService;
        private readonly ManagerService managerService;

        public StartService( IRepository repo)
        {
            Repo = repo;
            customerService = new CustomerService(ref Repo);
            managerService = new ManagerService(ref Repo);
        }

        public StartService(ref IRepository repo)
        {
            Repo = repo;
            customerService = new CustomerService(ref Repo);
            managerService = new ManagerService(ref Repo);
        }

        
        public bool DoesUserExistWithEmail(string email)
        {
            if (GetUserByEmail(email) != null) return true; else return false;
        }
        #region Interaction with DB
        public User GetUserByEmail(string email)
        {
            Log.Logger.Information("Retrieving a user by their email by querying both the Customer and Manager collections in the repository..");
            Customer userAsCustomer = customerService.GetCustomerByEmail(email);
            Manager userAsManager = managerService.GetManagerByEmail(email);
            if (userAsCustomer == null)
            {
                if (userAsManager == null)
                {
                    return null;
                }
                else
                {
                    return userAsManager;
                }
            }
            else
            {
                return userAsCustomer;
            }

        }


        #endregion

        public bool CheckIfCustomer(User user)
        {
            if (user is Customer) return true; else return false;
        }

        public bool CheckIfManager(User user)
        {
            if (user is Manager) return true; else return false;
        }


        // TODO: Use this to make methods that return a customer or manager, depending on what's needed
        public Type GetTypeOfUser(User user)
        {
            if (user is Customer)
            {
                return typeof(Customer);
            }
            else if (user is Manager)
            {
                return typeof(Manager);
            }
            else { return null; }
        }

    }
}
