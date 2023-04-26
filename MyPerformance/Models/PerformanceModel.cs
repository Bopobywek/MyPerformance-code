using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPerformance.Models
{
    internal class PerformanceModel
    {
        public string Name { get; set; }
        public TimeSpan Duration { get; set; } = new TimeSpan(0, 2, 3);
        public DateTime Date { get; set; } = new DateTime(2023, 3, 5);
        public PerformancePartModel[] PerformanceParts { get; set; }
    }
}
