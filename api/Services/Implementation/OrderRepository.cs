using api.Data;
using api.Enum;
using api.Models;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Services.Implementation;

public class OrderRepository : IOrderService
{
    public readonly AuctionDbContext _context;
    public OrderRepository(AuctionDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Order>> GetAllOrdersAsync(int? page = null, int? pageSize = null)
    {
        var query = _context.Orders
            .AsQueryable();

        // Apply pagination only if both page and pageSize are passed
        if (page.HasValue && pageSize.HasValue)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }

        return await query.ToListAsync();
    }
    public async Task<Order> GetOrderByIdAsync(int id)
    {
        return await _context.Orders.FindAsync(id);
    }
    public async Task AddOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateOrderAsync(int id, Order order)
    {
        _context.Entry(order).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    public async Task DeleteOrderAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<IEnumerable<Order>> GetOrdersByCustomerEmailAsync(string email)
    {
        return await _context.Orders
            .Where(o => o.CustomerEmail == email)
            .ToListAsync();
    }
    public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
    {
        return await _context.Orders
            .Where(o => o.OrderStatus == status)
            .ToListAsync();
    }
}