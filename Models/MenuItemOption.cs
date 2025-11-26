using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;   

namespace FastTrak.Models
{
    public class MenuItemOption
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int MenuItemId { get; set; }
        public int CustomOptionId { get; set; }
    }
}
