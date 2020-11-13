using IceShopDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceShopAPI.DTO
{
    public class APIMapper
    {

        #region Order Mapping
        public OrderDTO ParseOrder(Order order)
        {
            return new OrderDTO()
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                LocationId = order.LocationId,
                Address = order.Address,
                Subtotal = order.Subtotal,
                TimeOrderWasPlaced = order.TimeOrderWasPlaced,
                TimeOrderWasFulfilled = order.TimeOrderWasFulfilled
            };
        }

        public List<OrderDTO> ParseOrders(List<Order> orders)
        {
            List<OrderDTO> orderDTOs = new List<OrderDTO>();
            foreach (Order order in orders)
            {
                orderDTOs.Add(ParseOrder(order));
            }
            return orderDTOs;
        }

        public Order ParseOrderDTO(OrderDTO orderDTO)
        {
            return new Order( orderDTO.CustomerId, orderDTO.LocationId, orderDTO.Address, orderDTO.Subtotal, orderDTO.TimeOrderWasPlaced)
            {
                Id = orderDTO.Id,
                TimeOrderWasFulfilled = orderDTO.TimeOrderWasFulfilled
            };
        }

        public List<Order> ParseOrderDTOs(List<OrderDTO> orderDTOs)
        {
            List<Order> orders = new List<Order>();
            foreach (OrderDTO orderDTO in orderDTOs)
            {
                orders.Add(ParseOrderDTO(orderDTO));
            }
            return orders;
        }
        #endregion

    }
}
