using System.Collections.Generic;

namespace GraphAWSJsonData.Models.SData
{
    public class SDataRoot
    {
        public string SDataTable { get; set; }
        public List<SDataPoint> SDataPoints { get; set; }
    }
}
