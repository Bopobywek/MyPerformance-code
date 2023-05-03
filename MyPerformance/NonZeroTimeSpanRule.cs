using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPerformance
{
    public class NonZeroTimeSpanRule : IValidationRule<TimeSpan>
    {
        public string ValidationMessage { get; set; }

        public bool Check(TimeSpan value)
        {
            return value != TimeSpan.Zero;
        }
    }
}
