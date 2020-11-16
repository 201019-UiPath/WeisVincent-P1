using AutoMapper;
using IceShopAPI.DTO;
using IceShopBL;
using IceShopDB.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace IceShopAPI.Controllers
{
    /// <summary>
    /// API controller that handles product information, which includes getting a list of all products, 
    /// updating an existing product,
    /// and adding a new product.
    /// </summary>
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

        /// <summary>
        /// Action that handles sending a list of all products in the catalogue.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var getProducts = Task.Factory.StartNew(() => { return _productService.GetAllProducts(); });

                var products = await getProducts;
                return Ok(products);
            } catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Action that handles adding a new product to the catalogue.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [Consumes("application/json")]
        public async Task<IActionResult> AddNewProduct(ProductDTO product)
        {
            try
            {
                var mapProduct = Task.Factory.StartNew(() =>
               {
                   var newProduct = _mapper.Map<Product>(product);
                   return newProduct;
               });

                var newProductToAdd = await mapProduct;
                
                _productService.AddNewProduct(newProductToAdd);

                return CreatedAtAction("AddNewProduct", product);
            } catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Action that handles updating a product entry in the catalogue.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateProductEntry(ProductDTO product)
        {
            try
            {
                var updatedProduct = _mapper.Map<Product>(product);
                // TODO: Figure out if updating works statelessly.
                _productService.UpdateProductEntry(updatedProduct);

                return AcceptedAtAction("UpdateProductEntry", product);
            } catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
