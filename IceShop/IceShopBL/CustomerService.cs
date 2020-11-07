using IceShopDB.Models;
using IceShopDB.Repos;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace IceShopBL
{

    /// <summary>
    /// This class handles customer-specific business logic for the IceShop using a repository that implements IRepository.
    /// This includes adding new customers, getting customer information by email, and getting customer order histories.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly IRepository repo;

        public CustomerService(IRepository repo)
        {
            this.repo = repo;
        }

        public CustomerService(ref IRepository repo)
        {
            this.repo = repo;
        }

        public List<Customer> GetAllCustomers()
        {
            List<Customer> getCustomers = repo.GetAllCustomers();
            Log.Logger.Information("Fetching list of customers...");
            return getCustomers;
        }

        public void AddCustomerToRepo(Customer newCustomer)
        {
            Customer possibleExistingCustomer = repo.GetCustomerByEmail(newCustomer.Email);
            if (possibleExistingCustomer == null)
            {
                Log.Logger.Information("Adding new customer to repository..");
                repo.AddCustomer(newCustomer);
                repo.SaveChanges();
            }
            else
            {
                throw new NullException(possibleExistingCustomer);
            }


        }

        public Customer GetCustomerByEmail(string newEmail)
        {
            Log.Logger.Information("Retrieving customer data from repository by an email address..");
            return repo.GetCustomerByEmail(newEmail);
        }


        public List<Order> GetAllOrdersForCustomer(Customer customer)
        {
            Log.Logger.Information("Retrieving all orders associated with a certain customer...");
            return repo.GetAllOrdersForCustomer(customer.Id);
        }

        public List<Order> GetAllOrdersForCustomerAsync(Customer customer)
        {
            Log.Logger.Information("Retrieving all orders associated with a certain customer asynchronously...");
            Task<List<Order>> getOrders = repo.GetAllOrdersForCustomerAsync(customer.Id);
            return getOrders.Result;
        }

    }
}
