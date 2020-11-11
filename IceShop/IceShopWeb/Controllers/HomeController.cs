using IceShopWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using mvc = IceShopWeb.Models;

namespace IceShopWeb.Controllers
{
    public class HomeController : Controller
    {
        const string url = "https://localhost:5001/api/";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        

        public ViewResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(AuthPack userInput)
        {
            // Passing viewdata to the Index.cshtml file
            Customer sampleCustomer = new Customer("Sample Name", "sample@email.emailcom", "password", "The Ultimate Customer") { Id = -127 };
            ViewData["CustomerUsingViewData"] = sampleCustomer;

            //ViewBag.id = id;
            //ViewBag.name = name;

            ViewBag.Homie = sampleCustomer;

            if (ModelState.IsValid)
            {
                string inputEmail = userInput.Email;
                string inputPassword = userInput.Password;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var response = client.GetAsync($"customer/get/{inputEmail}");
                    response.Wait();

                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Customer>();
                        readTask.Wait();

                        var resultCustomer = readTask.Result;

                        if (resultCustomer.Password == inputPassword && resultCustomer.Email == inputEmail)
                        {
                            HttpContext.Session.Set<Customer>("CurrentCustomer", resultCustomer);
                            return RedirectToAction("GetAllCustomers", "Customer");
                        } else
                        {
                            return View(userInput);
                        }

                    }

                    //return RedirectToAction("Index", "Customer");

                }

            }


            return View(userInput);
        }

        // This following line of code is attribute-based routing.
        // Make sure it doesn't conflict with the conventional routing in the Startup.cs
        [Route(("Controller=Home/action=Privacy/"))]
        public IActionResult Privacy()
        {
            return View();
        }

        // You don't NEED to specify the action if it's in the right controller already.
        [HttpPost]
        [Route(("Controller=Home"))]
        public IActionResult About()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
