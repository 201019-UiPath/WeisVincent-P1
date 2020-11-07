using System.Collections.Generic;

namespace IceShopDB.Models
{
    //TODO: Add XML Documentation on Customer class
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
