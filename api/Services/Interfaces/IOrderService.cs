﻿using api.Enum;
using api.Models;

namespace api.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllOrdersAsync(int? page = null, int? pageSize = null);
    Task<Order> GetOrderByIdAsync(int id);
    Task AddOrderAsync(Order order);
    Task UpdateOrderAsync(int id, Order order);
    Task DeleteOrderAsync(int id);
    Task<IEnumerable<Order>> GetOrdersByCustomerEmailAsync(string email);
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
}