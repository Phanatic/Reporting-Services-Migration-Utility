
namespace ReportingServicesMigrationAssistant
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using ReportingServicesSchema.ReportingServices2010;

    public class DataSourceDefinitionParser
    {
        public static DataSourceDefinition Parse(Stream itemDefinitionStream)
        {
            XDocument definitionDocument = XDocument.Load(itemDefinitionStream);
            DataSourceDefinition definition = new DataSourceDefinition()
            {
                ConnectString = definitionDocument.Root.Descendants("ConnectString").First().Value,
                Enabled = bool.Parse(definitionDocument.Root.Descendants("Enabled").First().Value),
                Extension = definitionDocument.Root.Descendants("Extension").First().Value,
                CredentialRetrieval = (CredentialRetrievalEnum)Enum.Parse(typeof(CredentialRetrievalEnum), definitionDocument.Root.Descendants("CredentialRetrieval").First().Value)
            };

            return definition;
        }
    }
}
