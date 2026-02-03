using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastTrak.Api.Models;

public class CustomOption
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
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

    // Navigation property
    public List<MenuItemOption> MenuItemOptions { get; set; } = new();
}
