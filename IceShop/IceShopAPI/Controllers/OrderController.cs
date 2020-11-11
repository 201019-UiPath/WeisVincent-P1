using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IceShopBL;
using IceShopDB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IceShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        // TODO: What in the world do I do with OrderBuilder?

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("products/get/{order}")]
        [Produces("application/json")]
        public IActionResult GetAllProductsInOrder(Order order)
        {
            try
            {
                return Ok(_orderService.GetAllProductsInOrder(order));
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }

        [HttpPost("add/{order}")]
        [Produces("application/json")]
        public IActionResult AddOrder(Order order)
        {
            try
            {
                _orderService.AddOrderToRepo(order);
                return CreatedAtAction("AddOrder", order);
            } catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
