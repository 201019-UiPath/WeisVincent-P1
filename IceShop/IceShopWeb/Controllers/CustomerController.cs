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
            get { 
                return HttpContext.Session.Get<Customer>("CurrentCustomer"); 
            } 
            set { 
                HttpContext.Session.Set("CurrentCustomer", value); 
            } 
        }

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
            if(CurrentCustomer == null)
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
        public async Task<IActionResult> ViewOrderDetails(Order order)
        {
            if (CurrentCustomer == null)
            {
                return await LoginRedirectActionTask;
            }

            string request = $"order/products/get/{order}";
            var receivedOrderLineItems = await this.GetDataAsync<List<OrderLineItem>>(request);
            string productsRequest = $"product/get";
            var receivedProducts = await this.GetDataAsync<List<Product>>(productsRequest);


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
                Customer newCustomer = new Customer(customer.Name,customer.Email, customer.Password,customer.Address);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var response = client.GetAsync($"customer/add/{newCustomer}");
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

    }
}
