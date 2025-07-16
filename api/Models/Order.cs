using System.ComponentModel.DataAnnotations;
using api.Enum;

namespace api.Models;

public class Order
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Required]
    public OrderStatus OrderStatus { get; set; }

    public ICollection<OrderItem>? OrderItems { get; set; }
    
    [Range(0.0, 100000.0)]
    public decimal TotalAmount { get; set; } = 0.0m;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}