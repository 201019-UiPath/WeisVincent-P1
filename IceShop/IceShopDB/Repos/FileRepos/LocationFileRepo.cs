using IceShopDB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceShopDB.Repos.FileRepos
{
    internal class LocationFileRepo : ILocationRepo
    {
        private const string filepathLocations = "IceShopDB/SampleData/Locations.txt";
        private const string filepathLocationStock = "IceShopDB/SampleData/LocationStock.txt";

        public Task<List<Location>> GetAllLocationsAsync()
        {
            throw new NotImplementedException();
        }

        public List<Order> GetAllOrdersForLocation(int locationId)
        {
            throw new NotImplementedException();
        }

        public Task<List<InventoryLineItem>> GetAllInventoryLineItemsAtLocationAsync(int locationId)
        {
            throw new NotImplementedException();
        }

        // TODO: Implement this
        public List<Location> GetLocations()
        {
            throw new NotImplementedException();
        }

        public List<InventoryLineItem> GetAllInventoryLineItemsAtLocation(int locationId)
        {
            throw new NotImplementedException();
        }

        public void UpdateInventoryLineItem(InventoryLineItem lineItem)
        {
            throw new NotImplementedException();
        }

        public List<Location> GetAllLocations()
        {
            throw new NotImplementedException();
        }

        public void AddInventoryLineItem(InventoryLineItem lineItem)
        {
            throw new NotImplementedException();
        }

        public void RemoveInventoryLineItem(InventoryLineItem lineItem)
        {
            throw new NotImplementedException();
        }
    }
}
