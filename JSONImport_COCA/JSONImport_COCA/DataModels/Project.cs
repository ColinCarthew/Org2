using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSONImport_COCA.DataModels
{
    public class Project
    {
        [JsonProperty("project_id")]
        public Guid ProjectId { get; set; }

        [JsonProperty("project_group")]
        public string ProjectGroup { get; set; }

        [JsonProperty("environments")]
        public ICollection<Environment> Environments { get; set; }

        [JsonProperty("releases")]
        public ICollection<Releases> Releases { get; set; }

    }
}
