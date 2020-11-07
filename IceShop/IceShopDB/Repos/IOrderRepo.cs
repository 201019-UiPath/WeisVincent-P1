using IceShopDB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceShopDB.Repos
{
    public interface IOrderRepo
    {


        /// <summary>
        /// This adds an order entry to the data storage place.
        /// </summary>
        /// <param name="order"></param>
        void AddOrder(Order order);

        void AddOrderAsync(Order order);





        /// <summary>
        /// This gets the order history of a specific location, specified by that locations ID asynchronously.
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        Task<List<Order>> GetAllOrdersForLocationAsync(int locationId);


        List<Order> GetAllOrdersForLocation(int locationID);

        void RemoveInventoryLineItemFromLocation(InventoryLineItem lineItem);

        void RemoveInventoryLineItemsFromLocation(List<InventoryLineItem> lineItems);


    }
}
