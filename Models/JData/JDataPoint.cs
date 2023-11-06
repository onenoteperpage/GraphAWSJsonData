using Newtonsoft.Json;
using System;

namespace GraphAWSJsonData.Models.JData
{
    public class JDatapoint
    {
        [JsonProperty("Timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("Average")]
        public double Average { get; set; }

        [JsonProperty("Unit")]
        public string Unit { get; set; }
    }
}
