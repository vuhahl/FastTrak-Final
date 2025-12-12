using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FastTrak.Models
{
    public class LoggedItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int MenuItemId { get; set; }

        // Cloud items will use override fields instead
        public string NameOverride { get; set; }
        public int CaloriesOverride { get; set; }
        public decimal ProteinOverride { get; set; }
        public decimal CarbsOverride { get; set; }
        public decimal FatOverride { get; set; }
        public int SodiumOverride { get; set; }


        public int Quantity { get; set; } = 1;

        public DateTime LoggedAt { get; set; } = DateTime.Now;

        [Ignore]
        public List<LoggedItemOption> Options { get; set; } = new();


        // Computed properties for display
        public string DisplayName =>
            string.IsNullOrWhiteSpace(NameOverride)
                ? $"Item #{MenuItemId}" // fallback if needed
                : NameOverride;

        public int DisplayCalories => CaloriesOverride;

    }
}
