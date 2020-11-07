using IceShopDB.Models;
using IceShopDB.Repos;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceShopBL
{
    /// <summary>
    /// This class handles location-specific business logic for the IceShop using a repository that implements IRepository.
    /// This includes updating location inventory and fetching stock items abd order histories.
    /// </summary>
    public class LocationService
    {
        private readonly IRepository repo;


        public LocationService(IRepository repo)
        {
            this.repo = repo;
        }

        public LocationService(ref IRepository repo) : this(repo)
        {
            this.repo = repo;
        }

        
        public List<Location> GetAllLocations()
        {
            Log.Logger.Information("Retrieving list of locations from the repository..");
            return repo.GetAllLocations();
        }

        public List<Location> GetAllLocationsAsync()
        {
            Log.Logger.Information("Retrieving list of locations from repository asynchronously..");
            Task<List<Location>> getLocations = repo.GetAllLocationsAsync();
            return getLocations.Result;
        }

        public Task<List<InventoryLineItem>> GetAllProductsAtLocationAsync(Location location)
        {
            Log.Logger.Information("Retrieving list of products at a location from the repository..");
            return repo.GetAllInventoryLineItemsAtLocationAsync(location.Id);
        }

        public List<InventoryLineItem> GetAllProductsStockedAtLocation(Location location)
        {
            Log.Logger.Information("Retrieving list of inventory stock at a location from the repository..");
            return repo.GetAllInventoryLineItemsAtLocation(location.Id);
        }

        public List<Order> GetAllOrdersForLocation(Location location)
        {
            Log.Logger.Information("Retrieving list of orders associated with a location from the repository..");
            return repo.GetAllOrdersForLocation(location.Id);
        }

        public void AddInventoryLineItemInRepo(InventoryLineItem lineItem)
        {
            Log.Logger.Information("Adding new inventory line item to repository..");
            repo.AddInventoryLineItem(lineItem);
            repo.SaveChanges();
        }

        public void UpdateInventoryLineItemInRepo(InventoryLineItem lineItem)
        {
            if (lineItem.ProductQuantity < 1)
            {
                RemoveInventoryLineItemInRepo(lineItem);
                return;
            }
            Log.Logger.Information("Updating an existing inventory line item in the repository..");
            repo.UpdateInventoryLineItem(lineItem);
            repo.SaveChanges();
        }

        public void RemoveInventoryLineItemInRepo(InventoryLineItem lineItem)
        {
            Log.Logger.Information("Removing an inventory line item from repository..");
            repo.RemoveInventoryLineItem(lineItem);
            repo.SaveChanges();
        }


        

    }
}
