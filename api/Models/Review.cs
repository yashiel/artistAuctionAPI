using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api.Models;

public class Review
{
    public int Id { get; set; }
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string? ReviewerName { get; set; }

    [Required]
    [EmailAddress]
    public string? ReviewerEmail { get; set; }

    [MaxLength(1000)]
    public string? Comment { get; set; } = string.Empty;
    
    [Range(1, 5)]
    public int Rating { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [JsonIgnore]
    public Product? Product { get; set; }
}