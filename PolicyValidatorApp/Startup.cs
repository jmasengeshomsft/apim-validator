using System;
using System.Threading.Tasks;
using APIM.Validation.Services;
using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.Management.ApiManagement;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(APIM.Validation.Functions.Startup))]
namespace APIM.Validation.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {  
            builder.Services.AddSingleton<ApiManagementClient>(InitializeAPIManagementClient().GetAwaiter().GetResult());
            builder.Services.AddSingleton<Azure.Messaging.EventGrid.EventGridPublisherClient>(InitializeEventGridExceptionTopicClient());
            builder.Services.AddSingleton<IAPIPolicyService,APIPolicyService>();
        }

        public async Task<ApiManagementClient> InitializeAPIManagementClient()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://management.azure.com");

            var creds = new Microsoft.Rest.TokenCredentials(accessToken);

            var apimClient = new ApiManagementClient(creds);

            return apimClient;
        }

        public Azure.Messaging.EventGrid.EventGridPublisherClient InitializeEventGridExceptionTopicClient()
        {
            var creds = new DefaultAzureCredential();
            var endPointUrl = System.Environment.GetEnvironmentVariable("APIMEXCEPTIONS_TOPIC_URL");
            var eventGridClient = new Azure.Messaging.EventGrid.EventGridPublisherClient(new Uri(endPointUrl), creds);
            return eventGridClient;
        }
    }
}