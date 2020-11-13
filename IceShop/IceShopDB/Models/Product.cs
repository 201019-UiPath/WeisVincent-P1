using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IceShopDB.Models
{
    public enum ProductType
    {
        Water, Slush, Soft, Hard
    }
    public class Product
    {
        public Product(string name, double price, ProductType typeOfProduct, string description)
        {
            Name = name;
            Price = price;
            TypeOfProduct = typeOfProduct;
            Description = description;
        }

        [Key]
        public int Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Price")]
        public double Price { get; set; }

        [Column("Type")]
        public ProductType TypeOfProduct { get; set; }

        [NotMapped]
        public string TypeOfProductAsString { get { return Enum.GetName(typeof(ProductType), TypeOfProduct); } }

        [Column("Description")]
        public string Description { get; set; }


        public List<OrderLineItem> OrdersWithProduct { get; set; }


        public List<InventoryLineItem> LocationsWithProduct { get; set; }

    }
}
