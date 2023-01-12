using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Domain.Shared.DashBoard
{
    public enum ChartType
    {
        [Description("pie")]
        Pie,

        [Description("line")]
        Line,

        [Description("bar")]
        Bar
    }
    public enum LegendOrLabel
    {
        [Description("region")]
        Region,

        [Description("province")]
        Province,

        [Description("district")]
        District,

        [Description("tech")]
        Tech,

        [Description("frequencyBand")]
        FreQuencyBand,

        [Description("vendor")]
        Vendor,

        [Description("configuration")]
        Configuration
    }
}
