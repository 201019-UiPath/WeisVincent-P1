using IceShopDB.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace IceShopDB.Repos.FileRepos
{
    public class CustomerFileRepo : ICustomerRepo
    {
        private const string filepathCustomers = "IceShopDB/SampleData/Customers.txt";

        public void AddCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// Adds customer to file
        /// </summary>
        /// <param name="customer"></param>
        public async void AddCustomerAsync(Customer customer)
        {
            using (FileStream fs = File.Create(filepathCustomers))
            {
                await JsonSerializer.SerializeAsync(fs, customer);
                Console.WriteLine("Customer is being written to file");
            }
        }

        public List<Customer> GetAllCustomers()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all customers from file
        /// </summary>
        /// <returns></returns>
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            List<Customer> allHeroes = new List<Customer>();
            using (FileStream fs = File.OpenRead(filepathCustomers))
            {
                try
                {
                    allHeroes.Add(await JsonSerializer.DeserializeAsync<Customer>(fs));
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }
            return allHeroes;
        }

        public List<Order> GetAllOrdersForCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetAllOrdersForCustomerAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomerByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetCustomerByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
