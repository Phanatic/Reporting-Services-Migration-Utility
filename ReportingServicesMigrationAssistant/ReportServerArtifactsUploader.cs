
namespace ReportingServicesMigrationAssistant
{
    using System;
        using System.IO;
        using ReportingServicesSchema.ReportingServices2010;

    public class ReportServerArtifactsUploader : ReportServerMigrationAsisstantBase
    {
        public void Upload(string targetServerUri, string artifactsFolderPath)
        {
            bool isHttpsEndpoint = new Uri(targetServerUri).Scheme == Uri.UriSchemeHttps;
            ReportingService2010SoapClient reportingServicesClient = CreateClientProxy(targetServerUri, isHttpsEndpoint);

            var userHeader = new TrustedUserHeader();

            UploadReportDataSources(reportingServicesClient, userHeader, artifactsFolderPath);
            UploadSharedDataSets(reportingServicesClient, userHeader, artifactsFolderPath);
            UploadReports(reportingServicesClient, userHeader, artifactsFolderPath);
        }

        private void UploadSharedDataSets(ReportingService2010SoapClient reportingServicesClient, TrustedUserHeader userHeader, string artifactsFolderPath)
        {
            foreach (var dataSetDefinitionFile in Directory.GetFiles(artifactsFolderPath, String.Format("*{0}", DataSetFileExtension)))
            {
                string itemName = Path.GetFileNameWithoutExtension(dataSetDefinitionFile);
                byte[] itemDefinition = File.ReadAllBytes(dataSetDefinitionFile);
                CatalogItem dataSetItem = null;
                Warning[] warnings = null;
                Console.WriteLine("Uploaded shared data set '{0}'", itemName);
                reportingServicesClient.CreateCatalogItem(userHeader, "DataSet", itemName, "/", true, itemDefinition, null, out dataSetItem, out warnings);
            }
        }

        private void UploadReportDataSources(ReportingService2010SoapClient reportingServicesClient, TrustedUserHeader userHeader, string artifactsFolderPath)
        {
            foreach (var dataSourceDefinitionFile in Directory.GetFiles(artifactsFolderPath, String.Format("*{0}", DataSourceFileExtension)))
            {
                string itemName = Path.GetFileNameWithoutExtension(dataSourceDefinitionFile);
                CatalogItem dataSetItem = null;
                DataSourceDefinition definition = DataSourceDefinitionParser.Parse(File.OpenRead(dataSourceDefinitionFile));
                Console.WriteLine("Uploaded report data source '{0}'", itemName);
                reportingServicesClient.CreateDataSource(userHeader, itemName, "/", true, definition, null, out dataSetItem);
            }
        }

        private void UploadReports(ReportingService2010SoapClient reportingServicesClient, TrustedUserHeader userHeader, string artifactsFolderPath)
        {
            foreach (var reportDefinitionFile in Directory.GetFiles(artifactsFolderPath, String.Format("*{0}", ReportFileExtension)))
            {
                string itemName = Path.GetFileNameWithoutExtension(reportDefinitionFile);
                byte[] itemDefinition = File.ReadAllBytes(reportDefinitionFile);
                CatalogItem dataSetItem = null;
                Warning[] warnings = null;
                Console.WriteLine("Uploaded report '{0}'", itemName);
                reportingServicesClient.CreateCatalogItem(userHeader, "Report", itemName, "/", true, itemDefinition, null, out dataSetItem, out warnings);
            }
        }
    }
}
