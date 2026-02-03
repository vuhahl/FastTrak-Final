using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTrak.Models
{
    public class LoggedItemOption
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int LoggedItemId { get; set; }
        public int CustomOptionId { get; set; }

        // Store values at time of logging
        public string OptionName { get; set; }
        public int Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbs { get; set; }
        public decimal Fat { get; set; }
        public int Sodium { get; set; }

    }
}
