using IceShopDB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceShopDB.Repos
{
    public interface ICustomerRepo
    {
        /// <summary>
        /// This adds a customer entry to the data storage place asynchronously.
        /// </summary>
        /// <param name="customer"></param>
        void AddCustomerAsync(Customer customer);

        /// <summary>
        /// This adds a customer entry to the data storage place.
        /// </summary>
        /// <param name="customer"></param>
        void AddCustomer(Customer customer);

        /// <summary>
        /// This gets all customers from data storage place asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<List<Customer>> GetAllCustomersAsync();

        /// <summary>
        /// This gets all customers from data storage place.
        /// </summary>
        /// <returns></returns>
        List<Customer> GetAllCustomers();

        Task<Customer> GetCustomerByEmailAsync(string email);

        Customer GetCustomerByEmail(string email);



        /// <summary>
        /// This gets the order history of a specific customer, specified by that customer's ID.
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        List<Order> GetAllOrdersForCustomer(int customerId);

        /// <summary>
        /// This gets the order history of a specific customer, specified by that customer's ID, asynchronously.
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        Task<List<Order>> GetAllOrdersForCustomerAsync(int customerID);
    }
}
