using IceShopDB.Models;
using IceShopDB.Repos;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceShopBL.Services
{
    /// <summary>
    /// This class handles product-specific business logic for the IceShop using a repository that implements IRepository.
    /// This includes adding new abstract product entries to the repository and getting the products in an order or at location.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IRepository repo;

        public ProductService(IRepository repo)
        {
            this.repo = repo;
        }

        public ProductService(ref IRepository repo)
        {
            this.repo = repo;
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

        public void UpdateProductEntry(Product product)
        {
            Log.Logger.Information("Updating info on a product in the store catalogue repository..");
            repo.UpdateProduct(product);
            repo.SaveChanges();
        }
    }
}
