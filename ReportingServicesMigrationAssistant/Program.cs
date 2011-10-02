
namespace ReportingServicesMigrationAssistant
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("\r\n=============Report Server Migration Assistant=============\r\n");
                if (args.Length >= 3)
                {
                    string sourceServer = args[0];
                    string targetServer = args[1];
                    string tempFolder = args[2];

                    if (sourceServer.ToLower() == "upload")
                    {
                        UploadArtifactsToServer(targetServer, tempFolder);
                    }
                    else if (sourceServer.ToLower() == "download")
                    {
                        DownloadArtifactsFromServer(targetServer, tempFolder);
                    }
                    else
                    {
                        DownloadArtifactsFromServer(sourceServer, tempFolder);
                        UploadArtifactsToServer(targetServer, tempFolder);
                    }

                    Console.WriteLine("Migration complete");
                }
                else
                {
                    DisplayUsage();
                }
            }
            catch (Exception failedException)
            {
                Console.WriteLine(failedException.GetBaseException().ToString());
            }
        }

        private static void DownloadArtifactsFromServer(string sourceServer, string tempFolder)
        {
            ReportServerArtifactsDownloader downloader = new ReportServerArtifactsDownloader();
            downloader.Download(sourceServer, tempFolder);
            Console.WriteLine("Downloaded Artifacts");
        }

        private static void UploadArtifactsToServer(string targetServer, string tempFolder)
        {
            ReportServerArtifactsUploader uploader = new ReportServerArtifactsUploader();
            uploader.Upload(targetServer, tempFolder);
            Console.WriteLine("Uploaded Artifacts");
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("      ReportServerMigrationAssistant.exe <source server web service uri> <target server web service uri> <report artifacts folder path>");

            Console.WriteLine("e.g. 1) Migration ");
            Console.WriteLine(" ReportServerMigrationAssistant.exe \"http://phanaticsl4/ReportServerDENALI/ReportService2010.asmx\" \"http://barcelona-demo2/ReportServer/ReportService2010.asmx\" \"c:\\temp\"");

            Console.WriteLine("e.g. 2) Download Artifacts");
            Console.WriteLine(" ReportServerMigrationAssistant.exe download \"http://phanaticsl4/ReportServerDENALI/ReportService2010.asmx\" \"c:\\temp\"");

            Console.WriteLine("e.g. 2) Upload Artifacts ");
            Console.WriteLine(" ReportServerMigrationAssistant.exe upload \"http://barcelona-demo2/ReportServer/ReportService2010.asmx\" \"c:\\temp\"");
        }
    }
}
