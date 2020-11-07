using IceShopBL;
using IceShopDB.Models;
using IceShopLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace IceShopUI.Menus
{
    internal static class OrderHistoryUtility
    {

        #region Order History methods
        internal static void ShowOrderHistory(ref List<Order> ordersToBeShown, ref OrderService orderService, bool isIteratedForward)
        {
            if (!isIteratedForward)
            {
                ordersToBeShown.Reverse();
            }
            for (int i = 0; i < ordersToBeShown.Count; i++)
            {
                DisplayOrderInformation(ordersToBeShown[i], ref orderService);
                if (ordersToBeShown[i] == null)
                {
                    Console.WriteLine("okay then, thanks order");
                    Environment.Exit(0);
                }
            }
        }


        internal static void DisplayOrderInformation(Order order, ref OrderService orderService)
        {
            List<OrderLineItem> OrderedProductsInOrder = orderService.GetAllProductsInOrder(order);

            DateTime orderTime = DateTimeUtility.GetDateTimeFromUnixEpochAsDouble(order.TimeOrderWasPlaced);
            if (OrderedProductsInOrder == null)
            {
                Console.WriteLine("okay then, thanks time");
                Environment.Exit(0);
            }

            StringBuilder orderString = new StringBuilder(
                $"#{order.Id} at {order.Location.Name} on {orderTime.ToShortDateString()}\n_________________\n    Subtotal: ${order.Subtotal}\n    Products in Order: \n"
                );
            foreach (OrderLineItem orderedProduct in OrderedProductsInOrder)
            {
                orderString.Append($"      {orderedProduct.ProductQuantity} of {orderedProduct.Product.Name} \n");
            }
            string resultingString = orderString.ToString();

            Console.WriteLine(resultingString);
        }

        internal static async void DisplayOrderInformationAsync(Order order, OrderService orderService)
        {
            List<OrderLineItem> OrderedProductsInOrder = await orderService.GetAllProductsInOrderAsync(order);

            DateTime orderTime = DateTimeUtility.GetDateTimeFromUnixEpochAsDouble(order.TimeOrderWasPlaced);

            StringBuilder orderString = new StringBuilder(
                $"#{order.Id} at {order.Location.Name} on {orderTime.ToShortDateString()}\n_________________\n    Subtotal: ${order.Subtotal}\n    Products in Order: \n"
                );
            foreach (OrderLineItem orderedProduct in OrderedProductsInOrder)
            {
                orderString.Append($"      {orderedProduct.ProductQuantity} of {orderedProduct.Product.Name} \n");
            }
            string resultingString = orderString.ToString();
            Console.WriteLine(resultingString);
        }

        public static void ProcessSortingByDate(ref bool sortOrder)
        {

            string sortStartMessage = "Would you like the results sorted oldest to latest or latest to oldest?";
            List<string> sortOptions = new List<string>(2)
                    {
                        "Oldest to Latest.", "Latest to Oldest."
                    };
            UserResponseUtility.DisplayPossibleChoicesToUser(sortStartMessage, sortOptions);
            sortOrder = (UserRequestUtility.ProcessUserInputAgainstPossibleChoices(sortOptions)) switch
            {
                1 => true,
                2 => false,
                _ => throw new NotImplementedException(),
            };
        }

        public static void ProcessSortingByPrice(ref bool sortOrder)
        {

            string sortStartMessage = "You've chosen to sort orders by subtotal. Sort from lowest to highest subtotal, or highest to lowest subtotal?";
            List<string> sortOptions = new List<string>(2)
                    {
                        "Lowest to highest.", "Highest to Lowest."
                    };
            UserResponseUtility.DisplayPossibleChoicesToUser(sortStartMessage, sortOptions);
            sortOrder = (UserRequestUtility.ProcessUserInputAgainstPossibleChoices(sortOptions)) switch
            {
                1 => true,
                2 => false,
                _ => throw new NotImplementedException(),
            };
        }


        #endregion

    }
}
