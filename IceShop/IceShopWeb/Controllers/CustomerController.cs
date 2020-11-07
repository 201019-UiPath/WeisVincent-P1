using IceShopDB.Repos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using db = IceShopDB.Models;
using mvc = IceShopWeb.Models;

namespace IceShopWeb.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IRepository _repo;


        public CustomerController(IRepository repo)
        {
            _repo = repo;
        }

        // Use https://localhost:port/Customer/Index to use this function
        public async Task<IActionResult> Index()
        {
            return await GetAllCustomers();
        }

        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _repo.GetAllCustomersAsync();
            return View(customers);
        }

        public async Task<IActionResult> GetCustomerByEmail(string email)
        {
            var customer = await _repo.GetCustomerByEmailAsync(email);
            return View(customer);
        }

        // This view is to prompt for the information to be added. It's not a submission.
        // [HttpGet]
        public ViewResult AddCustomer()
        {

            return View();
        }

        [HttpPost]
        public IActionResult AddCustomer(db.Customer customer)
        {
            // TODO: Use Customer MVC Model instead of DB Model
            if (ModelState.IsValid)
            {
                mvc.Customer newCustomer = new mvc.Customer();
                newCustomer.Name = customer.Name;
                newCustomer.Email = customer.Email;
                newCustomer.Password = customer.Password;
                newCustomer.Address = customer.Address;


                _repo.AddCustomer(customer);
                return Redirect("GetAllCustomers");
            }
            else return View();


        }

    }
}
