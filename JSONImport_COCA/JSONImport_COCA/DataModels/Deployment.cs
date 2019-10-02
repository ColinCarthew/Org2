using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSONImport_COCA.DataModels
{
    public class Deployment
    {
        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
