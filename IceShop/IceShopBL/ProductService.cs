using IceShopDB.Models;
using IceShopDB.Repos;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceShopBL
{
    /// <summary>
    /// This class handles product-specific business logic for the IceShop using a repository that implements IRepository.
    /// This includes adding new abstract product entries to the repository and getting the products in an order or at location.
    /// </summary>
    public class ProductService
    {
        private readonly IRepository repo;

        public ProductService( IRepository repo)
        {
            this.repo = repo;
        }

        public ProductService(ref IRepository repo)
        {
            this.repo = repo;
        }

        public void AddProductToStock(Product addedProduct, Location targetLocation)
        {
            Log.Logger.Information("Adding a product entry to a location stock..");
            repo.AddNewProductToStock(addedProduct.Id, targetLocation.Id);
            repo.SaveChanges();
        }

        public List<InventoryLineItem> GetAllProductsAtLocation(Location location)
        {
            Log.Logger.Information("Retrieving all products available at a location..");
            return repo.GetAllInventoryLineItemsAtLocationAsync(location.Id).Result;
        }

        public List<OrderLineItem> GetAllProductsInOrder(Order order)
        {
            return repo.GetOrderedProductsInAnOrder(order.Id);
        }

        public Task<List<OrderLineItem>> GetAllProductsInOrderAsync(Order order)
        {
            return repo.GetOrderedProductsInAnOrderAsync(order.Id);
        }

        public List<Product> GetAllProducts()
        {
            Log.Logger.Information("Retrieving all products available from the catalogue..");
            return repo.GetAllProducts();
        }

        public void AddNewProduct(Product product)
        {
            Log.Logger.Information("Adding a new product to the store catalogue repository..");
            repo.AddNewProduct(product);
            repo.SaveChanges();
        }

    }
}
