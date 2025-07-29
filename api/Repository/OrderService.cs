using api.Data;
using api.Enum;
using api.Models;
using api.Services;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class OrderService : IOrderService
{
    public readonly AuctionDbContext _context;
    public OrderService(AuctionDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders.ToListAsync();
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