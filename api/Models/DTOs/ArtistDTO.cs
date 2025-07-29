using System.ComponentModel.DataAnnotations;


namespace api.Models.DTOs;

public class ArtistDTO
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }
    public string? ImageUrl { get; set; }
    [MaxLength(500)]
    public string? Bio { get; set; }
    [StringLength(100, MinimumLength = 3)]
    public string? Genre { get; set; }
    public string? Country { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; } = null;
    public string? WebsiteUrl { get; set; }
    public string? SocialMediaLinks { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}