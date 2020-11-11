using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IceShopDB.Models
{
    
    /// <summary>
    /// This class represents a single location of the store, complete with a name and address, an Id, and navigation properties to represent the location's managers and stock items.
    /// </summary>
    public class Location : IStorableInRepo
    {
        

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
