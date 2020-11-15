using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IceShopWeb.Models
{
    // TODO: Figure out how to make this enum work with the PostgreSQL DB
    public enum ProductType
    {
        Water, Slush, Soft, Hard
    }
    public class Product
    {
        public Product() { }

        public Product(string name, double price, ProductType typeOfProduct, string description)
        {
            Name = name;
            Price = price;
            TypeOfProduct = typeOfProduct;
            Description = description;
        }

        public int Id { get; set; }


        public string Name { get; set; }

        public double Price { get; set; }

        public ProductType TypeOfProduct { get; set; }

        public string TypeOfProductAsString { get { return Enum.GetName(typeof(ProductType), TypeOfProduct); } }

        public string Description { get; set; }


        //public List<OrderLineItem> OrdersWithProduct { get; set; }


        //public List<InventoryLineItem> LocationsWithProduct { get; set; }


        //public List<LocationStockedProduct> LocationsStockedAt;


    }
}
