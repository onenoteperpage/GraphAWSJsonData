using System.Collections.Generic;
using GraphAWSJsonData.Models.JData;
using Newtonsoft.Json;

namespace GraphAWSJsonData.Models.Utilizations
{
    public class CpuUtilization
    {
        [JsonProperty("Label")]
        public string Label { get; set; }

        [JsonProperty("Datapoints")]
        public List<JDatapoint> JDataPoints { get; set; }
    }
}
