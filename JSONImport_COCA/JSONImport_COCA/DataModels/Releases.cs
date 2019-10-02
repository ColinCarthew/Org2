using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSONImport_COCA.DataModels
{
    public class Releases
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("deployments")]
        public ICollection<Deployment> Deployments { get; set; }
    }
}
