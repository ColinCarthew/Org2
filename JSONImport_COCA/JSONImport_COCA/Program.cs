using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using JSONImport_COCA.DataModels;
using System.Text;
using JSONImport_COCA.Report;

namespace JSONImport_COCA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sourcePath = args[0];
            var targetPath = args[1];

            var report = new CreateReport(sourcePath);

            var output = new StringBuilder();

            output = report.Report(sourcePath);

            WriteToFile(output, targetPath);

        }


        public static void WriteToFile(StringBuilder sb, string targetPath)
        {
            try
            {
                System.IO.File.WriteAllText(targetPath, sb.ToString());
            }
            catch (Exception e)
            {
                throw new Exception($"Error when writing to file. Error: {e.Message}");
            }
        }


    }
}
