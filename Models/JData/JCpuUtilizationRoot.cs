using Newtonsoft.Json;
using System.Collections.Generic;

namespace GraphAWSJsonData.Models.JData
{
    public class JCpuUtilizationRoot
    {
        [JsonProperty("Label")]
        public string Label { get; set; }

        [JsonProperty("Datapoints")]
        public List<JDatapoint> JDatapoints { get; set; }
    }
}
