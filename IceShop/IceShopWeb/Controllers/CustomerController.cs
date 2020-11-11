using System;
using IceShopWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IceShopWeb.Controllers
{

    public class CustomerController : Controller
    {
        const string url = "https://localhost:5001/api/";


        public CustomerController()
        {
            
        }

        // Use https://localhost:port/Customer/Index to use this function
        public async Task<IActionResult> Index()
        {
            return await Task.Factory.StartNew(() => View());
        }



        public async Task<IActionResult> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            
            using (var client = new HttpClient())
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
                }
            }

            //var fetchedCustomers = await _repo.GetAllCustomersAsync();
            return View(customers);
        }

        public async Task<IActionResult> GetCustomerByEmail(string email)
        {

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

            //var fetchedCustomers = await _repo.GetAllCustomersAsync();

            // TODO: What to do here...
            return View();



            //var customer = await _repo.GetCustomerByEmailAsync(email);
            //return View(customer);
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
