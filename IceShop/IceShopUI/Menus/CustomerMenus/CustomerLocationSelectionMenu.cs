using IceShopBL;
using IceShopDB.Models;
using IceShopDB.Repos;
using Serilog;
using System;
using System.Collections.Generic;

namespace IceShopUI.Menus.CustomerMenus
{
    internal class CustomerLocationSelectionMenu : Menu, IMenu
    {
        private Customer CurrentCustomer;
        private readonly LocationService LocationService;
        private readonly List<Location> AllLocations;
        public CustomerLocationSelectionMenu(Customer currentCustomer, ref IRepository repo) : base(ref repo)
        {
            CurrentCustomer = currentCustomer;

            LocationService = new LocationService(ref Repo);
            AllLocations = LocationService.GetAllLocations();
        }

        public override void SetStartingMessage()
        {
            StartMessage = "Ready to buy some suffering? Pick a location to get your pain from.";
        }

        public override void SetUserChoices()
        {
            PossibleOptions = new List<string>(AllLocations.Count);
            foreach (Location location in AllLocations)
            {
                PossibleOptions.Add($"{location.Name}, located at: {location.Address}");
            }
            PossibleOptions.Add("Use this option to go back.");
        }

        public override void ExecuteUserChoice()
        {
            IMenu nextMenu = null;
            Location selectedLocation;

            for (int i = 1; i < PossibleOptions.Count; i++)
            {
                if (selectedChoice == PossibleOptions.Count)
                {
                    Console.WriteLine("Going back, ZOOM");
                    nextMenu = new CustomerStartMenu(CurrentCustomer, ref Repo);
                    break;
                }

                if (selectedChoice == i)
                {
                    try
                    {
                        selectedLocation = AllLocations[i - 1];
                        nextMenu = new CustomerOrderMenu(ref CurrentCustomer, ref selectedLocation, ref Repo);
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Log.Error(e.Message);
                    }
                }
            }

            MenuManager.Instance.ReadyNextMenu(nextMenu);

        }


    }
}