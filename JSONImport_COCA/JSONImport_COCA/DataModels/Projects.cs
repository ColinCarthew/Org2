using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSONImport_COCA.DataModels
{
    public class Projects
    {
        [JsonProperty("projects")]
        public ICollection<Project> Project { get; set; }
    }
}
