using System.ComponentModel.DataAnnotations;

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
    public DateTime? DeathDate { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? SocialMediaLinks { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Product>? Products { get; set; }
    public ICollection<EventArtist>? EventArtists { get; set; }
}