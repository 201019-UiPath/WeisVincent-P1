using IceShopDB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceShopDB.Repos
{
    public interface ILocationRepo
    {

        List<Location> GetAllLocations();

        Task<List<Location>> GetAllLocationsAsync();

        List<InventoryLineItem> GetAllInventoryLineItemsAtLocation(int locationId);

        Task<List<InventoryLineItem>> GetAllInventoryLineItemsAtLocationAsync(int locationId);

        void AddInventoryLineItem(InventoryLineItem lineItem);

        void UpdateInventoryLineItem(InventoryLineItem lineItem);

        void RemoveInventoryLineItem(InventoryLineItem lineItem);



    }
}
