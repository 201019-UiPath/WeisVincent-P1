using IceShopBL;
using IceShopDB.Models;
using IceShopDB.Repos;
using Serilog;
using System;
using System.Collections.Generic;

namespace IceShopUI.Menus.CustomerMenus
{
    internal class CustomerOrderMenu : Menu, IMenu
    {

        private readonly Customer CurrentCustomer;

        private OrderBuilder OrderBuilder;

        private bool CartHasItems;

        public CustomerOrderMenu(ref Customer currentCustomer, ref Location selectedLocation, ref IRepository repo) : base(ref repo)
        {
            CurrentCustomer = currentCustomer;
            CartHasItems = false;

            OrderBuilder = new OrderBuilder(ref CurrentCustomer, ref selectedLocation, ref Repo);
        }




        public override void SetStartingMessage()
        {
            if (OrderBuilder.OrderCart.Count > 0)
            {
                CartHasItems = true;
            };
            StartMessage = "Select a product available at this location to order.";
        }

        public override void SetUserChoices()
        {
            PossibleOptions = new List<string>(OrderBuilder.SelectedLocationStock.Count);

            PossibleOptions.AddRange(ReturnAvailableProductsAsStrings());

            if (CartHasItems)
            {
                PossibleOptions.Add("Use this option to view and edit your order.");
            };

            PossibleOptions.Add("Use this option to go back, cancelling your order.");

        }

        public override void ExecuteUserChoice()
        {
            InventoryLineItem selectedLineItem;

            for (int i = 1; i < PossibleOptions.Count; i++)
            {
                if (CartHasItems)
                {
                    if (selectedChoice == PossibleOptions.Count - 1)
                    {
                        EditOrder();
                        break;
                    }
                }
                if (selectedChoice == PossibleOptions.Count)
                {
                    GoBackToCustomerMenu();
                    break;
                }

                if (selectedChoice == i)
                {
                    try
                    {
                        selectedLineItem = OrderBuilder.SelectedLocationStock[i - 1];

                        int productQuantity = OrderBuilder.GetAvailableQuantityOfInventoryLineItem(selectedLineItem);

                        if (productQuantity > 0)
                        {
                            // This submenu will stage the next line item for order itself.
                            new CustomerLineItemQuantitySubMenu(ref selectedLineItem, ref OrderBuilder, ref Repo).Run();
                        }
                        else
                        {
                            Console.WriteLine("Can't order this one, chief. The rest of its stock is in your cart. Silly goose.");
                        }

                        // Reset this menu with the updated data.
                        RunAgain();
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Log.Error(e.Message);
                    }
                }
            }

        }

        // TODO: Move this to a UI class.
        public List<string> ReturnAvailableProductsAsStrings()
        {
            Log.Logger.Information("Processing available inventory stock as strings to be displayed in the console..");
            if (OrderBuilder.SelectedLocationStock.Count < 1)
            {
                Log.Error("The stock of the user's selected location was empty when used to display options. Can't successfully display non-existent options.");
            }
            List<string> availableItems = new List<string>(OrderBuilder.SelectedLocationStock.Count);

            foreach (InventoryLineItem entry in OrderBuilder.SelectedLocationStock)
            {
                Product product = entry.Product;

                int productQuantity = OrderBuilder.GetAvailableQuantityOfInventoryLineItem(entry);

                string productType = Enum.GetName(typeof(ProductType), product.TypeOfProduct);

                availableItems.Add($"{product.Name}: {product.Description} Part of our {productType} collection. Quantity: {productQuantity}");
            }

            return availableItems;
        }


        private void EditOrder()
        {
            CustomerOrderEditorSubMenu OrderEditor = new CustomerOrderEditorSubMenu(ref OrderBuilder, ref Repo);
            OrderEditor.Run();
            RunAgain();
        }

        private void RunAgain()
        {
            Console.WriteLine("\nWant to add more suffering to your cart?");
            Run();
        }

        public void GoBackToCustomerMenu()
        {
            IMenu nextMenu = new CustomerStartMenu(CurrentCustomer, ref Repo);
            Console.WriteLine("Going back, ZOOM");
            MenuManager.Instance.ReadyNextMenu(nextMenu);
        }

    }
}