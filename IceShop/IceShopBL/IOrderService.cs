using IceShopDB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceShopBL
{
    public interface IOrderService
    {
        void AddOrderToRepo(Order order);

        void AddOrderLineItemToRepo(OrderLineItem oli);

        List<OrderLineItem> GetAllProductsInOrder(Order order);
        Task<List<OrderLineItem>> GetAllProductsInOrderAsync(Order order);
        Order GetOrderById(int orderId);

        Order GetOrderByDateTime(double dateTimeDouble);
    }
}