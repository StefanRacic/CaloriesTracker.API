using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaloriesTracker.Models
{
    public class Meal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Calories { get; set; }
        public int UserId { get; set; }
    }
}
