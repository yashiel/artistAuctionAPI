using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class Category
{
    public int Id { get; set; }
    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}