using AutoMapper;
using IceShopAPI.DTO;
using IceShopBL;
using IceShopDB.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IceShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
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

        [HttpPost("add")]
        [Consumes("application/json")]
        public IActionResult AddNewProduct(ProductDTO product)
        {
            try
            {
                var newProduct = _mapper.Map<Product>(product);

                _productService.AddNewProduct(newProduct);
                return CreatedAtAction("AddNewProduct", product);
            } catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("update")]
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
