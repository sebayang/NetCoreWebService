using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.ViewModels
{
    public class ChartVM
    {
    }

    public class PieChartVM
    {
        public string DepartmentName { get; set; }
        public int Total { get; set; }
    }

    public class BarChartVM
    {
        public string DepartmentName { get; set; }
    }
}
