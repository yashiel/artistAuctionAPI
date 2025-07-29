using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;
using api.Services;
using api.Services.Interfaces;

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
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                _logger.LogInformation("Fetching all products from the database.");
                var products = await _productService.GetAllProductsAsync();
                if (products == null || !products.Any())
                {
                    _logger.LogWarning("No products found in the database.");
                    return NotFound("No products found.");
                }
                _logger.LogInformation($"Found {products.Count()} products.");
                return Ok(products);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching products.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching products.");
            }
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
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
                return Ok(product);
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
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("Product ID mismatch.");
            }

            try
            {
                _logger.LogInformation($"Updating product with ID {id}.");
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
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            try
            {
                if (product == null)
                {
                    _logger.LogWarning("Received null product in POST request.");
                    return BadRequest("Product cannot be null.");
                }
                _logger.LogInformation("Creating a new product.");
                    await _productService.AddProductAsync(product);
                        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);

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
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            try
            {
                _logger.LogInformation($"Fetching products in category '{category}'.");
                var products = await _productService.GetProductsByCategoryAsync(category);
                if (products == null || !products.Any())
                {
                    _logger.LogWarning($"No products found in category '{category}'.");
                    return NotFound($"No products found in category '{category}'.");
                }
                return Ok(products);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while fetching products in category '{category}'.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching products.");
            }
        }
        // GET: api/Products/search/{searchTerm}
        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts(string searchTerm)
        {
            try
            {
                _logger.LogInformation($"Searching for products with term '{searchTerm}'.");
                var products = await _productService.SearchProductsAsync(searchTerm);
                if (products == null || !products.Any())
                {
                    _logger.LogWarning($"No products found matching search term '{searchTerm}'.");
                    return NotFound($"No products found matching search term '{searchTerm}'.");
                }
                return Ok(products);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while searching for products with term '{searchTerm}'.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while searching for products.");
            }
        }

        // GET: api/Products/artist/{artistId}
        [HttpGet("artist/{artistId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByArtist(int artistId)
        {
            try
            {
                _logger.LogInformation($"Fetching products by artist ID {artistId}.");
                var products = await _productService.GetProductsByArtistAsync(artistId);
                if (products == null || !products.Any())
                {
                    _logger.LogWarning($"No products found for artist ID {artistId}.");
                    return NotFound($"No products found for artist ID {artistId}.");
                }
                return Ok(products);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while fetching products by artist ID {artistId}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching products.");
            }
        }
    }
}
