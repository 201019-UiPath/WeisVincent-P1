using IceShopDB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceShopBL
{
    public interface IProductService
    {
        void AddNewProduct(Product product);
        void AddProductToStock(Product addedProduct, Location targetLocation);
        List<Product> GetAllProducts();
        List<InventoryLineItem> GetAllProductsAtLocation(Location location);
        List<OrderLineItem> GetAllProductsInOrder(Order order);
        Task<List<OrderLineItem>> GetAllProductsInOrderAsync(Order order);
    }
}