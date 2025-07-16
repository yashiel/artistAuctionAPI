using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class Review
{
    public int Id { get; set; }
    [Required]
    public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Required]
    [MaxLength(100)]
    public string? ReviewerName { get; set; }

    [MaxLength(1000)]
    public string? Comment { get; set; } = string.Empty;
    
    [Range(1, 5)]
    public int Rating { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}