using IceShopBL;
using IceShopDB.Models;
using IceShopDB.Repos;
using Serilog;
using System;
using System.Collections.Generic;

namespace IceShopUI.Menus.ManagerMenus
{
    internal class ManagerProductAdditionSubMenu : Menu, IMenu
    {
        private readonly Location CurrentLocation;
        private readonly List<InventoryLineItem> locationStock;
        private readonly LocationService locationService;
        private readonly ProductService productService;

        private readonly List<Product> UnstockedProducts;

        public ManagerProductAdditionSubMenu(ref Location location, ref List<InventoryLineItem> locationStock, ref LocationService locationService, ref IRepository repo) : base(ref repo)
        {
            Log.Logger.Information("Instantiated Customer Order Editor Submenu.");
            CurrentLocation = location;
            if (locationStock != null)
            {
                this.locationStock = locationStock;
            }
            else locationStock = new List<InventoryLineItem>();

            this.locationService = locationService;
            productService = new ProductService(ref Repo);
            UnstockedProducts = GetProductsNotStockedAtLocation();
        }

        public override void SetStartingMessage()
        {
            StartMessage = $"You have chosen to add a new product to the stock of {CurrentLocation.Name}. What product do you want to add?";
        }

        public override void SetUserChoices()
        {
            PossibleOptions = new List<string>();
            for (int i = 1; i <= UnstockedProducts.Count; i++)
            {
                Product product = UnstockedProducts[i - 1];
                PossibleOptions.Add($"{product.Name}!   From our {product.TypeOfProductAsString} Suffering catalogue: \n    Price is ${product.Price}.\n    {product.Description}");
            }
            PossibleOptions.Add("Use this option to add a brand spanking new product.");
            PossibleOptions.Add("Use this option to go back.");
        }

        public override void ExecuteUserChoice()
        {
            for (int i = 1; i < PossibleOptions.Count; i++)
            {
                if (selectedChoice == PossibleOptions.Count)
                {
                    return;
                    //break;
                }

                if (selectedChoice == PossibleOptions.Count - 1)
                {
                    Product newProduct = InputBrandNewProduct();
                    productService.AddNewProduct(newProduct);
                    locationService.AddInventoryLineItemInRepo(new InventoryLineItem(CurrentLocation, newProduct, 1));
                    break;
                }


                if (selectedChoice == i)
                {
                    try
                    {
                        // Add one of the new product.
                        locationService.AddInventoryLineItemInRepo(new InventoryLineItem(CurrentLocation, UnstockedProducts[i - 1], 1));
                        break;
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Log.Error(e.Message);
                    }
                }
            }
        }

        public List<Product> GetProductsNotStockedAtLocation()
        {
            List<Product> allPossibleProducts = productService.GetAllProducts();
            List<Product> currentlyStockedProducts = new List<Product>();
            List<Product> unstockedProducts = new List<Product>();
            foreach (InventoryLineItem inventoryLineItem in locationStock)
            {
                currentlyStockedProducts.Add(inventoryLineItem.Product);
            }

            foreach (Product product in allPossibleProducts)
            {
                if (currentlyStockedProducts.Exists(p => p == product))
                {
                    continue;
                }
                unstockedProducts.Add(product);
            }

            return unstockedProducts;
        }



        private Product InputBrandNewProduct()
        {
            string newName = UserRequestUtility.QueryProductName();

            // TODO: Allow prices to be decimal numbers with two decimal points. More validation.
            Console.WriteLine("Enter a valid whole number price. We don't do that round-up 59.99 garbage here.");
            double newPrice = UserRequestUtility.QueryQuantity();

            ProductType newType = ProductType.Metaphysical;

            List<string> possibleTypes = new List<string>();
            foreach (string type in Enum.GetNames(typeof(ProductType)))
            {
                possibleTypes.Add(type);
            }
            UserResponseUtility.DisplayPossibleChoicesToUser("What type of suffering is this suffering?", possibleTypes);
            int selectedType = UserRequestUtility.ProcessUserInputAgainstPossibleChoices(possibleTypes);
            foreach (int type in Enum.GetValues(typeof(ProductType)))
            {
                if (selectedType == type)
                {
                    string newTypeAsString = Enum.GetNames(typeof(ProductType))[type];
                    newType = Enum.Parse<ProductType>(newTypeAsString);
                }
            }

            string newDescription = UserRequestUtility.QueryDescription();

            return new Product(newName, newPrice, newType, newDescription);
        }


    }
}