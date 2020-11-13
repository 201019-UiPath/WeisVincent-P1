using AutoMapper;
using IceShopAPI.DTO;
using IceShopBL;
using IceShopDB.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IceShopAPI.Controllers
{
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

        [HttpGet("get")]
        [Produces("application/json")]
        public IActionResult GetLocations()
        {
            try
            {
                return Ok(_locationService.GetAllLocations());
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }

        [HttpGet("stock/get/{locationId}")]
        [Produces("application/json")]
        public IActionResult GetStockAtLocation(int locationId)
        {
            //try
            {
                var location = _locationService.GetAllLocations().Single(l => l.Id == locationId);

                var stock = _locationService.GetAllProductsStockedAtLocation(location);

                var stockDTOs = new List<ILIDTO>();

                foreach(InventoryLineItem ili in stock)
                {
                    stockDTOs.Add(_mapper.Map<ILIDTO>(ili));
                    Console.WriteLine($"Added {ili}");
                }

                return Ok(stockDTOs);
            }
            //catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }

        [HttpGet("orders/get/{location}")]
        [Produces("application/json")]
        public IActionResult GetOrdersAtLocation(Location location)
        {
            try
            {
                return Ok(_locationService.GetAllOrdersForLocation(location));
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }

        [HttpPost("stock/add/{inventorylineitem}")]
        [Consumes("application/json")]
        public IActionResult AddLineItemToStock(InventoryLineItem lineItem)
        {
            try
            {
                _locationService.AddInventoryLineItemInRepo(lineItem);
                return CreatedAtAction("AddLineItemToStock", lineItem);
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }

        [HttpPut("stock/update/{inventorylineitem}")]
        [Consumes("application/json")]
        public IActionResult UpdateLineItemInStock(InventoryLineItem lineItem)
        {
            try
            {
                _locationService.UpdateInventoryLineItemInRepo(lineItem);
                return AcceptedAtAction("UpdateLineItemInStock", lineItem);
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }

        [HttpPut("stock/remove/{inventorylineitem}")]
        [Consumes("application/json")]
        public IActionResult RemoveLineItemInStock(InventoryLineItem lineItem)
        {
            try
            {
                _locationService.RemoveInventoryLineItemInRepo(lineItem);
                return CreatedAtAction("RemoveLineItemInStock", lineItem);
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }


    }
}
