using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceShopAPI.DTO
{
    public class UserDTO
    {


        public UserDTO(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        
        public int Id { get; set; }

        
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }


    }
}
