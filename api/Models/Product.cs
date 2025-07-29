using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api.Models;

public class Product
{
    public int Id { get; set; }
    [Required]
    public int ArtistId{ get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(5000)]
    public string Description { get; set; } = string.Empty;

    [Range(0.0, 100000.0)]
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public int CategoryId { get; set; }
    
    [JsonIgnore]
    public Category? Category { get; set; }

    [JsonIgnore]
    public Artist? Artist { get; set; }

    [JsonIgnore]
    public ICollection<Review>? Reviews { get; set; }
}