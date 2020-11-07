using IceShopDB.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceShopDB.Repos.DBRepos
{
    public class DBRepo : IRepository
    {

        private readonly IceShopContext context;

        public DBRepo(IceShopContext context)
        {
            this.context = context;
        }

        #region Customer Methods
        public void AddCustomerAsync(Customer customer)
        {
            context.Customers.AddAsync(customer);
        }

        public void AddCustomer(Customer customer)
        {
            context.Customers.Add(customer);
        }

        public Task<List<Customer>> GetAllCustomersAsync()
        {
            return context.Customers.ToListAsync();
        }

        public List<Customer> GetAllCustomers()
        {
            return context.Customers.ToList();
        }


        public Task<List<Order>> GetAllOrdersForCustomerAsync(int customerId)
        {
            return context.Orders.Include("Location").Include("OrderLineItems").Where(o => o.CustomerId == customerId).ToListAsync();
        }

        public List<Order> GetAllOrdersForCustomer(int customerId)
        {
            return context.Orders.Include("Location").Include("OrderLineItems").Where(o => o.CustomerId == customerId).ToList();
        }

        public Task<Customer> GetCustomerByEmailAsync(string email)
        {
            return context.Customers.Where(c => c.Email == email).FirstAsync();
        }

        public Customer GetCustomerByEmail(string email)
        {
            Customer customerWithEmail;
            try
            {
                customerWithEmail = context.Customers.Where(m => m.Email == email).First();
            }
            catch (InvalidOperationException e)
            {
                Log.Error(e.Message);
                customerWithEmail = null;
            }

            return customerWithEmail;
        }

        #endregion


        #region Manager Methods
        public void AddManager(Manager manager)
        {
            context.Managers.Add(manager);
        }

        public Task<List<Manager>> GetAllManagersAsync()
        {
            return context.Managers.ToListAsync();
        }

        public List<Manager> GetAllManagers()
        {
            return context.Managers.ToList();
        }

        public Task<Manager> GetManagerByEmailAsync(string email)
        {
            Task<Manager> managerWithEmail;
            try
            {
                managerWithEmail = context.Managers.Include("Location").Where(m => m.Email == email).FirstAsync();
            }
            catch (InvalidOperationException e)
            {
                Log.Error(e.Message);
                managerWithEmail = null;
            }

            return managerWithEmail;
        }

        public Manager GetManagerByEmail(string email)
        {
            Manager managerWithEmail;
            try
            {
                managerWithEmail = context.Managers.Include("Location").Where(m => m.Email == email).First();
            }
            catch (InvalidOperationException e)
            {
                Log.Error(e.Message);
                managerWithEmail = null;
            }

            return managerWithEmail;
        }

        #endregion


        //TODO: Implement everything under this
        #region Location Methods
        public List<Location> GetAllLocations()
        {
            return context.Locations.ToList();
        }

        public Task<List<Location>> GetAllLocationsAsync()
        {
            return context.Locations.ToListAsync();
        }

        public List<Order> GetAllOrdersForLocation(int locationID)
        {
            return context.Orders.Where(o => o.LocationId == locationID).ToList();
        }

        public Task<List<Order>> GetAllOrdersForLocationAsync(int locationID)
        {
            return context.Orders.Where(o => o.LocationId == locationID).ToListAsync();
        }

        public List<InventoryLineItem> GetAllInventoryLineItemsAtLocation(int locationID)
        {
            return context.InventoryLineItems.Include("Product").Include("Location").Where(ie => ie.LocationId == locationID).ToList();
        }

        public Task<List<InventoryLineItem>> GetAllInventoryLineItemsAtLocationAsync(int locationID)
        {
            return context.InventoryLineItems.Include("Product").Include("Location").Where(ie => ie.LocationId == locationID).ToListAsync();
        }

        #endregion


        #region Product methods
        public void AddNewProductToStock(int newProductId, int locationId)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveProductAtLocation(int removedProduct, int locationId)
        {
            throw new System.NotImplementedException();
        }

        public void AddInventoryLineItem(InventoryLineItem lineItem)
        {
            context.InventoryLineItems.Add(lineItem);
        }

        public void UpdateInventoryLineItem(InventoryLineItem lineItem)
        {
            context.InventoryLineItems.Update(lineItem);
        }

        public void RemoveInventoryLineItem(InventoryLineItem lineItem)
        {
            context.InventoryLineItems.Remove(lineItem);
        }


        #endregion



        public List<OrderLineItem> GetOrderedProductsInAnOrder(int orderId)
        {
            // TODO: Remember using AsNoTracking();
            return context.OrderLineItems.Include("Product").Include("Order").Where(op => op.OrderId == orderId)/*.AsNoTracking()*/.ToList();
        }

        public Task<List<OrderLineItem>> GetOrderedProductsInAnOrderAsync(int orderId)
        {
            return context.OrderLineItems.Include("Product").Include("Order").Where(op => op.OrderId == orderId).ToListAsync();
        }



        #region Order methods

        public void AddOrder(Order order)
        {
            context.Orders.Add(order);
        }
        public void AddOrderAsync(Order order)
        {
            context.Orders.AddAsync(order);
        }

        public void RemoveInventoryLineItemFromLocation(InventoryLineItem lineItem)
        {
            context.InventoryLineItems.Remove(lineItem);
        }

        public void RemoveInventoryLineItemsFromLocation(List<InventoryLineItem> lineItems)
        {
            context.InventoryLineItems.RemoveRange(lineItems);
        }
        #endregion


        public async void SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void SetSavePoint()
        {
            //TODO: Possible set of savepoint for 
        }

        public void RollbackChanges()
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAllProducts()
        {
            return context.Products.ToList();
        }

        public Task<List<Product>> GetAllProductsAsync()
        {
            Task<List<Product>> getProducts = context.Products.ToListAsync();
            return getProducts;
        }

        public void AddNewProduct(Product product)
        {
            context.Products.Add(product);
        }
    }
}
