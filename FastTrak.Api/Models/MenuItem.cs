using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastTrak.Api.Models;

public class MenuItem
{
    public int Id { get; set; }

    public int RestaurantId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    public int Calories { get; set; }

    [Column(TypeName = "decimal(6,2)")]
    public decimal Protein { get; set; }

    [Column(TypeName = "decimal(6,2)")]
    public decimal Carbs { get; set; }

    [Column(TypeName = "decimal(6,2)")]
    public decimal Fat { get; set; }

    public int Sodium { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Computed property - not stored in database
    [NotMapped]
    public bool IsDirectAdd =>
        string.Equals(Category, "Sauces", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(Category, "Sides", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(Category, "Donuts", StringComparison.OrdinalIgnoreCase);

    // Navigation properties
    public Restaurant Restaurant { get; set; } = null!;
    public List<MenuItemOption> MenuItemOptions { get; set; } = new();
}
