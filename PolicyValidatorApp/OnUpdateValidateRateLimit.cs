// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.Management.ApiManagement.Models;
using Microsoft.Azure.Management.ApiManagement;
using Newtonsoft.Json;
using APIM.Validation.Modoles;

namespace APIM.Validation.Functions
{
    public static class OnUpdateValidateRateLimit
    {
        [FunctionName("OnUpdateValidateRateLimit")]
        public static async Task Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation($"The event data is: {eventGridEvent.Data.ToString()}");

            try
            {
                var eventData = JsonConvert.DeserializeObject<ApiEventGridData>(eventGridEvent.Data.ToString());
                var apiEventGridData = ApiEventGridData.CreateFromUrl(eventData.ResourceUri);
                var apiPolicies = await GetAPIPolicyAsync(apiEventGridData);
                log.LogInformation($"The API details are: {JsonConvert.SerializeObject(apiPolicies)}");
                
            }catch(Exception e)
            {
               log.LogInformation($"Unable to get the API. The error is: {e.Message}");
            }
        }

        public static async Task<PolicyContract> GetAPIPolicyAsync(ApiEventGridData data)
        {
            var accessToken = await GetAccessTokenAsync();
            var creds = new Microsoft.Rest.TokenCredentials(accessToken);

            var apimClient = new ApiManagementClient(creds);
            apimClient.SubscriptionId = data.Subscription;

            ///var api = await apimClient.Api.GetAsync("apim-rg", "jm-demo-apim", "nonehoeventstats");
    
            var apiPolicies = await apimClient.ApiPolicy.GetAsync(data.ResourceGroup, data.ApimServiceName, data.ApiName);

            return apiPolicies;
        }

        public static  async Task<string> GetAccessTokenAsync()
          {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://management.azure.com");

            return accessToken;
          }
    }
}
