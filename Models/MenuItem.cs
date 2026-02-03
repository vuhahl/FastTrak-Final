using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTrak.Models
{
    public class MenuItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int RestaurantId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Calories { get; set; }

        public decimal Protein { get; set; }
        public decimal Carbs { get; set; }
        public decimal Fat { get; set; }
        public int Sodium { get; set; }

        // Category (optional)
        public string Category { get; set; } = string.Empty;
    

    // Decide whether this item should bypass customization and be added directly
    public bool IsDirectAdd =>
    string.Equals(Category, "Sauces", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(Category, "Sides", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(Category, "Donuts", StringComparison.OrdinalIgnoreCase);
    
    } 
}
