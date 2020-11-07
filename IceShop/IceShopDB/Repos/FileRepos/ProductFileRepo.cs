using IceShopDB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceShopDB.Repos.FileRepos
{
    public class ProductFileRepo : IProductRepo
    {
        private const string filepathProducts = "IceShopDB/SampleData/Products.txt";

        public void AddNewProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public void AddNewProductToStock(int newProductId, int locationId)
        {
            throw new NotImplementedException();
        }

        public Task<List<InventoryLineItem>> GetAllInventoryEntriesAtLocationAsync(int locationID)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public List<OrderLineItem> GetOrderedProductsInAnOrder(int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderLineItem>> GetOrderedProductsInAnOrderAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        public void RemoveProductAtLocation(int removedProductId, int locationId)
        {
            throw new NotImplementedException();
        }
    }
}
