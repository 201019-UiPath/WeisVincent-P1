using IceShopWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IceShopWeb.Controllers
{
    [Route("staff")]
    public class ManagerController : Controller
    {

        private Manager CurrentManager
        {
            get { return HttpContext.Session.Get<Manager>("CurrentManager"); }
            set { 
                HttpContext.Session.Set("CurrentManager", value);
                var locations = this.GetDataAsync<List<Location>>($"location/get").Result;
                ManagedLocation = locations.Where(l => l.Id == value.LocationId).First();
            }
        }

        private List<InventoryLineItem> CurrentStock
        {
            get { return HttpContext.Session.Get<List<InventoryLineItem>>("CurrentStock"); }
            set { HttpContext.Session.Set<List<InventoryLineItem>>("CurrentStock", value); }
        }

        private Location ManagedLocation 
        { 
            get { return HttpContext.Session.Get<Location>("ManagedLocation"); } 
            set { CurrentStock = null; HttpContext.Session.Set<Location>("ManagedLocation", value); } 
        }

        private readonly IActionResult LoginRedirectAction;
        private readonly Task<IActionResult> LoginRedirectActionTask;
        public ManagerController()
        {
            LoginRedirectAction = RedirectToAction("Login", "Manager", "-1");
            LoginRedirectActionTask = Task.Factory.StartNew(() => LoginRedirectAction);
        }

        [HttpGet]
        [Route("login")]
        public ViewResult Login(int? sessionExists)
        {
            if (sessionExists == 0)
            {
                ViewData["Redirect"] = "Your session does not exist. Please sign in.";
            }
            return View();
        }

        [Route("login")]
        public async Task<IActionResult> Login(AuthPack userInput)
        {
            if (ModelState.IsValid)
            {
                string inputEmail = userInput.Email;
                string inputPassword = userInput.Password;

                try
                {
                    string request = $"manager/get/{inputEmail}";
                    var resultManager = await this.GetDataAsync<Manager>(request);
                    if (resultManager.Password == inputPassword && resultManager.Email == inputEmail)
                    {
                        CurrentManager = resultManager;
                        return RedirectToAction("Index", "Manager");
                    }
                    else
                    {
                        // TODO: Show the user the login failed.

                        return View(userInput);
                    };
                }
                catch (HttpRequestException)
                {
                    // TODO: Add information for the user if the request failed.
                    return View(userInput);
                }
                catch (NullReferenceException)
                {
                    // TODO: Add information for the user if there was no customer returned.
                    return View(userInput);
                }
            }

            return View(userInput);
        }


        [Route("location/orders")]
        public async Task<IActionResult> GetLocationOrderHistory(int? sortBy)
        {
            if (CurrentManager == null)
            {
                return await LoginRedirectActionTask;
            }

            string request = $"location/orders/get/{ManagedLocation.Id}";
            var receivedOrders = await this.GetDataAsync<List<Order>>(request);

            string locationRequest = $"location/get";
            var receivedLocations = await this.GetDataAsync<List<Location>>(locationRequest);

            foreach (Order order in receivedOrders)
            {
                order.Location = receivedLocations.Single(l => l.Id == order.LocationId);
            }

            List<Order> sortedOrders = receivedOrders;
            sortedOrders = sortBy switch
            {
                0 => receivedOrders.OrderBy(o => o.TimeOrderWasPlaced).ToList(),
                1 => receivedOrders.OrderBy(o => o.TimeOrderWasPlaced).Reverse().ToList(),
                2 => receivedOrders.OrderBy(o => o.Subtotal).ToList(),
                3 => receivedOrders.OrderBy(o => o.Subtotal).Reverse().ToList(),
                _ => receivedOrders
            };
            ViewData["LocationName"] = ManagedLocation.Name;
            if (receivedOrders != default) return View(sortedOrders); else return StatusCode(500);
            //return resultView;

            return StatusCode(500);
        }

        [Route("location/orders/details")]
        public async Task<IActionResult> ViewOrderDetails(int orderId)
        {
            if (CurrentManager == null) return await LoginRedirectActionTask;
            string olisRequest = $"order/products/get/{orderId}";
            var receivedOrderLineItems = await this.GetDataAsync<List<OrderLineItem>>(olisRequest);
            string productsRequest = $"product/get";
            var receivedProducts = await this.GetDataAsync<List<Product>>(productsRequest);

            if (receivedOrderLineItems == null)
            {
                return StatusCode(500);
            }


            foreach (OrderLineItem oli in receivedOrderLineItems)
            {
                oli.Product = receivedProducts.Single(p => p.Id == oli.ProductId);
            }

            return View(receivedOrderLineItems);
        }


        [Route("location/stock/{locationId}")]
        public async Task<IActionResult> ManageLocationStock(int locationId)
        {
            if (ManagedLocation == null)
            {
                string requestLocations = $"location/get";
                var locations = await this.GetDataAsync<List<Location>>(requestLocations);
                ManagedLocation = locations.Where(l => l.Id == locationId).First();
            };
            if (CurrentManager == null) return await LoginRedirectActionTask;

            string request = $"location/stock/get/{locationId}";
            var stock = await this.GetDataAsync<List<InventoryLineItem>>(request);

            CurrentStock = stock;

            var getProcessedStock = Task<List<InventoryLineItem>>.Factory.StartNew(() => { return ReturnStockWithProductData(stock); });

            string productsRequest = $"product/get";
            var allProducts = await this.GetDataAsync<List<Product>>(productsRequest);

            List<InventoryLineItem> processedStock = await getProcessedStock;
            Task<List<Product>> getProductsInStock = Task<List<Product>>.Factory.StartNew(
                () => {
                    var stockedProducts = new List<Product>(allProducts.Count);
                    foreach (InventoryLineItem ili in processedStock)
                    {
                        stockedProducts.Add(ili.Product);
                    }
                    return stockedProducts.Distinct().ToList();
                });

            List<Product> productsInStock = await getProductsInStock;

            var getUnstockedProducts = Task<List<Product>>.Factory.StartNew(() =>
            {
                var productsNotStocked = new List<Product>();
                foreach(Product product in allProducts)
                {
                    if (!productsInStock.Any(p=>p.Id == product.Id))
                    {
                        productsNotStocked.Add(product);
                    }
                }
                return productsNotStocked;
            });
            
            
            ViewData["LocationName"] = ManagedLocation.Name;
            ViewData["StockData"] = CurrentStock;
            ViewData["UnstockedProducts"] = await getUnstockedProducts;

            return View(processedStock);
        }

        [Route("location/stock/add")]
        public async Task<IActionResult> AddNewProductToStock(int productId)
        {
            if (CurrentManager == null) return await LoginRedirectActionTask;
            if (CurrentStock == null) return RedirectToAction("ManageLocationStock", "Manager");

            try
            {
                var newLineItem = new InventoryLineItem(ManagedLocation.Id, productId, 1);
                string newILIRequest = $"location/stock/add";

                await this.PostDataAsync(newILIRequest, newLineItem);
            }
            catch (HttpRequestException e)
            {
                //TODO: Do something if the request to the API fails.
                var message = e.Message;
                Console.WriteLine(message);
            }
            return RedirectToAction("ManageLocationStock", new { locationId = ManagedLocation.Id });
        }




        [Route("location/stock/increment")]
        public async Task<IActionResult> IncrementStockItem(InventoryLineItem ili)
        {
            if (CurrentManager == null) return await LoginRedirectActionTask;
            if (CurrentStock == null) return await Task.Factory.StartNew(()=> RedirectToAction("ManageLocationStock", "Manager"));

            InventoryLineItem processedILI = await GetProductForILIAsync(ili);
            List<InventoryLineItem> tempStock = CurrentStock;

            try
            {
                var incrementedLineItem = tempStock.Where(sli => sli.ProductId == processedILI.ProductId).First();
                incrementedLineItem.ProductQuantity += 1;
                string updateILIRequest = $"location/stock/update";

                await this.PutDataAsync(updateILIRequest, incrementedLineItem);
            }
            catch (HttpRequestException e)
            {
                //TODO: Do something if the request to the API fails.
                var message = e.Message;
                Console.WriteLine(message);
            }
            return RedirectToAction("ManageLocationStock", new { locationId = ManagedLocation.Id });
        }

        [Route("location/stock/remove")]
        public async Task<IActionResult> DecrementStockItem(InventoryLineItem ili)
        {
            if (CurrentManager == null) return await LoginRedirectActionTask;
            if (CurrentStock == null) return await Task.Factory.StartNew(() => RedirectToAction("ManageLocationStock", "Manager"));

            InventoryLineItem processedILI = await GetProductForILIAsync(ili);
            List<InventoryLineItem> tempStock = CurrentStock;

            try
            {
                var incrementedLineItem = tempStock.Where(sli => sli.ProductId == processedILI.ProductId).First();
                incrementedLineItem.ProductQuantity -= 1;
                string updateILIRequest = $"location/stock/update";

                await this.PutDataAsync(updateILIRequest, incrementedLineItem);
            }
            catch (HttpRequestException e)
            {
                //TODO: Do something if the request to the API fails.
                var message = e.Message;
                Console.WriteLine(message);
            }
            return RedirectToAction("ManageLocationStock", new { locationId = ManagedLocation.Id });
        }


        // This view is to prompt for the information to be added. It's not a submission.
        [Route("location/stock/addnew")]
        [HttpGet]
        public ViewResult AddBrandNewProduct() { 
            if (ModelState.IsValid)
            {
                ViewBag.ProductTypes = GetProductTypesAsSelectOptions();
            }
            return View(); 
        }

        [HttpPost]
        [Route("location/stock/addnew")]
        public async Task<IActionResult> AddBrandNewProduct(Product product)
        {
            // TODO: Use Customer MVC Model instead of DB Model
            if (ModelState.IsValid)
            {
                try
                {
                    Product newProduct = new Product(product.Name, product.Price, product.TypeOfProduct, product.Description);
                    await this.PostDataAsync($"product/add", newProduct);

                    return RedirectToAction("ManageLocationStock", ManagedLocation.Id);
                }
                catch (HttpRequestException e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    ViewBag.ProductTypes = GetProductTypesAsSelectOptions();
                    return View(product);
                }
            }
            else return View(product);
        }

        private List<SelectListItem> GetProductTypesAsSelectOptions()
        {
            var productTypeOptions = new List<SelectListItem>();
            string[] productTypes = Enum.GetNames(typeof(ProductType));
            for (int i = 0; i < productTypes.Length; i++)
            {
                productTypeOptions.Add(new SelectListItem { Text = productTypes[i], Value = i.ToString(), Selected = false });
            }
            return productTypeOptions;
        }



        [Route("")]
        public async Task<IActionResult> Index()
        {
            if (CurrentManager == null)
            {
                return await LoginRedirectActionTask;
            }
            return await Task.Factory.StartNew(() => View(CurrentManager));
        }

        private List<InventoryLineItem> ReturnStockWithProductData(List<InventoryLineItem> stock)
        {
            var processedStock = ProcessInventory(stock);
            return processedStock;
        }

        private List<InventoryLineItem> ProcessInventory(List<InventoryLineItem> stockToProcess)
        {// TODO: This function is a duplicate from the CustomerController. Needs its own class.
            var getProcessedStock = new List<Task<InventoryLineItem>>();
            var processedStock = new List<InventoryLineItem>();

            foreach (InventoryLineItem ili in stockToProcess)
            {
                var getProcessedILI = GetProductForILIAsync(ili);
                getProcessedStock.Add(getProcessedILI);
            }
            //getProcessedStock.ForEach(t => t.Start());
            getProcessedStock.ForEach(t => processedStock.Add(t.Result));
            return processedStock;
        }

        
        private async Task<InventoryLineItem> GetProductForILIAsync(InventoryLineItem ili)
        {// TODO: This function is a duplicate from the CustomerController. Needs its own class.
            string productsRequest = $"product/get";
            var receivedProducts = await this.GetDataAsync<List<Product>>(productsRequest);

            ili.Product = receivedProducts.First(p => p.Id == ili.ProductId);
            return ili;
        }

    }
}
