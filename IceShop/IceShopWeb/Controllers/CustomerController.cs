using IceShopWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IceShopWeb.Controllers
{
    [Route("user")]
    public class CustomerController : Controller
    {
        const string url = "https://localhost:5001/api/";

        private Customer CurrentCustomer {
            get { return HttpContext.Session.Get<Customer>("CurrentCustomer");}
            set { HttpContext.Session.Set("CurrentCustomer", value);}
        }

        private List<InventoryLineItem> CurrentCart
        {
            get { return HttpContext.Session.Get<List<InventoryLineItem>>("CurrentCart");}
            set { HttpContext.Session.Set<List<InventoryLineItem>>("CurrentCart", value);}
        }

        private Location CurrentLocation { get { return HttpContext.Session.Get<Location>("CurrentLocation"); } set { CurrentCart = null; HttpContext.Session.Set<Location>("CurrentLocation", value); } }

        private readonly IActionResult LoginRedirectAction;
        private readonly Task<IActionResult> LoginRedirectActionTask;
        public CustomerController()
        {
            LoginRedirectAction = RedirectToAction("Login", "Home", "-1");
            LoginRedirectActionTask = Task.Factory.StartNew(() => LoginRedirectAction);
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            // TODO: Check if the customer is logged in before returning this
            if (CurrentCustomer == null)
            {
                return await LoginRedirectActionTask;
            }
            return await Task.Factory.StartNew(() => View(CurrentCustomer));
        }

        [Route("orders")]
        public async Task<IActionResult> GetOrderHistory(int? sortBy)
        {
            if (CurrentCustomer == null)
            {
                return await LoginRedirectActionTask;
            }

            string request = $"customer/get/orders/{CurrentCustomer.Email}";
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

            if (receivedOrders != default) return View(sortedOrders); else return StatusCode(500);
            //return resultView;

            return StatusCode(500);
        }

        [Route("orders/details")]
        public async Task<IActionResult> ViewOrderDetails(int orderId)
        {
            if (CurrentCustomer == null) return await LoginRedirectActionTask;
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


        [Route("sample/AllCustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            //if (CurrentCustomer == null) return LoginRedirectAction;

            //List<Customer> customers = new List<Customer>();

            string request = $"customer/get";

            var customers = await this.GetDataAsync<List<Customer>>(request);

            return View(customers);
            #region Old way of getting all customers
            /*using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                var response = client.GetAsync("customer/get");
                response.Wait();

                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsAsync<Customer[]>();


                    var arrayOfCustomers = readTask;//.Result;
                    foreach (var customer in arrayOfCustomers)
                    {
                        customers.Add(customer);
                    }
                    customers.Add(CurrentCustomer);
                }
            }
            //var fetchedCustomers = await _repo.GetAllCustomersAsync();
             return View(customers);
             */
            #endregion

        }

        [Route("get/{email}")]
        public async Task<IActionResult> GetCustomerByEmail(string email)
        {
            if (CurrentCustomer == null) return LoginRedirectAction;
            Customer customer;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                var response = client.GetAsync($"customer/get/{email}");
                response.Wait();

                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Customer>();
                    readTask.Wait();

                    var resultCustomer = readTask.Result;
                    customer = resultCustomer;
                    return View(customer);
                }
            }


            // TODO: What to do here...
            return View();

        }

        // This view is to prompt for the information to be added. It's not a submission.
        // [HttpGet]
        [Route("add")]
        public ViewResult AddCustomer()
        {

            return View();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddCustomer(Customer customer)
        {
            // TODO: Use Customer MVC Model instead of DB Model
            if (ModelState.IsValid)
            {
                Customer newCustomer = new Customer(customer.Name, customer.Email, customer.Password, customer.Address);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var response = client.PostAsJsonAsync($"customer/add", newCustomer);
                    response.Wait();

                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Customer>();
                        readTask.Wait();

                        var resultCustomer = readTask.Result;
                        customer = resultCustomer;
                        return View(customer);
                    }
                }

                return Redirect("GetAllCustomers");
            }
            else return View();
        }

        [Route("order/")]
        public async Task<IActionResult> SelectLocation()
        {
            if (CurrentCustomer == null) return await LoginRedirectActionTask;
            string request = $"location/get";

            var locations = await this.GetDataAsync<List<Location>>(request);

            return View(locations);
        }

        [Route("order/viewStock/{locationId}")]
        public async Task<IActionResult> ViewInventoryAtLocation(int locationId)
        {
            if (CurrentLocation == null) {
                string requestLocations = $"location/get";
                var locations = await this.GetDataAsync<List<Location>>(requestLocations);
                CurrentLocation = locations.Where(l=>l.Id == locationId).First();
            };

            if (CurrentCustomer == null) return await LoginRedirectActionTask;

            if (CurrentCart == null) CurrentCart = new List<InventoryLineItem>();

            string request = $"location/stock/get/{locationId}";
            var stock = await this.GetDataAsync<List<InventoryLineItem>>(request);

            var processedStock = ReturnStockWithoutCartItems(stock, CurrentCart);

            ViewData["LocationName"] = CurrentLocation.Name;
            ViewData["CartData"] = CurrentCart;

            return View(processedStock);
        }

        [Route("order/cart/view")]
        public async Task<IActionResult> ViewCart()
        {
            if (CurrentCustomer == null) return await LoginRedirectActionTask;
            if (CurrentCart != null) return View(CurrentCart);

            ModelState.AddModelError(string.Empty, "Your cart is empty.");

            return View(CurrentCart);
            
        }

        [Route("order/cart/add")]
        public async Task<IActionResult> AddItemToCart(InventoryLineItem ili)
        {
            if (CurrentCustomer == null) return await LoginRedirectActionTask;
            if (CurrentCart == null) CurrentCart = new List<InventoryLineItem>();

            InventoryLineItem processedILI = await GetProductForILIAsync(ili);

            List<InventoryLineItem> tempCart = CurrentCart;

            try
            {
                tempCart.Where(sli => sli.ProductId == processedILI.ProductId).First().ProductQuantity += 1;
            }
            catch (InvalidOperationException e)
            {
                var message = e.Message;
                StagedLineItem newSLI = new StagedLineItem(processedILI.Product, 1, processedILI);
                InventoryLineItem newILI = new InventoryLineItem(CurrentLocation, processedILI.Product, 1);
                tempCart.Add(newILI);
            }
            CurrentCart = tempCart;

            return RedirectToAction("ViewInventoryAtLocation", new { locationId = CurrentLocation.Id });

        }

        [Route("order/cart/remove")]
        public async Task<IActionResult> RemoveItemFromCart(InventoryLineItem ili)
        {
            if (CurrentCustomer == null) return await LoginRedirectActionTask;
            if (CurrentCart == null) CurrentCart = new List<InventoryLineItem>();

            InventoryLineItem processedILI = await GetProductForILIAsync(ili);
            List<InventoryLineItem> tempCart = CurrentCart;

            try
            {
                tempCart.Where(sli => sli.ProductId == processedILI.ProductId).First().ProductQuantity -= 1;
                tempCart.RemoveAll(sli => sli.ProductQuantity < 1);
            }
            catch (InvalidOperationException e) {// TODO: What happens if removing an item doesn't work here?
            }
            CurrentCart = tempCart;
            return RedirectToAction("ViewInventoryAtLocation", new { locationId = CurrentLocation.Id });
        }

        [Route("order/submit")]
        public async Task<IActionResult> SubmitOrder()
        {
            if (CurrentCustomer == null) return await LoginRedirectActionTask;
            if (CurrentCart == null) return await LoginRedirectActionTask;//TODO: Redirect properly.

            // Arrange data needed to submit the order.
            List<InventoryLineItem> tempCart = CurrentCart;
            List<OrderLineItem> orderItems = new List<OrderLineItem>();

            // Get the time of the order.
            double timeNowAsDouble = DateTimeUtility.GetUnixEpochAsDouble(DateTime.Now);

            // Get the total of the order.
            double total = 0.0;
            tempCart.ForEach(cartItem => total += cartItem.Product.Price * cartItem.ProductQuantity);

            // Generate a new order, without an ID.
            Order newOrder = new Order(CurrentCustomer.Id, CurrentLocation.Id, CurrentCustomer.Address, total, timeNowAsDouble);

            // Submit the order without the ID.
            var postOrderRequest = $"order/add";
            await this.PostDataAsync(postOrderRequest, newOrder);

            // Get the order back so we can have the ID.
            var getOrderRequest = $"order/get?dateTimeDouble={timeNowAsDouble}";
            var retrievedRedundantOrder = await this.GetDataAsync<Order>(getOrderRequest);

            // Make the order items from the cart items, using the fetched order Id.
            foreach(InventoryLineItem cartItem in tempCart)
            {
                var newOrderItem = new OrderLineItem(retrievedRedundantOrder.Id, cartItem.ProductId, cartItem.ProductQuantity);
                orderItems.Add(newOrderItem);
            }



            // Submit the order line items.
            foreach (OrderLineItem oli in orderItems)
            {
                var postOrderLineItemRequest = $"order/lineitem/add";
                await this.PostDataAsync(postOrderLineItemRequest, oli);
            }

            string request = $"location/stock/get/{CurrentLocation.Id}";
            var stock = await this.GetDataAsync<List<InventoryLineItem>>(request);

            var processedStock = ReturnStockWithoutCartItems(stock, tempCart);

            foreach (InventoryLineItem ili in processedStock)
            {
                string putStockRequest= $"location/stock/update";
                await this.PutDataAsync(putStockRequest, ili);
            }

            // Clear all relevant data.
            tempCart.Clear();
            CurrentCart = null;

            // TODO: Show successful order submission.
            return RedirectToAction("Index");
            
        }

        private List<InventoryLineItem> ReturnStockWithoutCartItems(List<InventoryLineItem> stock, List<InventoryLineItem> cart)
        {
            var processedStock = ProcessInventory(stock);

            var tempCart = cart;

            foreach (InventoryLineItem ili in processedStock)
            {
                Predicate<InventoryLineItem> predicate = new Predicate<InventoryLineItem>(
                    cartItem => cartItem.LocationId == ili.LocationId && cartItem.ProductId == ili.ProductId);

                Func<InventoryLineItem, bool> standard = new Func<InventoryLineItem, bool>(predicate);

                if (tempCart.Exists(predicate))
                {
                    var matchingCartItem = tempCart.First(standard);
                    ili.ProductQuantity -= matchingCartItem.ProductQuantity;
                };
            }

            processedStock.RemoveAll(ili => ili.ProductQuantity < 1);
            return processedStock;
        }

        private List<InventoryLineItem> ProcessInventory(List<InventoryLineItem> stockToProcess)
        {
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
        {
            string productsRequest = $"product/get";
            var receivedProducts = await this.GetDataAsync<List<Product>>(productsRequest);

            ili.Product = receivedProducts.First(p => p.Id == ili.ProductId);
            return ili;
        }

    }
}
