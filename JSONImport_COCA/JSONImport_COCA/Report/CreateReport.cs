using System.Text;

namespace JSONImport_COCA.Report
{
    public class CreateReport
    {
        private string sourcePath;

        public CreateReport(string sourcePath)
        {
            this.sourcePath = sourcePath;
        }

        public StringBuilder Report(string sourcePath)
        {
            var results = new Results();

            var importedFile = results.LoadFile(sourcePath);

            var question1 = results.SuccessfulDeployments(importedFile).ToString();

            var question3 = results.MostDeploymentOnDayOfWeek(importedFile);

            var question2A = results.SuccessfulDeploymentsByProjectGroup(importedFile);

            var question2B = results.SuccessfulDeploymentsByEnvironment(importedFile);

            var question2C = results.SuccessfulDeploymentsByYear(importedFile);

            var question4 = results.AverageReleaseTimes(importedFile);

            var question5A = results.ReleasesByProjectGroup(importedFile);

            var question5B = results.RepeatedSuccessfulReleases(importedFile);

            var output = new StringBuilder();

            output.AppendLine("Org2test for COCA");
            output.AppendLine("");
            output.AppendLine("1.How many successful deployments have taken place?");
            output.AppendLine("");
            output.AppendLine(question1);
            output.AppendLine("");
            output.AppendLine("");

            output.AppendLine("2. How does this break down by project group, by environment, by year?");
            output.AppendLine("");
            output.AppendLine("**** By project group ****");

            output.AppendLine(question2A.ToString());
            output.AppendLine("");

            output.AppendLine("**** By environment ****");
            output.AppendLine(question2B.ToString());
            output.AppendLine("");

            output.AppendLine("**** By year ****");
            output.AppendLine(question2C.ToString());
            output.AppendLine("");
            output.AppendLine("");

            output.AppendLine("3. Which is the most popular day of the week for live deployments?");
            output.AppendLine("");
            output.AppendLine(question3);
            output.AppendLine("");
            output.AppendLine("");

            output.AppendLine("4. What is the average length of time a release takes from integration to live, by project group?");
            output.AppendLine("");
            output.AppendLine(question4.ToString());
            output.AppendLine("");
            output.AppendLine("");

            output.AppendLine("5. Please provide a break down by project group of success and unsuccessful releases (successful being releases that have been deployed to live), ");
            output.AppendLine("the number of deployments involved in the release pipeline and whether some environments had to be repeatedly deployed.");
            output.AppendLine("");
            output.AppendLine("Releases by project group");
            output.Append(question5A.ToString());
            output.AppendLine("");
            output.AppendLine("");
            output.AppendLine("Releases by project group");
            output.Append(question5B.ToString());

            return output;

        }
    }
}


