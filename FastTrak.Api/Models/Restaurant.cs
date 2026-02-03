using System.ComponentModel.DataAnnotations;

namespace FastTrak.Api.Models;

public class Restaurant
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Slug { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property - EF Core uses this to load related MenuItems
    public List<MenuItem> MenuItems { get; set; } = new();
}
