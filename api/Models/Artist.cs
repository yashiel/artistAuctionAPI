using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api.Models;

public class Artist
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    public string? ImageUrl { get; set; }
    [MaxLength(500)]
    public string? Bio { get; set; }
    public string? Genre { get; set; }
    public string? Country { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; } = null;
    public string? WebsiteUrl { get; set; }
    public string? SocialMediaLinks { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    [JsonIgnore]
    public ICollection<Product>? Products { get; set; }
    [JsonIgnore]
    public ICollection<EventArtist>? EventArtists { get; set; }
}