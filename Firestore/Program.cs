/*//TO UPLOAD PUBLISH FILE TO FIREBASE STORAGE
using System;
using System.IO;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;

namespace FirebaseStorageUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            InitializeFirestoreDb();


            var storage = StorageClient.Create();

            // Path to the zip file you want to upload
            string filePath = "C://Users//LENOVO//Desktop//publish.zip";
            string bucketName = "iot-kefa.appspot.com";

            // Upload the zip folder
            using (var stream = File.OpenRead(filePath))
            {
                storage.UploadObject(bucketName, "kefa-publish/publish.zip", null, stream);
            }

            Console.WriteLine("Zip folder uploaded successfully.");
        }

        public static FirestoreDb InitializeFirestoreDb()
        {
            var filePath = GetFirestoreCredentialsFilePath();
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);
            Console.WriteLine(filePath);
            return FirestoreDb.Create("iot-kefa");
        }

        private static string GetFirestoreCredentialsFilePath()
        {
            var dirPath = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.GetFullPath(Path.Combine(dirPath, "iot-kefa.json"));
            return filePath;
        }
    }
}*/



//TO DOWNLOAD PUBLISH FILE FROM FIREBASE STORAGE
/*using System;
using System.IO;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;

namespace FirebaseStorageUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            InitializeFirestoreDb();

            var storage = StorageClient.Create();
            string bucketName = "iot-kefa.appspot.com";
            string objectName = "kefa-publish/publish.zip";


            string downloadDirectory = "C://Users//LENOVO//Desktop//firebase-downloads";
            string downloadFilePath = Path.Combine(downloadDirectory, "publish.zip");

            // Download the zip folder
            using (var outputFile = File.OpenWrite(downloadFilePath))
            {
                storage.DownloadObject(bucketName, objectName, outputFile);
            }

            Console.WriteLine("Zip folder downloaded successfully.");
        }

        public static FirestoreDb InitializeFirestoreDb()
        {
            var filePath = GetFirestoreCredentialsFilePath();
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);
            Console.WriteLine(filePath);
            return FirestoreDb.Create("iot-kefa");
        }

        private static string GetFirestoreCredentialsFilePath()
        {
            var dirPath = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.GetFullPath(Path.Combine(dirPath, "iot-kefa.json"));
            return filePath;
        }
    }
}*/



using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using DriveFolderDownload;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FirebaseStorageUpload
{
   
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("\n <<<Main>>> ");
            InstallerData installerData = new ();
            //Console.WriteLine("\n <<<InstallerData>>> ");
            installerData = GetInfo();
            //Console.WriteLine($"\n <<<installerData>>> {installerData}");
            var isUpdate = bool.Parse(installerData.IsUpdate);
            //Console.WriteLine($"\n <<<isUpdate>>> {isUpdate}");
            var bucketName = installerData.BucketName;
            //Console.WriteLine($"\n <<<bucketName>>> {bucketName}");
            var targetFileToDownload = installerData.TargetFileNameToDownload;
            //Console.WriteLine($"\n <<<targetFileToDownload>>> {targetFileToDownload}");
            if (installerData.IsUpdate == "True") UpdateOperations(isUpdate, bucketName, targetFileToDownload);
            else RollBackApp();          
        }
        public static InstallerData GetInfo()
        {
            var rootPath = GetRootFolder();
            //Console.WriteLine($"\n <<<rootPath>>> {rootPath}");
            string datafilePath = GetDataJsonPath(rootPath);
            //Console.WriteLine($"\n <<<datafilePath>>> {datafilePath}");
            if (File.Exists(datafilePath))
            {
                // Read the JSON data from the file
                string jsonData = File.ReadAllText(datafilePath);

                // Deserialize the JSON data to PUBGData object
                InstallerData? installerData = JsonConvert.DeserializeObject<InstallerData>(jsonData);

                File.Delete(datafilePath);
                return installerData!;
            }
            else
            {
                // Return a new instance of InstallerData with default values
                InstallerData installerData = new InstallerData
                {
                    IsUpdate = "False",
                    BucketName = "",
                    TargetFileNameToDownload = "",
                };
                return installerData;
            }

        }

        public static string GetDataJsonPath(string rootPath) => Path.Combine(rootPath, "net6.0", "SETTING", "data.json");

        public static string GetRootFolder()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo directoryInfo = Directory.GetParent(baseDirectory)!.Parent!;
            return directoryInfo.FullName;
        }
        private static async void UpdateOperations(bool isUpdate, string bucketName, string targetFileToDownload)
        {
            //Console.WriteLine($"\n <<<UpdateOperations>>> Download");
            //Download
            Download(bucketName, targetFileToDownload);

            //Archive
            Archive(isUpdate);

            //Extract
            Extract(isUpdate);

            //Run
            await Run();
        }
        private static void RollBackApp() { }


        private static void Download(string bucketName, string targetFileToDownload)
        {
            //await Task.Delay(TimeSpan.FromSeconds(1));

            string downloadDirectory = FileManager.GetNewVersionFolderPath();
            if (Directory.Exists(downloadDirectory))
            {
                //Console.WriteLine("Directory.Exists");
                Directory.Delete(downloadDirectory, true);
            }
            //Console.WriteLine("CreateDirectory");
            Console.WriteLine("          <<<KNEXUS FIRESTORE INSTALLER>>>");
            Directory.CreateDirectory(downloadDirectory);

            string LocalDestinationFilename = FileManager.GetNewVersionZipPath();
            //Console.WriteLine($"\n <<<InitializeFirestoreDb>>> Download");
            InitializeFirestoreDb();
            //Console.WriteLine($"\n <<<StorageClient.Create()>>> Download");
            var storage = StorageClient.Create();

            // Download the zip folder with progress tracking
            Console.WriteLine("\nDownloading...\n");

            DownloadFiletWithProgress(storage, bucketName, targetFileToDownload, LocalDestinationFilename);
        }

        private static void Archive(bool isUpdate)
        {
            Console.WriteLine("\nArchiving...\n");

            string rootFolder = FileManager.GetRootFolder(); // "D:\\publish"
            string tempFolder = FileManager.GetTEMPPath();

            if (Directory.Exists(tempFolder))
            {
                Directory.Delete(tempFolder, true);
            }
            Directory.CreateDirectory(tempFolder);

            int numSticks = 50; // Number of sticks to represent the full progress

            Console.Write("");
            for (int i = 0; i < numSticks; i++)
            {
                Console.Write(".");
            }
            Console.Write("");

            long totalBytesToProcess = GetDirectorySize(new DirectoryInfo(rootFolder));
            long bytesProcessed = 0;

            bytesProcessed += MoveAllFiles(rootFolder, tempFolder, ref bytesProcessed, totalBytesToProcess, numSticks);

            string sourceFolderPath = FileManager.GetTEMPPath(); // Path.Combine(RootFolder, "ARCH", "TEMP");
            string destinationPath = FileManager.GetOldVersionZipPath(); // Path.Combine(RootFolder, "ARCH", "KnexusOld", "KnexusOld.zip");
            string zipDirectory = FileManager.GetOldVersionFolderPath(); // Path.Combine(RootFolder, "ARCH", "KnexusOld");

            if (isUpdate)
            {
                // Delete Arch Folder if exist
                if (Directory.Exists(zipDirectory))
                {
                    Directory.Delete(zipDirectory, true);
                }
                Directory.CreateDirectory(zipDirectory);

                ZipFile.CreateFromDirectory(sourceFolderPath, destinationPath);
            }
            else
            {
                if (!File.Exists(destinationPath))
                {
                    // Delete Arch Folder if exist
                    if (Directory.Exists(zipDirectory))
                    {
                        Directory.Delete(zipDirectory, true);
                    }
                    Directory.CreateDirectory(zipDirectory);

                    ZipFile.CreateFromDirectory(sourceFolderPath, destinationPath);
                }
            }

            // Update progress bar to 100% after ZIP archiving is complete
            Console.Write("\r");
            for (int i = 0; i < numSticks; i++)
            {
                Console.Write("|");
            }
            Console.Write(" 100%");

            Directory.Delete(tempFolder, true);

            Console.WriteLine();
        }
        private static void Extract(bool isUpdate)
        {
            Console.WriteLine("\nExtracting...\n");

            string zipFilePath = isUpdate ? FileManager.GetNewVersionZipPath() : FileManager.GetOldVersionZipPath();
            string destinationFolderPath = FileManager.GetRootFolder();

            try
            {
                if (File.Exists(zipFilePath))
                {
                    ExtractWithProgress(zipFilePath, destinationFolderPath);
                }
            }
            catch { }
        }


        private static void ExtractWithProgress(string zipFilePath, string destinationFolderPath)
        {
            int progressBarWidth = 50; // Width of the progress bar
            long totalBytesToExtract = new FileInfo(zipFilePath).Length;
            long bytesExtracted = 0;
            int lastPercentage = -1;

            using (ZipArchive zipArchive = ZipFile.OpenRead(zipFilePath))
            {
                foreach (ZipArchiveEntry entry in zipArchive.Entries)
                {
                    string entryDestinationPath = Path.Combine(destinationFolderPath, entry.FullName);
                    string entryDirectory = Path.GetDirectoryName(entryDestinationPath)!;

                    if (!Directory.Exists(entryDirectory))
                    {
                        Directory.CreateDirectory(entryDirectory);
                    }

                    if (!string.IsNullOrEmpty(entry.Name))
                    {
                        using (Stream entryStream = entry.Open())
                        using (FileStream fileStream = File.Create(entryDestinationPath))
                        {
                            byte[] buffer = new byte[4096];
                            int bytesRead;

                            while ((bytesRead = entryStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                fileStream.Write(buffer, 0, bytesRead);
                                bytesExtracted += bytesRead;

                                double progressPercentage = (double)bytesExtracted / totalBytesToExtract * 100;
                                int currentPercentage = (int)Math.Min(progressPercentage, 100); // Clamp to 0-100

                                // Print progress when percentage changes
                                if (currentPercentage != lastPercentage)
                                {
                                    int progressBarLength = (int)(progressBarWidth * progressPercentage / 100);
                                    Console.Write("\r" + new string('|', progressBarLength) + new string('.', progressBarWidth - progressBarLength) + " {0}%", currentPercentage,"\n");
                                    lastPercentage = currentPercentage;
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine();
        }


        private static async Task Run()
        {
            Console.WriteLine("\nRunning...\n");
            string KApp = FileManager.GetKnexusAppPath();                               // Path.Combine(RootFolder, "Knexus.exe");

            if (File.Exists(KApp))
            {
                Process _userAppProcess = new();
                var startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "C:\\WINDOWS\\System32\\cmd.exe";
                startInfo.Arguments = " /c start \"\" \"C:\\Users\\LENOVO\\source\\repos\\DriveFolderDownload\\DriveFolderDownload\\bin\\Debug\\Knexus.exe";
                //startInfo.Arguments = " /c start \"\" \"C:\\Users\\KOBIL\\Desktop\\verrsion-06-xx\\KnexusUpdater\\KnexusUpdater\\bin\\Debug\\Knexus.exe\"";
                _userAppProcess.StartInfo = startInfo;
                _userAppProcess.Start();
            }
            await Task.Delay(3000);
            Environment.Exit(0);
        }


        public static FirestoreDb InitializeFirestoreDb()
        {
            var filePath = GetFirestoreCredentialsFilePath();
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);
            return FirestoreDb.Create("iot-kefa");
        }

        private static string GetFirestoreCredentialsFilePath()
        {
            var dirPath = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.GetFullPath(Path.Combine(dirPath, "iot-kefa.json"));

            return filePath;
        }

        private static void DownloadFiletWithProgress(StorageClient storage, string bucketName, string targetFileToDownload, string downloadFilePath)
        {
            using (var outputFile = File.OpenWrite(downloadFilePath))
            {
                var objectStream = new MemoryStream();
                storage.DownloadObject(bucketName, targetFileToDownload, objectStream);
                objectStream.Position = 0;

                var buffer = new byte[4096];
                int bytesRead;
                long totalBytesRead = 0;
                long totalBytesToRead = objectStream.Length;
                int numSticks = 50; // Number of sticks to represent the full progress

                Console.Write("");
                for (int i = 0; i < numSticks; i++)
                {
                    Console.Write(".");
                }
                Console.Write("");

                while ((bytesRead = objectStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outputFile.Write(buffer, 0, bytesRead);
                    totalBytesRead += bytesRead;
                    long progress = totalBytesRead * numSticks / totalBytesToRead;

                    Console.Write("\r");
                    for (int i = 0; i < progress; i++)
                    {
                        Console.Write("|");
                    }
                    for (int i = (int)progress; i < numSticks; i++)
                    {
                        Console.Write(".");
                    }
                    Console.Write(" {0}%", progress * 100 / numSticks);
                }

                Console.WriteLine();
            }
        }

        private static long MoveAllFiles(string sourceDir, string destinationDir, ref long bytesProcessed, long totalBytesToProcess, int numSticks, bool recursive = true)
        {
            long bytesProcessedInThisMethod = 0;
            DirectoryInfo sourceDirectory = new(sourceDir);

            foreach (FileInfo file in sourceDirectory.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.MoveTo(targetFilePath, true);

                bytesProcessed += file.Length;
                bytesProcessedInThisMethod += file.Length;

                long progress = bytesProcessed * numSticks / totalBytesToProcess;

                Console.Write("\r[");
                for (int i = 0; i < progress; i++)
                {
                    Console.Write("|");
                }
                for (int i = (int)progress; i < numSticks; i++)
                {
                    Console.Write(".");
                }
                Console.Write("] {0}%", progress * 100 / numSticks);
            }

            if (recursive)
            {
                foreach (DirectoryInfo subDir in sourceDirectory.GetDirectories())
                {
                    if (subDir.Name.Equals("CEED") || subDir.Name.Equals("Videos"))
                    {
                        string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                        if (Directory.Exists(newDestinationDir))
                        {
                            Directory.Delete(newDestinationDir, true);
                        }
                        Directory.CreateDirectory(newDestinationDir);

                        bytesProcessedInThisMethod += MoveAllFiles(subDir.FullName, newDestinationDir, ref bytesProcessed, totalBytesToProcess, numSticks, true);

                        Directory.Delete(subDir.FullName, true);
                    }
                }
            }

            return bytesProcessedInThisMethod;
        }

        private static long GetDirectorySize(DirectoryInfo directory)
        {
            long size = 0;

            foreach (FileInfo fileInfo in directory.GetFiles())
            {
                size += fileInfo.Length;
            }

            foreach (DirectoryInfo subdirectory in directory.GetDirectories())
            {
                size += GetDirectorySize(subdirectory);
            }

            return size;
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
