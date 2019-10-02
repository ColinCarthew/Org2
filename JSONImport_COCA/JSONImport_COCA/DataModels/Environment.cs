using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSONImport_COCA.DataModels
{
    public class Environment
    {
        [JsonProperty("environment")]
        public string name { get; set; }
    }
}
