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

        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }
        public int Sodium { get; set; }

        // Category (optional)
        public string Category { get; set; } = string.Empty;
    }
}
