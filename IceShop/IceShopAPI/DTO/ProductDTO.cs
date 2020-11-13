using IceShopDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceShopAPI.DTO
{
    public class ProductDTO
    {
        public ProductDTO(string name, double price, ProductType typeOfProduct, string description)
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

    }
}
