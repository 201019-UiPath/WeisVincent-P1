using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IceShopDB.Models
{
    // TODO: Figure out how to make this enum work with the PostgreSQL DB
    public enum ProductType
    {
        Physical, Cognitive, Emotional, Metaphysical
    }
    public class Product
    {

        //TODO: Make constructor for Product.



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



        //public List<LocationStockedProduct> LocationsStockedAt;









    }
}
