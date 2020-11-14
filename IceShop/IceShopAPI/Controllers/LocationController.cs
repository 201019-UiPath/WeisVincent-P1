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
        public IActionResult GetOrdersAtLocation(LocationDTO locationDTO)
        {
            try
            {
                var location = _mapper.Map<Location>(locationDTO);
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
        public IActionResult AddLineItemToStock(ILIDTO lineItemDTO)
        {
            try
            {
                var lineItem = _mapper.Map<InventoryLineItem>(lineItemDTO);
                _locationService.AddInventoryLineItemInRepo(lineItem);
                return CreatedAtAction("AddLineItemToStock", lineItem);
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }

        [HttpPut("stock/update")]
        [Consumes("application/json")]
        public IActionResult UpdateLineItemInStock(ILIDTO lineItemDTO)
        {
            try
            {
                var lineItem = _mapper.Map<InventoryLineItem>(lineItemDTO);

                _locationService.UpdateInventoryLineItemInRepo(lineItem);
                return AcceptedAtAction("UpdateLineItemInStock", lineItemDTO);
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }

        [HttpPut("stock/remove")]
        [Consumes("application/json")]
        public IActionResult RemoveLineItemInStock(InventoryLineItem lineItemDTO)
        {
            try
            {
                var lineItem = _mapper.Map<InventoryLineItem>(lineItemDTO);
                _locationService.RemoveInventoryLineItemInRepo(lineItem);
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
