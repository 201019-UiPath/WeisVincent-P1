using IceShopBL;
using IceShopDB.Models;
using IceShopDB.Repos;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IceShopUI.Menus.CustomerMenus
{
    internal class CustomerOrderHistoryMenu : Menu, IMenu
    {
        private readonly Customer CurrentCustomer;
        private readonly CustomerService CustomerService;
        private List<Order> CustomerOrders;
        private OrderService OrderService;

        public CustomerOrderHistoryMenu(Customer customer, ref IRepository repo) : base(ref repo)
        {
            CurrentCustomer = customer;
            CustomerService = new CustomerService(ref Repo);
            OrderService = new OrderService(ref Repo);

        }

        public override void SetStartingMessage()
        {
            StartMessage = "You've chosen to view your rich history of suffering. \nHow would you like the results sorted?";
        }

        public override void SetUserChoices()
        {
            PossibleOptions = new List<string>() {
                "Sort by date the order was placed.",
                "Sort by price.",
                "Go back."
            };
        }




        public override void ExecuteUserChoice()
        {
            CustomerOrders = CustomerService.GetAllOrdersForCustomer(CurrentCustomer);

            List<Order> sortedOrderList = new List<Order>(CustomerOrders.Count);

            sortedOrderList = CustomerOrders;

            bool IsSortOrderForward = false;

            IMenu previousMenu = new CustomerStartMenu(CurrentCustomer, ref Repo);

            switch (selectedChoice)
            {
                case 1:
                    sortedOrderList = CustomerOrders.OrderBy(o => o.Subtotal).ToList();
                    OrderHistoryUtility.ProcessSortingByDate(ref IsSortOrderForward);
                    break;
                case 2:
                    sortedOrderList = CustomerOrders.OrderBy(o => o.TimeOrderWasPlaced).ToList();
                    OrderHistoryUtility.ProcessSortingByPrice(ref IsSortOrderForward);
                    break;
                case 3:
                    Console.WriteLine("Going back.");
                    MenuManager.Instance.ReadyNextMenu(previousMenu);
                    return;
                //break;
                default:
                    sortedOrderList = null;
                    throw new NotImplementedException();
                    //break;
            }

            try
            {
                OrderHistoryUtility.ShowOrderHistory(ref sortedOrderList, ref OrderService, IsSortOrderForward);
                Console.WriteLine("Really interesting list, right?");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("No orders were found for this account. Cry about it, I suppose.");
                Log.Information($"{e.Message}");
            }

            MenuManager.Instance.ReadyNextMenu(previousMenu);

        }



    }
}


