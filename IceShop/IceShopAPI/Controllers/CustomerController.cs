using IceShopBL;
using IceShopDB.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IceShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("add")]
        [Consumes("application/json")]
        public IActionResult AddCustomer(Customer newCustomer)
        {
            try
            {
                _customerService.AddCustomerToRepo(newCustomer);
                return CreatedAtAction("AddCustomer", newCustomer);
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
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


        [HttpGet("get/orders/{email}")]
        [Produces("application/json")]
        public IActionResult GetOrders(string email)
        {
            try
            {
                var customer = _customerService.GetCustomerByEmail(email);
                return Ok(_customerService.GetAllOrdersForCustomer(customer));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPut("update/{customer}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public IActionResult UpdateCustomer(Customer customer)
        {
            try
            {
                // TODO: Figure out if updating works statelessly.
                _customerService.UpdateCustomerEntry(customer);
                return AcceptedAtAction("UpdateProductEntry", customer);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


    }
}
