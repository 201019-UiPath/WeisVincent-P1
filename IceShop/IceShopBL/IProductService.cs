using IceShopDB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceShopBL
{
    public interface IProductService
    {
        void AddNewProduct(Product product);
        List<Product> GetAllProducts();
        void UpdateProductEntry(Product product);
    }
}