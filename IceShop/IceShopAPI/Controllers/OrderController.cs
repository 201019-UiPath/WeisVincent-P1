using AutoMapper;
using IceShopAPI.DTO;
using IceShopBL;
using IceShopDB.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace IceShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IOrderService _orderService;

        // TODO: What in the world do I do with OrderBuilder?

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet("products/get/{orderId}")]
        [Produces("application/json")]
        public IActionResult GetAllProductsInOrder(int orderId)
        {
            try
            {
                var order = _orderService.GetOrderById(orderId);
                var productDTOs = _mapper.Map<List<OLIDTO>>(_orderService.GetAllProductsInOrder(order));

                return Ok(productDTOs);
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }

        [HttpPost("add")]
        [Produces("application/json")]
        public IActionResult AddOrder(OrderDTO orderDTO)
        {
            try
            {
                var order = _mapper.Map<Order>(orderDTO);
                _orderService.AddOrderToRepo(order);

                var fetchedOrder = _orderService.GetOrderByDateTime(order.TimeOrderWasPlaced);

                var mappedOrder = _mapper.Map<OrderDTO>(fetchedOrder);
                return CreatedAtAction("AddOrder", fetchedOrder);
            } catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("lineitem/add")]
        [Produces("application/json")]
        public IActionResult AddOrderLineItem(OLIDTO oli)
        {
            try
            {
                var orderLineItem = _mapper.Map<OrderLineItem>(oli);

                _orderService.AddOrderLineItemToRepo(orderLineItem);
                    

                return CreatedAtAction("AddOrder", orderLineItem);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("get")]
        [Produces("application/json")]
        public IActionResult GetOrderByDateTime(double dateTimeDouble)
        {
            try
            {
                var order = _orderService.GetOrderByDateTime(dateTimeDouble);

                var mappedOrder = _mapper.Map<OrderDTO>(order);

                return Ok( mappedOrder);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
