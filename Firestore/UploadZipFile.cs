//TO UPLOAD PUBLISH FILE TO FIREBASE STORAGE
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
            //C:\Users\LENOVO\Desktop\KnexusFirebase\single-06.01-alter-Log-FluentModbus\Knexus\Knexus\bin\Release\net6.0-windows\win-x64\publish\publish.zip
            string filePath = "C:\\Users\\LENOVO\\Desktop\\KnexusFirebase\\single-06.01-alter-Log-FluentModbus\\Knexus\\Knexus\\bin\\Release\\net6.0-windows\\win-x64\\publish\\publish.zip";   //local zip file location
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
            return FirestoreDb.Create("***"); //*project name
        }

        private static string GetFirestoreCredentialsFilePath()
        {
            var dirPath = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.GetFullPath(Path.Combine(dirPath, "***")); //*.json file
            return filePath;
        }
    }
}
