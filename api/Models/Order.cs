using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using api.Enum;

namespace api.Models;

public class Order
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
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [JsonIgnore]
    public ICollection<OrderItem>? OrderItems { get; set; }
}