using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTrak.Models
{
    public partial class SelectableOption : ObservableObject
    {
        [ObservableProperty]
        private bool isSelected;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }

        public int Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbs { get; set; }
        public decimal Fat { get; set; }
        public int Sodium { get; set; }

        public string NutritionSummary =>
            $"{Calories} cal | {Protein}g P | {Carbs}g C | {Fat}g F | {Sodium}mg Na";


        public SelectableOption(CustomOption opt)
        {
            Id = opt.Id;
            Name = opt.Name;
            Category = opt.Category;

            Calories = (int)opt.Calories;
            Protein = (decimal)opt.Protein;
            Carbs = (decimal)opt.Carbs;
            Fat = (decimal)opt.Fat;
            Sodium = (int)opt.Sodium;
        }
    }
}
