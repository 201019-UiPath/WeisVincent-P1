using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IceShopDB.Models
{
    //TODO: Add XML Documentation on Location class
    public class Location : IStorableInRepo
    {
        // TODO: Create Location constructors

        public Location(string name, string address)
        {
            Name = name;
            Address = address;
        }

        [Key]
        public int Id { get; set; }

        //public List<Manager> Managers { get; set; }//TODO: revisit
        [Column("Name")]
        public string Name { get; set; }

        [Column("Address")]
        public string Address { get; set; }


        public List<InventoryLineItem> InventoryLineItems { get; set; }

        public List<Manager> Managers { get; set; }

        //public Stack<Order> OrderHistory;


    }
}
