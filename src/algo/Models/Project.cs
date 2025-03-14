using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algo.Models
{
    public record Project
    {
        public int Id { get; set; }
        public int T { get; set; }
        public int Q { get; set; }
        public int C { get; set; }  

        public Project(int id, int t, int q, int c)
        {
            Id = id;
            T = t;
            Q = q;
            C = c;
        }
    }
}
