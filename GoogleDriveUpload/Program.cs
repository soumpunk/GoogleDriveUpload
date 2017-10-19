using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace GoogleDriveUpload
{
    public class Program
    {
        static string[] scopes = new string[] { DriveService.Scope.Drive,
                           DriveService.Scope.DriveAppdata,
                           DriveService.Scope.DrivePhotosReadonly,
                           DriveService.Scope.DriveFile,
                           DriveService.Scope.DriveMetadataReadonly,
                           DriveService.Scope.DriveReadonly,
                           DriveService.Scope.DriveScripts };

        static string ApplicationName = "Drive API .NET Quickstart";

        public static void Main(string[] args)
        {
            string[] scopes = new string[] { DriveService.Scope.Drive,
                                            DriveService.Scope.DriveMetadata,
                                            DriveService.Scope.DriveAppdata,
                                            DriveService.Scope.DrivePhotosReadonly,
                                            DriveService.Scope.DriveFile,
                                            DriveService.Scope.DriveMetadataReadonly,
                                            DriveService.Scope.DriveReadonly,
                                            DriveService.Scope.DriveScripts };

            string ApplicationName = "Adroit_Web";

            UserCredential credential;

            using (var stream =
                new FileStream("client_secret_1.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
            Google.Apis.Drive.v3.Data.File file = CreateFolder(service);
            CreateFile(service, file.Id);
            //uploadFile(service);
        }

        //private static void uploadFile(DriveService driverService)
        //{
        //    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
        //    {
        //        Name = "10",
        //        MimeType = "application/vnd.google-apps.spreadsheet"
        //    };
        //    FilesResource.CreateMediaUpload request;
        //    using (var stream = new System.IO.FileStream(@"C:\Users\soumik\Desktop\10.csv", System.IO.FileMode.Open))
        //    {
        //        request = driverService.Files.Create(fileMetadata, stream, "text/csv");
        //        request.Fields = "id";
        //        request.Upload();
        //    }
        //    var file = request.ResponseBody;
        //    Console.WriteLine("File ID: " + file.Id);
        //}

        //Insert new folder
        private static Google.Apis.Drive.v3.Data.File CreateFolder(DriveService service)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = "Invoices",
                MimeType = "application/vnd.google-apps.folder"
            };
            var request = service.Files.Create(fileMetadata);
            request.Fields = "id";
            var file = request.Execute();
            Console.WriteLine("Folder ID: " + file.Id);
            return file;
        }

        //Insert a new file
        private static void CreateFile(DriveService driverService,string folderId)
        {
            //var folderId = "0BwwA4oUTeiV1TGRPeTVjaWRDY1E";
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = "error.jpg",
                Parents = new List<string>
                {
                    folderId
                }
            };
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(@"C:\Users\soumik\Desktop/error.jpg",System.IO.FileMode.Open))
            {
                request = driverService.Files.Create(
                    fileMetadata, stream, "image/jpeg");
                request.Fields = "id";
                request.Upload();
            }
            var file = request.ResponseBody;
            Console.WriteLine("File ID: " + file.Id);
        }
    }
}
