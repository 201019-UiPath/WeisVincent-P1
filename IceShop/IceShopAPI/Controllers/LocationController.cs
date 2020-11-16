using AutoMapper;
using IceShopAPI.DTO;
using IceShopBL;
using IceShopDB.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IceShopAPI.Controllers
{
    /// <summary>
    /// API controller that handles location information, which includes inventory management and order history.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService, IMapper mapper)
        {
            _locationService = locationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Action that sends a list of locations back to the customer.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get")]
        [Produces("application/json")]
        public async Task<IActionResult> GetLocations()
        {
            try
            {
                var locations = _locationService.GetAllLocations();

                var getMappedLocations = new List<Task<LocationDTO>>();
                locations.ForEach(l => {
                    var getMappedLocation = Task<LocationDTO>.Factory.StartNew(() => {
                        return _mapper.Map<LocationDTO>(l);
                    });
                    getMappedLocations.Add(getMappedLocation);
                });

                var mappedLocations = await Task.WhenAll(getMappedLocations);
                return Ok(mappedLocations);
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Action that handles sending the stock of a location as a list of inventory items.
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [HttpGet("stock/get/{locationId}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetStockAtLocation(int locationId)
        {
            try
            {
                var getLocation = Task.Factory.StartNew( 
                    () => { return _locationService.GetAllLocations().Single(l => l.Id == locationId); }
                );
                var location = await getLocation;

                var getStock = _locationService.GetAllProductsAtLocationAsync(location);
                var stock = await getStock;

                var mapAllStock = new List<Task<ILIDTO>>();
                stock.ForEach(
                    ili => {
                        var mapStockItem = Task.Factory.StartNew(() =>
                       {
                           return _mapper.Map<ILIDTO>(ili);
                       });
                        mapAllStock.Add(mapStockItem);
                    });

                var stockDTOs = await Task.WhenAll(mapAllStock);
                return Ok(stockDTOs);
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Action that handles sending the orders associated with a location, selected by id.
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [HttpGet("orders/get/{locationId}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetOrdersAtLocation(int locationId)
        {
            try
            {
                var getLocation = Task.Factory.StartNew(
                    () => { return _locationService.GetLocationById(locationId); }
                );
                Location location = await getLocation;

                var getLocationOrderHistory = Task<List<Order>>.Factory.StartNew(() => { return _locationService.GetAllOrdersForLocation(location); });
                List<Order> locationOrderHistory = await getLocationOrderHistory;

                var getMappedOrderHistory = new List<Task<OrderDTO>>();

                locationOrderHistory.ForEach(o => {
                    var mapOrder = Task.Factory.StartNew(() => { return _mapper.Map<OrderDTO>(o); });
                    getMappedOrderHistory.Add(mapOrder);
                });

                var mappedOrderHistory = await Task.WhenAll(getMappedOrderHistory);
                return Ok(mappedOrderHistory);
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Action that handles adding a line item to stock.
        /// </summary>
        /// <param name="lineItemDTO"></param>
        /// <returns></returns>
        [HttpPost("stock/add")]
        [Consumes("application/json")]
        public async Task<IActionResult> AddLineItemToStock(ILIDTO lineItemDTO)
        {
            try
            {
                var lineItem = _mapper.Map<InventoryLineItem>(lineItemDTO);
                var addLineItem = Task.Factory.StartNew( () => _locationService.AddInventoryLineItemInRepo(lineItem) );
                await addLineItem;
                return CreatedAtAction("AddLineItemToStock", lineItem);
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Action that handles updating an existing line item, and removing it if the product quantity is zero.
        /// </summary>
        /// <param name="lineItemDTO"></param>
        /// <returns></returns>
        [HttpPut("stock/update")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateLineItemInStock(ILIDTO lineItemDTO)
        {
            try
            {
                var existingILIs = await _locationService.GetAllProductsAtLocationAsync(lineItemDTO.LocationId);
                var existingLineItem = existingILIs.Single(ili => ili.ProductId == lineItemDTO.ProductId);

                if (existingLineItem != null)
                {
                    existingLineItem.ProductQuantity = lineItemDTO.ProductQuantity;
                    _locationService.UpdateInventoryLineItemInRepo(existingLineItem);
                }
                
                return AcceptedAtAction("UpdateLineItemInStock", lineItemDTO);
            } catch (InvalidOperationException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Action that handles removing an existing line item.
        /// </summary>
        /// <param name="lineItemDTO"></param>
        /// <returns></returns>
        [HttpPut("stock/remove")]
        [Consumes("application/json")]
        public async Task<IActionResult> RemoveLineItemInStock(InventoryLineItem lineItemDTO)
        {
            try
            {
                var lineItem = _mapper.Map<InventoryLineItem>(lineItemDTO);

                var removeLineItem = Task.Factory.StartNew(() => _locationService.RemoveInventoryLineItemInRepo(lineItem));
                await removeLineItem;
                
                return AcceptedAtAction("RemoveLineItemInStock", lineItemDTO);
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }


    }
}
