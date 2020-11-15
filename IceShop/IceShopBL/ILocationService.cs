using IceShopDB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceShopBL
{
    public interface ILocationService
    {
        void AddInventoryLineItemInRepo(InventoryLineItem lineItem);
        List<Location> GetAllLocations();
        List<Location> GetAllLocationsAsync();

        Location GetLocationById(int locationId);
        List<Order> GetAllOrdersForLocation(Location location);
        Task<List<InventoryLineItem>> GetAllProductsAtLocationAsync(Location location);
        Task<List<InventoryLineItem>> GetAllProductsAtLocationAsync(int locationId);
        List<InventoryLineItem> GetAllProductsStockedAtLocation(Location location);
        void RemoveInventoryLineItemInRepo(InventoryLineItem lineItem);
        void UpdateInventoryLineItemInRepo(InventoryLineItem lineItem);
    }
}