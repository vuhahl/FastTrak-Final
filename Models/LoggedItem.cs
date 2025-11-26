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

        public int Quantity { get; set; } = 1;

        public DateTime LoggedAt { get; set; } = DateTime.Now;

    }
}
