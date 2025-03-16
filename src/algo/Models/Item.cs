using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algo.Models
{
    public class Item
    {
        public int Weight { get; set; }
        public int Value { get; set; }
        public Item(int weight, int value)
        {
            Weight = weight;
            Value = value;
        }
    }
}
