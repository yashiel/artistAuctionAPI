using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Enum;
using api.Models;
using api.Models.DTOs;
using api.Services;
using api.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                    return BadRequest("Page and PageSize must be greater than zero.");
                _logger.LogInformation($"Fetching products from page {page} with page size {pageSize}.");


                _logger.LogInformation("Fetching all orders from the database.");
                var orders = await _orderService.GetAllOrdersAsync(page, pageSize);
                if (orders == null || !orders.Any())
                {
                    _logger.LogWarning("No orders found in the database.");
                    return NotFound("No orders found.");
                }
                _logger.LogInformation($"Found {orders.Count()} orders.");
                var orderDtos = orders.Adapt<IEnumerable<OrderDTO>>();
                return Ok(orderDtos);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching orders.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching orders.");
            }
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
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
                var orderDto = order.Adapt<OrderDTO>();
                return Ok(orderDto);
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
        public async Task<IActionResult> PutOrder(int id, OrderDTO orderDto)
        {
            try
            {
                if (id != orderDto.Id)
                {
                    _logger.LogWarning($"Order ID mismatch: {id} does not match {orderDto.Id}.");
                    return BadRequest("Order ID mismatch.");
                }
                _logger.LogInformation($"Updating order with ID {id}.");
                var order = orderDto.Adapt<Order>();
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
        public async Task<ActionResult<OrderDTO>> PostOrder(OrderDTO orderDto)
        {
            try
            {
                if (orderDto == null)
                {
                    _logger.LogWarning("Received null order object.");
                    return BadRequest("Order cannot be null.");
                }
                _logger.LogInformation("Adding a new order to the database.");
                
                var order = orderDto.Adapt<Order>();
                await _orderService.AddOrderAsync(order);

                var createdOrderDto = order.Adapt<OrderDTO>();
                _logger.LogInformation($"Order with ID {order.Id} created successfully.");
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, createdOrderDto);
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
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersByCustomerEmail(string email)
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
                var orderDtos = orders.Adapt<IEnumerable<OrderDTO>>();
                return Ok(orderDtos);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching orders by customer email.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching orders by customer email.");
            }
        }

        // GET: api/Orders/ByStatus/{status}
        [HttpGet("ByStatus/{status}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersByStatus(OrderStatus status)
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
                var orderDtos = orders.Adapt<IEnumerable<OrderDTO>>();
                return Ok(orderDtos);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching orders by status.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching orders by status.");
            }
        }

    }
}
