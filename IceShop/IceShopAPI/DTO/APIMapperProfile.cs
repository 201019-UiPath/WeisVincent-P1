using AutoMapper;
using IceShopDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceShopAPI.DTO
{
    public class APIMapperProfile : Profile
    {

        public APIMapperProfile()
        {
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDTO, Order>();

            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerDTO, Customer>();

            CreateMap<Location, LocationDTO>();
            CreateMap<LocationDTO, Location>();

            CreateMap<Manager, ManagerDTO>();
            CreateMap<ManagerDTO, Manager>();

            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();

            CreateMap<OrderLineItem, OLIDTO>();
            CreateMap<OLIDTO, OrderLineItem>();

            CreateMap<InventoryLineItem, ILIDTO>();
            CreateMap<ILIDTO, InventoryLineItem>();

        }






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


        #region Order Line Item Mapping
        public OLIDTO ParseOLI(OrderLineItem order)
        {
            return new OLIDTO(order.OrderId,order.ProductId, order.ProductQuantity);
        }

        public List<OLIDTO> ParseOLIs(List<OrderLineItem> olis)
        {
            List<OLIDTO> oliDTOs = new List<OLIDTO>();
            foreach (OrderLineItem oli in olis)
            {
                oliDTOs.Add(ParseOLI(oli));
            }
            return oliDTOs;
        }

        public OrderLineItem ParseOLIDTO(OLIDTO orderDTO)
        {
            return new OrderLineItem(orderDTO.OrderId, orderDTO.ProductId, orderDTO.ProductQuantity);
        }

        public List<OrderLineItem> ParseOLIDTOs(List<OLIDTO> orderDTOs)
        {
            List<OrderLineItem> orders = new List<OrderLineItem>();
            foreach (OLIDTO orderDTO in orderDTOs)
            {
                orders.Add(ParseOLIDTO(orderDTO));
            }
            return orders;
        }
        #endregion


    }
}
