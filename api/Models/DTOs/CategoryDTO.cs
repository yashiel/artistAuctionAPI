using System.ComponentModel.DataAnnotations;

namespace api.Models.DTOs;

public class CategoryDTO
{
    public int Id { get; set; }
    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;
}

public class CreateCategoryDTO
{
    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;
}