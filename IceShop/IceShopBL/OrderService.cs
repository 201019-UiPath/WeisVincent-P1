using IceShopDB.Models;
using IceShopDB.Repos;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceShopBL
{
    /// <summary>
    /// This class handles order-specific business logic for the IceShop using a repository that implements IRepository.
    /// This includes logic that happens during the submission of a new order, such as updating inventory and adding a new order entry.
    /// </summary>
    public class OrderService
    {
        private readonly IRepository Repo;

        public OrderService( IRepository repo)
        {
            Repo = repo;
        }

        public OrderService(ref IRepository repo)
        {
            Repo = repo;
        }

        public void UpdateLineItemsInLocationInventory()
        {
            Repo.SaveChanges();
        }

        public void RemoveLineItemsFromLocationInventory(List<InventoryLineItem> inventoryLineItems)
        {
            Log.Logger.Information("Removing multiple inventory line items from repository..");
            Repo.RemoveInventoryLineItemsFromLocation(inventoryLineItems);
            Repo.SaveChanges();
        }

        public void RemoveLineItemFromLocationInventory(InventoryLineItem inventoryLineItem)
        {
            Log.Logger.Information("Removing inventory line item from repository..");
            Repo.RemoveInventoryLineItemFromLocation(inventoryLineItem);
            Repo.SaveChanges();
        }

        public void AddOrderToRepo(Order order)
        {
            Log.Logger.Information("Adding a new order entry to the repository..");
            Repo.AddOrder(order);
            Repo.SaveChanges();
        }


        public List<OrderLineItem> GetAllProductsInOrder(Order order)
        {
            Log.Logger.Information("Retrieving all products associated with an order..");
            return Repo.GetOrderedProductsInAnOrder(order.Id);
        }

        public Task<List<OrderLineItem>> GetAllProductsInOrderAsync(Order order)
        {
            Log.Logger.Information("Retrieving all products associated with an order asynchronoously..");
            return Repo.GetOrderedProductsInAnOrderAsync(order.Id);
        }


    }
}
