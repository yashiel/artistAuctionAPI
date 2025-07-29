using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Enum;
using api.Models;
using api.Services;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            try
            {
                _logger.LogInformation("Fetching all orders from the database.");
                var orders = await _orderService.GetAllOrdersAsync();
                if (orders == null || !orders.Any())
                {
                    _logger.LogWarning("No orders found in the database.");
                    return NotFound("No orders found.");
                }
                _logger.LogInformation($"Found {orders.Count()} orders.");
                return Ok(orders);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching orders.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching orders.");
            }
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching order with ID {id} from the database.");
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    _logger.LogWarning($"Order with ID {id} not found.");
                    return NotFound($"Order with ID {id} not found.");
                }
                return Ok(order);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching the order.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the order.");
            }
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            try
            {
                if (id != order.Id)
                {
                    _logger.LogWarning($"Order ID mismatch: {id} does not match {order.Id}.");
                    return BadRequest("Order ID mismatch.");
                }
                _logger.LogInformation($"Updating order with ID {id}.");
                await _orderService.UpdateOrderAsync(id, order);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _orderService.GetOrderByIdAsync(id) == null )
                {
                    _logger.LogWarning($"Order with ID {id} not found for update.");
                    return NotFound($"Order with ID {id} not found.");
                }
                else
                {
                    _logger.LogError($"An error occurred while updating order with ID {id}.");
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the order.");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while updating order with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the order.");
            }
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            try
            {
                if (order == null)
                {
                    _logger.LogWarning("Received null order object.");
                    return BadRequest("Order cannot be null.");
                }
                _logger.LogInformation("Adding a new order to the database.");
                await _orderService.AddOrderAsync(order);
                _logger.LogInformation($"Order with ID {order.Id} created successfully.");
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while adding a new order.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the order.");
            }
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    _logger.LogWarning($"Order with ID {id} not found for deletion.");
                    return NotFound($"Order with ID {id} not found.");
                }
                _logger.LogInformation($"Deleting order with ID {id} from the database.");
                await _orderService.DeleteOrderAsync(id);
                _logger.LogInformation($"Order with ID {id} deleted successfully.");
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while deleting order with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the order.");
            }
        }

        // GET: api/Orders/ByCustomerEmail/{email}
        [HttpGet("ByCustomerEmail/{email}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByCustomerEmail(string email)
        {
            try
            {
                _logger.LogInformation($"Fetching orders for customer with email {email}.");
                var orders = await _orderService.GetOrdersByCustomerEmailAsync(email);
                if (orders == null || !orders.Any())
                {
                    _logger.LogWarning($"No orders found for customer with email {email}.");
                    return NotFound($"No orders found for customer with email {email}.");
                }
                return Ok(orders);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching orders by customer email.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching orders by customer email.");
            }
        }

        // GET: api/Orders/ByStatus/{status}
        [HttpGet("ByStatus/{status}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByStatus(OrderStatus status)
        {
            try
            {
                _logger.LogInformation($"Fetching orders with status {status}.");
                var orders = await _orderService.GetOrdersByStatusAsync(status);
                if (orders == null || !orders.Any())
                {
                    _logger.LogWarning($"No orders found with status {status}.");
                    return NotFound($"No orders found with status {status}.");
                }
                return Ok(orders);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching orders by status.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching orders by status.");
            }
        }

    }
}
