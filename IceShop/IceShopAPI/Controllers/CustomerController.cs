using AutoMapper;
using IceShopAPI.DTO;
using IceShopBL;
using IceShopDB.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceShopAPI.Controllers
{
    /// <summary>
    /// API controller that handles customer information, which includes sending customer data and associated orders, and adding new customers to the database.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        /// <summary>
        /// Action that handles adding a new customer account to the database.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [Consumes("application/json")]
        public async Task<IActionResult> AddCustomer(CustomerDTO customer)
        {
            try
            {
                var addCustomerTask = Task.Factory.StartNew(() =>
                {
                    var newCustomer = _mapper.Map<Customer>(customer);
                    _customerService.AddCustomerToRepo(newCustomer);
                    return CreatedAtAction("AddCustomer", customer);
                });

                return await addCustomerTask;
                
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Action that handles sending a list of all customers and associated data. Insecure, should be removed.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var allCustomers = _customerService.GetAllCustomers();
                List<Task<CustomerDTO>> mapAllCustomers = new List<Task<CustomerDTO>>();

                allCustomers.ForEach(c => {
                    var mapCustomer = Task.Factory.StartNew(() => { 
                        return _mapper.Map<CustomerDTO>(c);
                    });
                    mapAllCustomers.Add(mapCustomer);
                });
                mapAllCustomers.ForEach(t => Console.WriteLine(t.Status));
                var allCustomersMapped = await Task.WhenAll(mapAllCustomers);

                return Ok( allCustomersMapped);
            } catch (Exception)
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Action that handles sending the correct customer by their respective email to verify their information.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("get/{email}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetCustomerByEmail(string email)
        {
            try
            {
                var getCustomerByEmail = Task.Factory.StartNew(() => { return _customerService.GetCustomerByEmail(email); });

                var returnedCustomer = await getCustomerByEmail;

                return Ok(returnedCustomer);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Action that handles sending all orders associated with a certain customer by their respective email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("get/orders/{email}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetOrders(string email)
        {
            try
            {
                var customer = _customerService.GetCustomerByEmail(email);
                var customerOrders = _customerService.GetAllOrdersForCustomer(customer);

                List<Task<OrderDTO>> mapAllOrders = new List<Task<OrderDTO>>();

                customerOrders.ForEach(o => {
                    var mapOrder = Task.Factory.StartNew(() => {
                        return _mapper.Map<OrderDTO>(o);
                    });
                    mapAllOrders.Add(mapOrder);
                });

                var allOrdersMapped = await Task.WhenAll(mapAllOrders);

                //return Content(orders[0].Customer.ToString());
                return Ok(allOrdersMapped);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPut("update")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateCustomer(Customer customer)
        {
            try
            {
                // TODO: Figure out if updating works statelessly.
                Task updateCustomer = Task.Factory.StartNew(() => _customerService.UpdateCustomerEntry(customer) );
                await updateCustomer;

                return AcceptedAtAction("UpdateProductEntry", customer);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


    }
}
