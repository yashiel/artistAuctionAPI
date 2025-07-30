using System.ComponentModel.DataAnnotations;

namespace api.Models.DTOs;

public class ProductDTO
{
    public int Id { get; set; }
    [Required]
    public int ArtistId { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(5000)]
    public string Description { get; set; } = string.Empty;

    [Range(0.0, 100000.0)]
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int CategoryId { get; set; }

    public List<ReviewDTO> Reviews { get; set; } = new();
}

public class CreateProductDTO
{
    [Required]
    public int ArtistId { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(5000)]
    public string Description { get; set; } = string.Empty;
    [Range(0.0, 100000.0)]
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int CategoryId { get; set; }
}

public class UpdateProductDTO
{
    [Required]
    public int ArtistId { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(5000)]
    public string Description { get; set; } = string.Empty;
    [Range(0.0, 100000.0)]
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int CategoryId { get; set; }
}