﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using api.Models.DTOs;
using api.Services;
using api.Services.Interfaces;
using MailKit.Search;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                    return BadRequest("Page and PageSize must be greater than zero.");
                _logger.LogInformation($"Fetching products from page {page} with page size {pageSize}.");

                _logger.LogInformation("Fetching all products from the database.");
                var products = await _productService.GetAllProductsAsync(page, pageSize);
                if (products == null || !products.Any())
                {
                    _logger.LogWarning("No products found in the database.");
                    return NotFound("No products found.");
                }
                _logger.LogInformation($"Found {products.Count()} products.");
                var productDtos = products.Adapt<IEnumerable<ProductDTO>>();
                return Ok(productDtos);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching products.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching products.");
            }
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching product with ID {id} from the database.");
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found.");
                    return NotFound($"Product with ID {id} not found.");
                }
                _logger.LogInformation($"Product with ID {id} found.");
                var productDto = product.Adapt<ProductDTO>();
                return Ok(productDto);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching the product.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the product.");
            }
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductDTO productDto)
        {
            if (id != productDto.Id)
                return BadRequest("Product ID mismatch.");

            try
            {
                _logger.LogInformation($"Updating product with ID {id}.");
                var product = productDto.Adapt<Product>();
                await _productService.UpdateProductAsync(id, product);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _productService.GetProductByIdAsync(id) == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found for update.");
                    return NotFound($"Product with ID {id} not found.");
                }
                else
                {
                    _logger.LogError($"Concurrency error occurred while updating product with ID {id}.");
                    throw;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while updating product with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the product.");
            }
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> PostProduct(CreateProductDTO createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (createDto == null)
                {
                    _logger.LogWarning("Received null product in POST request.");
                    return BadRequest("Product cannot be null.");
                }
                _logger.LogInformation("Creating a new product.");
                var product = createDto.Adapt<Product>();
                await _productService.AddProductAsync(product);
                var productDto = product.Adapt<ProductDTO>();
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productDto);

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while creating the product.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the product.");
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting product with ID {id}.");
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found for deletion.");
                    return NotFound($"Product with ID {id} not found.");
                }
                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while deleting product with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the product.");
            }
        }

        // GET: api/Products/category/{category}
        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategory(string category, [FromQuery] int? page = null, [FromQuery] int? pageSize = null)
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                    return BadRequest("Page and PageSize must be greater than zero.");
                _logger.LogInformation($"Fetching products from page {page} with page size {pageSize}.");

                _logger.LogInformation($"Fetching products in category '{category}'.");
                var products = await _productService.GetProductsByCategoryAsync(category, page, pageSize);
                if (products == null || !products.Any())
                {
                    _logger.LogWarning($"No products found in category '{category}'.");
                    return NotFound($"No products found in category '{category}'.");
                }
                var productDtos = products.Adapt<IEnumerable<ProductDTO>>();
                return Ok(productDtos);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while fetching products in category '{category}'.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching products.");
            }
        }
        // GET: api/Products/search/{searchTerm}
        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> SearchProducts(string searchTerm, [FromQuery] int? page = null, [FromQuery] int? pageSize = null)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest("Search term cannot be empty.");
            try
            {
                if (page <= 0 || pageSize <= 0)
                    return BadRequest("Page and PageSize must be greater than zero.");
                _logger.LogInformation($"Fetching products from page {page} with page size {pageSize}.");

                _logger.LogInformation($"Searching for products with term '{searchTerm}'.");
                var products = await _productService.SearchProductsAsync(searchTerm, page, pageSize);
                if (products == null || !products.Any())
                {
                    _logger.LogWarning($"No products found matching search term '{searchTerm}'.");
                    return NotFound($"No products found matching search term '{searchTerm}'.");
                }
                var productDtos = products.Adapt<IEnumerable<ProductDTO>>();
                return Ok(productDtos);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while searching for products with term '{searchTerm}'.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while searching for products.");
            }
        }

        // GET: api/Products/artist/{artistId}
        [HttpGet("artist/{artistId}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByArtist(int artistId, [FromQuery] int? page = null, [FromQuery] int? pageSize = null)
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                    return BadRequest("Page and PageSize must be greater than zero.");
                _logger.LogInformation($"Fetching products from page {page} with page size {pageSize}.");

                _logger.LogInformation($"Fetching products by artist ID {artistId}.");
                var products = await _productService.GetProductsByArtistAsync(artistId, page, pageSize);
                if (products == null || !products.Any())
                {
                    _logger.LogWarning($"No products found for artist ID {artistId}.");
                    return NotFound($"No products found for artist ID {artistId}.");
                }
                var productDtos = products.Adapt<IEnumerable<ProductDTO>>();
                return Ok(productDtos);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while fetching products by artist ID {artistId}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching products.");
            }
        }

        // Get: api/Products/price/pricetange
        [HttpGet("by-price-range")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByPriceRange(
            [FromQuery] decimal minPrice,
            [FromQuery] decimal maxPrice,
            [FromQuery] int? page = null,
            [FromQuery] int? pageSize = null)
        {
            try
            {
                if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
                    return BadRequest("Invalid price range.");

                if (page <= 0 || pageSize <= 0)
                    return BadRequest("Page and PageSize must be greater than zero.");

                _logger.LogInformation($"Fetching products with price range {minPrice} - {maxPrice}, page {page}, page size {pageSize}.");

                var products = await _productService.GetProductsByPriceRangeAsync(minPrice, maxPrice, page, pageSize);
                if (products == null || !products.Any())
                {
                    _logger.LogWarning($"No products found in price range {minPrice} - {maxPrice}.");
                    return NotFound($"No products found in the specified price range {minPrice} - {maxPrice}.");
                }

                var productDtos = products.Adapt<IEnumerable<ProductDTO>>();
                return Ok(productDtos);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while fetching products in price range {minPrice} - {maxPrice}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching products.");
            }
        }


    }
}
