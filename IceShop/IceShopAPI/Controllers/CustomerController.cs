using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IceShopBL;
using IceShopDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace IceShopAPI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("get")]
        [Produces("application/json")]
        public IActionResult GetAllCustomers()
        {
            try
            {
                return Ok(_customerService.GetAllCustomers());
            } catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("get/{email}")]
        [Produces("application/json")]
        public IActionResult GetCustomerByEmail(string email)
        {
            try
            {
                return Ok(_customerService.GetCustomerByEmail(email));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("add")]
        [Consumes("application/json")]
        public IActionResult AddCustomer(Customer newCustomer)
        {
            try
            {
                _customerService.AddCustomerToRepo(newCustomer);
                return CreatedAtAction("AddHero", newCustomer);
            } catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }


    }
}
