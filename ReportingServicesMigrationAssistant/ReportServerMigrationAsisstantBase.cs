
namespace ReportingServicesMigrationAssistant
{
    using System.Net;
    using System.Security.Principal;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Security;
    using System.Xml;
    using ReportingServicesSchema.ReportingServices2010;

    public abstract class ReportServerMigrationAsisstantBase
    {
        protected const string ReportFileExtension = ".rdl";
        protected const string DataSourceFileExtension = ".rsds";
        protected const string DataSetFileExtension = ".rds";

        protected ReportingService2010SoapClient CreateClientProxy(string targetServerUri, bool isHttpsEndpoint)
        {
            Binding secureBinding = new BasicHttpBinding()
            {
                Security = new BasicHttpSecurity()
                {
                    Mode = isHttpsEndpoint ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.TransportCredentialOnly,
                    Transport = new HttpTransportSecurity()
                    {
                        ClientCredentialType = HttpClientCredentialType.Ntlm,
                        ProxyCredentialType = HttpProxyCredentialType.Ntlm
                    },
                    Message = new BasicHttpMessageSecurity()
                    {
                        AlgorithmSuite = SecurityAlgorithmSuite.Default,
                        ClientCredentialType = BasicHttpMessageCredentialType.UserName
                    }
                },
                ReaderQuotas = new XmlDictionaryReaderQuotas()
                {
                    MaxDepth = int.MaxValue,
                    MaxArrayLength = int.MaxValue,
                    MaxBytesPerRead = 4096,
                    MaxNameTableCharCount = int.MaxValue,
                    MaxStringContentLength = int.MaxValue
                },
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize = int.MaxValue
            };

            EndpointAddress endpointAddress = new EndpointAddress(targetServerUri);
            ReportingService2010SoapClient reportingServicesClient = new ReportingService2010SoapClient(secureBinding, endpointAddress);
            reportingServicesClient.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
            reportingServicesClient.ClientCredentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
            return reportingServicesClient;
        }
    }
}
