using IceShopDB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceShopDB.Repos
{
    public interface IProductRepo
    {

        void AddNewProduct(Product product);

        void AddNewProductToStock(int newProductId, int locationId);

        void RemoveProductAtLocation(int removedProductId, int locationId);

        List<OrderLineItem> GetOrderedProductsInAnOrder(int orderId);

        Task<List<OrderLineItem>> GetOrderedProductsInAnOrderAsync(int orderId);

        List<Product> GetAllProducts();

        Task<List<Product>> GetAllProductsAsync();

    }
}
