using IceShopBL;
using IceShopDB.Models;
using IceShopDB.Repos;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IceShopUI.Menus.ManagerMenus
{
    internal class ManagerLocationOrderHistoryMenu : Menu, IMenu
    {
        private readonly Manager currentManager;
        private List<Order> LocationOrders;

        private readonly LocationService LocationService;

        public ManagerLocationOrderHistoryMenu(Manager currentManager, ref IRepository repo) : base(ref repo)
        {
            this.currentManager = currentManager;
            LocationService = new LocationService(ref Repo);
        }

        public override void SetStartingMessage()
        {
            StartMessage = "You've chosen to view this location's rich history of suffering. \n How would you like the results sorted?";
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
            LocationOrders = LocationService.GetAllOrdersForLocation(currentManager.Location);

            List<Order> sortedOrderList = new List<Order>(LocationOrders.Count);
            bool IsSortOrderForward = false;


            IMenu previousMenu = new ManagerStartMenu(currentManager, ref Repo);

            switch (selectedChoice)
            {
                case 1:
                    sortedOrderList = LocationOrders.OrderBy(o => o.Subtotal).ToList();
                    OrderHistoryUtility.ProcessSortingByDate(ref IsSortOrderForward);

                    break;
                case 2:
                    sortedOrderList = LocationOrders.OrderBy(o => o.TimeOrderWasPlaced).ToList();
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
                OrderService orderService = new OrderService(ref Repo);
                OrderHistoryUtility.ShowOrderHistory(ref sortedOrderList, ref orderService, IsSortOrderForward);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("No orders were found for this location. Cry about it, I suppose.");
                Log.Error(e.Message);
            }

            Console.WriteLine("Really interesting list, right?");


            MenuManager.Instance.ReadyNextMenu(previousMenu);
        }


    }
}