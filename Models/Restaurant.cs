using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FastTrak.Models
{
    public class Restaurant
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Display name shown on the Restaurants screen.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Optional internal identifier for custom logic later.
        /// </summary>
        public string Slug { get; set; } = string.Empty;
    }
}
