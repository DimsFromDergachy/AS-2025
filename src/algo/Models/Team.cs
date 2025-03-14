using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algo.Models
{
    public record Team
    {
        public int Id { get; set; }
        public int Efficiency { get; set; }

        public Team(int id, int efficiency)
        {
            Id = id;
            Efficiency = efficiency;
        }
    }
}
