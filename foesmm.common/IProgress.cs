using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace foesmm.common
{
    public interface IProgress
    {
        string TaskTitle { get; set; }
        string Overall { get; set; }
        double OverallDone { get; set; }
        double OverallTotal { get; set; }
        string Step { get; set; }
        double StepDone { get; set; }
        double StepTotal { get; set; }
    }
}
