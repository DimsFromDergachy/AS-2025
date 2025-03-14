using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algo.Models
{
    public record ProjectInWork
    {
        public int ProjectId { get; set; }
        public int TeamId { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
    }
}
