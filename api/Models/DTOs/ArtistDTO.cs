using System.ComponentModel.DataAnnotations;


namespace api.Models.DTOs;

public class ArtistDTO
{
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
    public string? WebsiteUrl { get; set; }
    public string? SocialMediaLinks { get; set; }
}


public class CreateArtistDTO
{
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
    public string? WebsiteUrl { get; set; }
    public string? SocialMediaLinks { get; set; }
}