﻿using IceShopDB.Models;
using IceShopDB.Repos;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceShopBL.Services
{
    /// <summary>
    /// This class handles order-specific business logic for the IceShop using a repository that implements IRepository.
    /// This includes logic that happens during the submission of a new order, such as updating inventory and adding a new order entry.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IRepository Repo;

        public OrderService(IRepository repo)
        {
            Repo = repo;
        }

        public OrderService(ref IRepository repo)
        {
            Repo = repo;
        }

        public void AddOrderLineItemToRepo(OrderLineItem oli)
        {
            var existingOrderedProducts = Repo.GetOrderedProductsInAnOrder(oli.OrderId);
            if (existingOrderedProducts.Exists(li => li.ProductId == oli.ProductId))
            {
                existingOrderedProducts.First(li => li.ProductId == oli.ProductId).ProductQuantity += oli.ProductQuantity;
            } else
            {
                Repo.AddOrderLineItem(oli);
            }
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

        public Order GetOrderByDateTime(double dateTimeDouble)
        {
            return Repo.GetOrderByDateTime(dateTimeDouble);
        }

        public Order GetOrderById(int orderId)
        {
            return Repo.GetOrderById(orderId);
        }
    }
}
