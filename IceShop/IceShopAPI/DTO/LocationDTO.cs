using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceShopAPI.DTO
{
    public class LocationDTO
    {

        public LocationDTO(string name, string address)
        {
            Name = name;
            Address = address;
        }

        
        public int Id { get; set; }

        
        public string Name { get; set; }

        public string Address { get; set; }


        //public List<ILIDTO> InventoryLineItems { get; set; }

        //public List<ManagerDTO> Managers { get; set; }

        //public Stack<Order> OrderHistory;
    }
}
