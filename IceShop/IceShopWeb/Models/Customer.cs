using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IceShopWeb.Models
{
    public class Customer : User
    {
        /// <summary>
        /// Constructor for new customers
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        [JsonConstructor]
        public Customer(string name, string email, string password, string address) : base(name, email, password)
        {
            Address = address;
        }

        public Customer(string name, string email, string password) : base(name, email, password)
        {
            //Address = "";
        }


        //TODO: Foreign key link to orders?
        //public List<Order> Orders { get; set; }

        //[Required]
        [DisplayName("Address")]
        [DataType(DataType.Text, ErrorMessage = "A user's address should be just text.")]
        public string Address { get; set; }

    }
}
