using JSONImport_COCA.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JSONImport_COCA.Report
{
    class Results
    {
        //Question 1

        public Projects LoadFile(string sourcePath)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("1.How many successful deployments have taken place ?");

                using (StreamReader r = new StreamReader(sourcePath))
                {
                    string json = r.ReadToEnd();
                    var deserializedObject = JsonConvert.DeserializeObject<Projects>(json);

                    return deserializedObject;
                }

            }
            catch (Exception e)
            {
                throw new Exception($"Error loading JSON file. Error: {e.Message}");
            }

        }

        public int SuccessfulDeployments(Projects importFile)
        {
            try
            {
                var successfulDeployments = importFile.Project
                    .SelectMany(x => x.Releases)
                    .SelectMany(y => y.Deployments)
                    .Count(z => z.State == "Success");

                return successfulDeployments;
            }
            catch (Exception e)
            {
                throw new Exception($"Error when calculating the successful deployments. Error: {e.Message}");
            }
        }

        // Question 2 (a) 
        public StringBuilder SuccessfulDeploymentsByProjectGroup(Projects importFile)
        {
            try
            {
                var output = new StringBuilder();

                var projectGroups = importFile.Project.GroupBy(x => x.ProjectGroup);

                foreach (var projectGroup in projectGroups.OrderBy(x => x.Key))
                {
                    var deployments = projectGroup.SelectMany(x => x.Releases)
                        .SelectMany(d => d.Deployments).Count(x => x.State == "Success");

                    output.AppendLine($"Project group: {projectGroup.Key} - Successful deployments: {deployments}"); 
                }

                return output;

            }
            catch (Exception e)
            {
                throw new Exception($"Error when getting successful deployments by project group. Error: {e.Message}", e);
            }
        }

        // Question 2 (b) 
        public StringBuilder SuccessfulDeploymentsByEnvironment(Projects importFile)
        {
            try
            {
                var output = new StringBuilder();

                var environmentGroups = importFile.Project.SelectMany(x => x.Releases)
                    .SelectMany(d => d.Deployments).Where(x => x.State == "Success").GroupBy(x => x.Environment);

                foreach (var environmentGroup in environmentGroups.OrderBy(x => x.Key))
                {
                    var environmentCount = environmentGroup.Count();

                    output.AppendLine($"Environment: {environmentGroup.Key} - Successful deployments: {environmentCount}");

                }

                return output;

            }
            catch (Exception e)
            {
                throw new Exception($"Error when getting successful deployments by environment. Error: {e.Message}", e);
            }
        }

        // Question 2 (c) 
        public StringBuilder SuccessfulDeploymentsByYear(Projects importFile)
        {
            try
            {
                var output = new StringBuilder(); 

                var yearGroups = importFile.Project.SelectMany(x => x.Releases)
                    .SelectMany(d => d.Deployments).Where(x => x.State == "Success").GroupBy(x => x.Created.Year);


                foreach (var yearGroup in yearGroups.OrderBy(x => x.Key))
                {
                    var year = yearGroup.Count();

                    output.AppendLine($"Year: {yearGroup.Key} - Successful deployments: {year}");

                }

                return output;

            }
            catch (Exception e)
            {
                throw new Exception($"Error when getting successful deployments by year. Error: {e.Message}", e);
            }
        }


        public StringBuilder AverageReleaseTimes(Projects importFile)
        {
            try
            {
                var output = new StringBuilder();

                var groupedProjects = importFile.Project.OrderBy(x => x.ProjectGroup).GroupBy(x => x.ProjectGroup);

                foreach (var project in groupedProjects)
                {
                    var timespans = new List<TimeSpan>();

                    var allReleases = project
                        .SelectMany(x => x.Releases);

                    foreach (var release in allReleases)
                    {
                        var integration = release.Deployments.FirstOrDefault(x => x.Environment == "Integration");

                        var live = release.Deployments.FirstOrDefault(x => x.Environment == "Live");

                        //Check we have both times
                        if (integration != null && live != null)
                        {
                            timespans.Add(live.Created - integration.Created);
                        }
                    }

                    if (timespans.Any())
                    {
                        var ticks = timespans.Average(x => x.Ticks);
                        var longTicks = Convert.ToInt64(ticks);
                        var timespan = new TimeSpan(longTicks);

                        output.AppendLine($"Project group: {project.Key} - Average time between releases: {timespan.Days.ToString()} days, " +
                                          $"{timespan.Hours.ToString()} hours, {timespan.Minutes.ToString()} minutes, " +
                                          $"{timespan.Seconds.ToString()} seconds");
                    }
                }

                return output;
            }
            catch (Exception e)
            {
                throw new Exception($"Error when Average times. Error: {e.Message}",
                    e);
            }
        }


        public string MostDeploymentOnDayOfWeek(Projects importFile)
        {
            try
            {
                var dayOfWeek = importFile.Project.SelectMany(x => x.Releases)
                    .SelectMany(x => x.Deployments)
                    .Where(x => x.Name == "Deploy to Live")
                    .GroupBy(x => x.Created.DayOfWeek).OrderByDescending(x => x.Count()).First();

                return dayOfWeek.Key.ToString();
            }
            catch (Exception e)
            {
                throw new Exception($"Error when calculating the successful deployments. Error: {e.Message}");
            }
        }



        public StringBuilder ReleasesByProjectGroup(Projects importFile)
        {
            try
            {
                var output = new StringBuilder();

                var projectGroups = importFile.Project.GroupBy(x => x.ProjectGroup);

                foreach (var projectGroup in projectGroups)
                {
                    output.AppendLine($"Project group: {projectGroup.Key.ToString()}");

                    var successfulReleases = projectGroup.SelectMany(x => x.Releases)
                        .SelectMany(y => y.Deployments)
                        .Count(y => y.Environment == "Live" && y.State == "Success");

                    output.AppendLine($"    Successful deployments : {successfulReleases.ToString()}");

                    var unSuccessfulReleases = projectGroup.SelectMany(x => x.Releases)
                        .SelectMany(y => y.Deployments)
                        .Count(y => y.Environment == "Live" && y.State != "Success");

                    output.AppendLine($"    Unsuccessful deployments : {unSuccessfulReleases.ToString()}");
                }

                return output;
            }
            catch (Exception e)
            {
                throw new Exception($"Error when calculating the Releases By Project Group. Error: {e.Message}");
            }

        }



            public StringBuilder RepeatedSuccessfulReleases(Projects importFile)
            {
                try
                { 
                    var output = new StringBuilder();

                    var projectGroups = importFile.Project.GroupBy(x => x.ProjectGroup);


                    foreach (var project in projectGroups)
                    {

                        var unsuccessfulReleases = project
                            .SelectMany(x => x.Releases)
                            .Where(x => x.Deployments
                            .Any(y => y.Environment == "Live" && y.State != "Success"))
                            .ToList();

                        var successfulReleases = project
                            .SelectMany(x => x.Releases)
                            .Where(x => x.Deployments
                            .Any(y => y.Environment == "Live" && y.State == "Success"))
                            .ToList();

                        output.AppendLine("********************************************************************");
                        output.AppendLine($"Project Group: {project.Key}");
                        output.AppendLine("");
                        output.AppendLine($"Successful Releases: {successfulReleases.Count}");
                        output.AppendLine($"Unsuccessful Releases: {unsuccessfulReleases.Count}");

                        if (successfulReleases.Any())
                        {
                            output.AppendLine("");
                            output.AppendLine("**** Successful Releases ****");
                            output.AppendLine("");

                        foreach (var release in successfulReleases)
                        {
                            var distinctEnvironments =
                                release.Deployments.Select(x => x.Environment).Distinct().Count();
                            var repeatedlyDeployedEnvironments = string.Join(", ",
                                release.Deployments
                                    .OrderBy(x => x.Environment)
                                    .GroupBy(x => x.Environment)
                                    .Where(x => x.Count() > 1)
                                    .Select(x => x.Key));

                            output.AppendLine($"Release version:                    {release.Version}");
                            output.AppendLine($"Number of distinct deployments:     {distinctEnvironments}");
                            if (repeatedlyDeployedEnvironments.Length > 1)
                            {
                                output.AppendLine(
                                    $"Repeatedly deployed environments:   {repeatedlyDeployedEnvironments}");
                            }

                            output.AppendLine("");
                        }

                        output.AppendLine("");
                        }

                        if (unsuccessfulReleases.Any())
                        {
                            output.AppendLine("**** Unsuccessful Releases ****");
                            output.AppendLine("");

                            foreach (var release in unsuccessfulReleases)
                            {
                                var distinctEnvironments =
                                    release.Deployments.Select(x => x.Environment).Distinct().Count();
                                var repeatedlyDeployedEnvironments = string.Join(", ",
                                release.Deployments
                                    .OrderBy(x => x.Environment)
                                    .GroupBy(x => x.Environment)
                                    .Where(x => x.Count() > 1)
                                    .Select(x => x.Key));

                                output.AppendLine($"release version:                    {release.Version}");
                                output.AppendLine($"Number of distinct deployments:     {distinctEnvironments}");
                                if (repeatedlyDeployedEnvironments.Length > 1)
                                {
                                    output.AppendLine($"Repeatedly deployed environments:   {repeatedlyDeployedEnvironments}");
                                }

                                output.AppendLine("");
                            }

                            output.AppendLine("");
                        }

                        output.AppendLine("");
                    }

                    return output;
                }
                catch (Exception e)
                {
                    throw new Exception($"Error when calculating the Repeated Successful Releases. Error: {e.Message}");
                }
            }

    }
}
