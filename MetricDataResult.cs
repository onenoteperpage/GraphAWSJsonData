using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAWSJsonData
{
    public class MetricDataResult
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string[] Timestamps { get; set; }
        public double[] Values { get; set; }
        public string StatusCode { get; set; }
    }
}
