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
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get")]
        //[Produces("application/json")]
        public IActionResult GetAllProducts()
        {
            try
            {
                return Ok(_productService.GetAllProducts());
            } catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("add/{product}")]
        [Consumes("application/json")]
        public IActionResult AddNewProduct(Product product)
        {
            try
            {
                _productService.AddNewProduct(product);
                return CreatedAtAction("AddNewProduct", product);
            } catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("update/{product}")]
        [Consumes("application/json")]
        public IActionResult UpdateProductEntry(Product product)
        {
            try
            {
                // TODO: Figure out if updating works statelessly.
                _productService.UpdateProductEntry(product);
                return AcceptedAtAction("UpdateProductEntry", product);
            } catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
