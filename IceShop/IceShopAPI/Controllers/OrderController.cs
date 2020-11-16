using AutoMapper;
using IceShopAPI.DTO;
using IceShopBL;
using IceShopDB.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceShopAPI.Controllers
{
    /// <summary>
    /// API controller that handles order information, which includes getting a list of products in an order, 
    /// getting an order by its date time as a double, 
    /// and adding a new order and associated line items.
    /// </summary>
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
        
        /// <summary>
        /// Action that gets all the products in a specific order, selected by id.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("products/get/{orderId}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllProductsInOrder(int orderId)
        {
            try
            {
                var getOrder = Task.Factory.StartNew(() => {return _orderService.GetOrderById(orderId); });
                var order = await getOrder;

                var getOrderedProducts = Task.Factory.StartNew(() => { return _orderService.GetAllProductsInOrder(order); });
                var orderedProducts = await getOrderedProducts;

                var productDTOs = _mapper.Map<List<OLIDTO>>(orderedProducts);

                return Ok(productDTOs);
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Action that handles adding an order, and returns the order added, complete with the generated ID.
        /// </summary>
        /// <param name="orderDTO"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [Produces("application/json")]
        public async Task<IActionResult> AddOrder(OrderDTO orderDTO)
        {
            try
            {
                var order = _mapper.Map<Order>(orderDTO);

                var addOrder = Task.Factory.StartNew( () => _orderService.AddOrderToRepo(order) );
                await addOrder;

                var fetchAddedOrder = Task.Factory.StartNew(() => { return _orderService.GetOrderByDateTime(order.TimeOrderWasPlaced); });
                var fetchedOrder = await fetchAddedOrder;

                var mappedOrder = _mapper.Map<OrderDTO>(fetchedOrder);
                return CreatedAtAction("AddOrder", fetchedOrder);
            } catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Action that handles adding an order line item, to an associated order.
        /// </summary>
        /// <param name="oli"></param>
        /// <returns></returns>
        [HttpPost("lineitem/add")]
        [Produces("application/json")]
        public async Task<IActionResult> AddOrderLineItem(OLIDTO oli)
        {
            try
            {
                var orderLineItem = _mapper.Map<OrderLineItem>(oli);
                var addOrderLineItem = Task.Factory.StartNew(() => _orderService.AddOrderLineItemToRepo(orderLineItem) );

                await addOrderLineItem;

                return CreatedAtAction("AddOrderLineItem", orderLineItem);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Action that handles adding multiple order line items in bulk.
        /// </summary>
        /// <param name="olis"></param>
        /// <returns></returns>
        [HttpPost("lineitem/addmany")]
        [Produces("application/json")]
        public async Task<IActionResult> AddOrderLineItems(List<OLIDTO> olis)
        {
            try
            {
                var mapOrderLineItems = new List<Task<OrderLineItem>>();

                foreach(OLIDTO oli in olis)
                {
                    var mapOrderLineItem = Task.Factory.StartNew(() => { 
                        var orderLineItem = _mapper.Map<OrderLineItem>(oli);
                        return orderLineItem; 
                    });

                    mapOrderLineItems.Add(mapOrderLineItem);
                }

                var orderLineItems = await Task.WhenAll(mapOrderLineItems);

                var addOrderLineItems = new List<Task>();
                foreach(OrderLineItem oli in orderLineItems)
                {
                    var addOLITask = Task.Factory.StartNew(()=> _orderService.AddOrderLineItemToRepo(oli));
                    addOrderLineItems.Add(addOLITask);
                }

                await Task.WhenAll(addOrderLineItems);

                return CreatedAtAction("AddOrderLineItems", olis);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Action that handles getting order data by finding an order by the time it was submitted. Not ideal.
        /// </summary>
        /// <param name="dateTimeDouble"></param>
        /// <returns></returns>
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
