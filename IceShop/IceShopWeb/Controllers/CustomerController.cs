using IceShopWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IceShopWeb.Controllers
{

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

        [Route("u/")]
        public async Task<IActionResult> Index()
        {
            // TODO: Check if the customer is logged in before returning this
            if(CurrentCustomer == null)
            {
                return await LoginRedirectActionTask;
            }
            return await Task.Factory.StartNew(() => View(CurrentCustomer));
        }

        [Route("u/orders/{sortBy}")]
        public async Task<IActionResult> GetOrderHistory(int? sortBy)
        {
            if (CurrentCustomer == null)
            {
                return await LoginRedirectActionTask; 
            }


            string request = $"customer/get/orders/{CurrentCustomer.Id}";

            var receivedOrders = await this.GetDataAsync<List<Order>>(request);

            var sortedOrders = sortBy switch
            {
                0 => receivedOrders.OrderBy(o => o.Subtotal),
                1 => receivedOrders.OrderBy(o => o.Subtotal).Reverse(),
                2 => receivedOrders.OrderBy(o => o.TimeOrderWasPlaced),
                3 => receivedOrders.OrderBy(o => o.TimeOrderWasPlaced).Reverse(),
                _ => receivedOrders
            };

            var resultView = receivedOrders != default ? View(sortedOrders) : null;

            if (resultView != null) return resultView; else return StatusCode(500);
            return resultView;
            #region Old way of doing things
            /*var response = client.GetAsync(request);

            response.Wait();

            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<Order>>();
                readTask.Wait();

                var resultingOrders = readTask.Result;

                return View(resultingOrders);
            }*/
            #endregion

            return StatusCode(500);
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
        public ViewResult AddCustomer()
        {

            return View();
        }

        [HttpPost]
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
