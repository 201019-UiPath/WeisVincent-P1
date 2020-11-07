using IceShopDB.Models;
using IceShopDB.Repos;
using Serilog;
using System;
using System.Collections.Generic;

namespace IceShopUI.Menus.CustomerMenus
{
    internal class CustomerStartMenu : Menu, IMenu
    {

        private readonly Customer CurrentCustomer;

        private IMenu NextMenu;
        public CustomerStartMenu(Customer customer, ref IRepository repo) : base(ref repo)
        {
            CurrentCustomer = customer;
        }

        public override void SetStartingMessage()
        {
            StartMessage = $"Welcome, loyal sufferer {CurrentCustomer.Name}! \n You may view your order history, or select a location to begin placing your order!";
        }

        public override void SetUserChoices()
        {
            PossibleOptions = new List<string>() {
                "View Order History.",
                "Select a location to waste money at.",
                "Exit"
            };
        }


        public override void ExecuteUserChoice()
        {
            switch (selectedChoice)
            {
                case 1:
                    NextMenu = new CustomerOrderHistoryMenu(CurrentCustomer, ref Repo);
                    break;
                case 2:
                    NextMenu = new CustomerLocationSelectionMenu(CurrentCustomer, ref Repo);
                    break;
                case 3:
                    Console.WriteLine("Enjoy your suffering!");
                    return;
                //Environment.Exit(Environment.ExitCode);
                //break;
                default:
                    throw new NotImplementedException();
                    //break;
            }

            try
            {
                MenuManager.Instance.ReadyNextMenu(NextMenu);
            }
            catch (NullReferenceException e)
            {
                Log.Error(e.Message);
            }


        }



    }
}
