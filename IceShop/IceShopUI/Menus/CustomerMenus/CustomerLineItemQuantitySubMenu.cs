using IceShopBL;
using IceShopDB.Models;
using IceShopDB.Repos;
using Serilog;
using System;
using System.Collections.Generic;

namespace IceShopUI.Menus.CustomerMenus
{
    internal class CustomerLineItemQuantitySubMenu : Menu, IMenu
    {
        private readonly InventoryLineItem SelectedLineItem;
        private readonly Product selectedProduct;

        /// <summary>
        /// This number cannot be less than one, and should be as great as the number of the selected product the user can buy.
        /// </summary>
        private readonly int maxQuantity;

        private int selectedQuantity;

        private readonly OrderBuilder OrderBuilder;
        public CustomerLineItemQuantitySubMenu(ref InventoryLineItem selectedLineItem, ref OrderBuilder orderBuilder, ref IRepository repo) : base(ref repo)
        {
            Log.Logger.Information("Instantiated Customer Line Item Quantity Selection Submenu.");
            SelectedLineItem = selectedLineItem;
            selectedProduct = selectedLineItem.Product;
            OrderBuilder = orderBuilder;
            StagedLineItem stagedLineItem = OrderBuilder.GetStagedLineItemForAffectedLineItemIfItExists(selectedLineItem);
            if (stagedLineItem != null)
            {
                maxQuantity = stagedLineItem.GetNewQuantityOfAffectedInventoryLineItem();
            }
            else maxQuantity = selectedLineItem.ProductQuantity;
        }


        public override void SetStartingMessage()
        {
            StartMessage = $"You have selected {selectedProduct.Name}, for some great {Enum.GetName(typeof(ProductType), selectedProduct.TypeOfProduct)} suffering. How much would you like?";
        }

        public override void SetUserChoices()
        {
            PossibleOptions = new List<string>(maxQuantity + 1);

            for (int i = 1; i <= maxQuantity; i++)
            {
                if (i == 1)
                {
                    PossibleOptions.Add($"{i} unit of {selectedProduct.Name}.");
                }
                else
                {
                    PossibleOptions.Add($"{i} units of {selectedProduct.Name}.");
                }

            }
            //TODO: Add a way to back out of the product order quantity menu.
        }

        public override void ExecuteUserChoice()
        {
            for (int i = 1; i <= PossibleOptions.Count; i++)
            {
                if (selectedChoice == i)
                {
                    try
                    {
                        selectedQuantity = i;
                        OrderBuilder.StageProductForOrder(SelectedLineItem, selectedQuantity);
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Console.WriteLine("You selected a quantity you shouldn't be able to select. I am breaking apart now.");
                        Log.Error(e.Message);
                        Environment.Exit(0);
                    }
                }
            }
        }



    }
}