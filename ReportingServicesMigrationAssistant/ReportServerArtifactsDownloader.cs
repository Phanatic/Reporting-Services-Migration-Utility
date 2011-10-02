
namespace ReportingServicesMigrationAssistant
{
    using System;
    using System.IO;
    using System.Linq;
    using ReportingServicesSchema.ReportingServices2010;

    public class ReportServerArtifactsDownloader : ReportServerMigrationAsisstantBase
    {
        private string outputFolderPath;

        public void Download(string targetServerUri, string artifactsFolderPath)
        {
            bool isHttpsEndpoint = new Uri(targetServerUri).Scheme == Uri.UriSchemeHttps;
            outputFolderPath = artifactsFolderPath;
            ReportingService2010SoapClient reportingServicesClient = CreateClientProxy(targetServerUri, isHttpsEndpoint);

            var userHeader = new TrustedUserHeader();
            CatalogItem[] items = null;
            reportingServicesClient.ListChildren(userHeader, "/", true, out items);
            DownloadReportDataSources(reportingServicesClient, userHeader, items);
            DownloadSharedDataSets(reportingServicesClient, userHeader, items);
            DownloadReports(reportingServicesClient, userHeader, items);
        }

        private void DownloadSharedDataSets(ReportingService2010SoapClient reportingServicesClient, TrustedUserHeader userHeader, CatalogItem[] items)
        {
            foreach (var dataSetDefinition in items.Where(item => item.TypeName == "DataSet"))
            {
                byte[] itemDefinition = null;
                string outputFilePath = Path.Combine(outputFolderPath, dataSetDefinition.Name + DataSetFileExtension);
                reportingServicesClient.GetItemDefinition(userHeader, dataSetDefinition.Path, out itemDefinition);
                string reportContents = new StreamReader(new MemoryStream(itemDefinition)).ReadToEnd();
                Console.WriteLine("Downloaded shared data set '{0}' into file '{1}'", dataSetDefinition.Name, outputFilePath);
                File.WriteAllText(outputFilePath, reportContents);
            }
        }

        private void DownloadReportDataSources(ReportingService2010SoapClient reportingServicesClient, TrustedUserHeader userHeader, CatalogItem[] items)
        {
            foreach (var dataSourceDefinition in items.Where(item => item.TypeName == "DataSource"))
            {
                byte[] itemDefinition = null;
                reportingServicesClient.GetItemDefinition(userHeader, dataSourceDefinition.Path, out itemDefinition);
                string reportContents = new StreamReader(new MemoryStream(itemDefinition)).ReadToEnd();
                string outputFilePath = Path.Combine(outputFolderPath, dataSourceDefinition.Name + DataSourceFileExtension);
                Console.WriteLine("Downloaded report data source '{0}' into file '{1}'", dataSourceDefinition.Name, outputFilePath);
                File.WriteAllText(outputFilePath, reportContents);
            }
        }

        private void DownloadReports(ReportingService2010SoapClient reportingServicesClient, TrustedUserHeader userHeader, CatalogItem[] items)
        {
            foreach (var reportDefinition in items.Where(item => item.TypeName == "Report"))
            {
                byte[] itemDefinition = null;
                reportingServicesClient.GetItemDefinition(userHeader, reportDefinition.Path, out itemDefinition);
                string reportContents = new StreamReader(new MemoryStream(itemDefinition)).ReadToEnd();
                string outputFilePath = Path.Combine(outputFolderPath, reportDefinition.Name + ReportFileExtension);
                Console.WriteLine("Downloaded report '{0}' into file '{1}'", reportDefinition.Name, outputFilePath);
                File.WriteAllText(outputFilePath, reportContents);
            }
        }
    }
}
