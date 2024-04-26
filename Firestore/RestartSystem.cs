using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knexus.Helper
{
    public class RestartSytem
    {
        public static async void RestartKnexus(string restart)
        {
            await Task.Delay(3);
            KnexusRestart(restart);
        }
        public static void KnexusRestart(string restart)
        {
            try
            {
                //_seriaLogger.LogInformation($"BucketName: {bucketName}, TargetFileNameToDownload: {targetFileNameToDownload}");
                PopulateKnexusRestarter(restart);

                var currLoc = AppDomain.CurrentDomain.BaseDirectory;
                Process userAppProcess = new();
                //_seriaLogger.LogInformation($"Current AppDomain Location : {currLoc}");
                userAppProcess.StartInfo.FileName = currLoc + "\\FirestoreInstaller\\FirestoreInstaller.exe";
                userAppProcess.StartInfo.WorkingDirectory = currLoc;
                userAppProcess.StartInfo.UseShellExecute = false;
                userAppProcess.StartInfo.RedirectStandardInput = true;
                //_seriaLogger.LogInformation($"Starting {currLoc} + \\FirestoreInstaller\\FirestoreInstaller.exe");
                userAppProcess.Start();

                Environment.Exit(0);
            }
            catch (Exception e)
            {
                //_seriaLogger.LogError($"ConsoleUpdater: Exception {e.Message}");
                Trace.WriteLine(e.Message);
            }
        }

        private static void PopulateKnexusRestarter(string restart)
        {
            // Create a data object
            var data = new InstallerData
            {
                IsUpdate = restart,
                BucketName = "",
                TargetFileNameToDownload = "",

            };
            // Write the data object to the data.json file
            WriteJsonToFile(data);
        }

        private static void WriteJsonToFile(InstallerData data)
        {
            // Construct the file path for the data.json file
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\SETTING\\data.json";

            // Convert the data object to JSON
            string jsonData = JsonConvert.SerializeObject(data);

            if (File.Exists(filePath))
            {
                // Delete the file if it already exists
                File.Delete(filePath);
            }
            // Write the JSON data to the file
            File.WriteAllText(filePath, jsonData);
        }
    }

    public class InstallerData
    {
        // Properties to store installer data
        public string IsUpdate { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
        public string TargetFileNameToDownload { get; set; } = string.Empty;
    }
}

