using System.ComponentModel.DataAnnotations;
using api.Enum;

namespace api.Models.DTOs;

public class OrderDTO
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required]
    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Required]
    public OrderStatus OrderStatus { get; set; }

    [Range(0.0, 100000.0)]
    public decimal TotalAmount { get; set; } = 0.0m;

    public ICollection<OrderItem>? OrderItems { get; set; }
}

public class CreateOrderDTO
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string CustomerName { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string CustomerEmail { get; set; } = string.Empty;
    [Required]
    public DateTime OrderDate { get; set; } = DateTime.Now;
    [Required]
    public OrderStatus OrderStatus { get; set; }
    [Range(0.0, 100000.0)]
    public decimal TotalAmount { get; set; } = 0.0m;
    public ICollection<OrderItem>? OrderItems { get; set; }
}