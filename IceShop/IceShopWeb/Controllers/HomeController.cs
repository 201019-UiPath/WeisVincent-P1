using IceShopWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using mvc = IceShopWeb.Models;

namespace IceShopWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index(int id = -1, string name = "defaultname")
        {

            ViewBag.id = id;
            ViewBag.name = name;


            // Passing viewdata to the Index.cshtml file
            mvc.Customer customer = new mvc.Customer() { Id = 1, Address = "The Ultimate Customer", Name = "Mr. Customer" };
            ViewData["CustomerUsingViewData"] = customer;

            ViewBag.Homie = customer;


            return View(customer);
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
