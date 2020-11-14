using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceShopAPI.DTO
{
    public class CustomerDTO : UserDTO
    {

        public CustomerDTO() { }

        /// <summary>
        /// Constructor for new customers
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public CustomerDTO(string name, string email, string password, string address) : base(name, email, password)
        {
            Address = address;
        }

        public CustomerDTO(string name, string email, string password) : base(name, email, password)
        {
            Address = "";
        }



        public string Address { get; set; }

    }
}
