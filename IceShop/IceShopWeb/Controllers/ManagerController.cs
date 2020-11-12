using Microsoft.AspNetCore.Mvc;

namespace IceShopWeb.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
