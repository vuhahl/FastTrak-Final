using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTrak.Models
{
    public class OptionGroup
    {
        public string Category { get; set; }
        public ObservableCollection<SelectableOption> Options { get; set; }
    }
}
