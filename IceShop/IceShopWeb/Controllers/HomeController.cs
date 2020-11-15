using IceShopWeb.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

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
        
        [HttpGet]
        public ViewResult Login(int? sessionExists)
        {
            if (sessionExists == 0)
            {
                ViewData["Redirect"] = "Your session does not exist. Please sign in.";
            }
            return View();
        }
        
        public async Task<IActionResult> Login(AuthPack userInput)
        {
            if (ModelState.IsValid)
            {
                string inputEmail = userInput.Email;
                string inputPassword = userInput.Password;

                try
                {
                    string request = $"customer/get/{inputEmail}";
                    var resultCustomer = await this.GetDataAsync<Customer>(request);
                    if (resultCustomer.Password == inputPassword && resultCustomer.Email == inputEmail)
                    {
                        HttpContext.Session.Set<Customer>("CurrentCustomer", resultCustomer);
                        return RedirectToAction("Index", "Customer");
                    }
                    else {
                        // TODO: Show the user the login failed.

                        return View(userInput);
                    };
                } catch (HttpRequestException)
                {
                    // TODO: Add information for the user if the request failed.
                    return View(userInput);
                } catch (NullReferenceException)
                {
                    // TODO: Add information for the user if there was no customer returned.
                    return View(userInput);
                }
            }

            return View(userInput);
        }

        // This following line of code is attribute-based routing.
        // Make sure it doesn't conflict with the conventional routing in the Startup.cs
        [Route(("Privacy/"))]
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
