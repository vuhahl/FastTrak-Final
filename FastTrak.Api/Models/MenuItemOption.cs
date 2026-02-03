namespace FastTrak.Api.Models;

public class MenuItemOption
{
    public int Id { get; set; }

    public int MenuItemId { get; set; }
    public int CustomOptionId { get; set; }

    // Navigation properties
    public MenuItem MenuItem { get; set; } = null!;
    public CustomOption CustomOption { get; set; } = null!;
}
