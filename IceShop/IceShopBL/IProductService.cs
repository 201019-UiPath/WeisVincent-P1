using IceShopDB.Models;
using System.Collections.Generic;

namespace IceShopBL
{
    public interface IProductService
    {
        void AddNewProduct(Product product);
        List<Product> GetAllProducts();
        void UpdateProductEntry(Product product);
    }
}