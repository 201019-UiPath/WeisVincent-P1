using System.Collections.Generic;

namespace IceShopDB.Models
{
    /// <summary>
    /// This class represents a Customer, with associated names, login credentials, 
    /// physical addresses stored as mostly unvalidated strings, and a navigation property that represents a customer's orders.
    /// </summary>
    public class Customer : User
    {

        /// <summary>
        /// Constructor for new customers
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public Customer(string name, string email, string password, string address) : base(name, email, password)
        {
            Address = address;
        }

        public Customer(string name, string email, string password) : base(name, email, password)
        {
            Address = "";
        }


        //TODO: Foreign key link to orders?
        public List<Order> Orders { get; set; }

        public string Address { get; set; }


    }
}
