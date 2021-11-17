// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Azure.Management.ApiManagement;
using Newtonsoft.Json;
using APIM.Validation.Modoles;
using APIM.Validation.Services;
using System.Collections.Generic;

namespace APIM.Validation.Functions
{
    public class OnUpdateValidateRateLimit
    {
        private ILogger _log;
        public readonly IAPIPolicyService _apiPolicyService;
        public readonly ApiManagementClient _apiManagementClient;
        public OnUpdateValidateRateLimit(ApiManagementClient apiManagementClient, IAPIPolicyService policyService)
        {
            _apiManagementClient = apiManagementClient;
            _apiPolicyService = policyService;
           // _log = logger;
        }

        [FunctionName("OnUpdateValidateRateLimit")]
        public  async Task Run([EventGridTrigger]EventGridEvent eventGridEvent, 
                               [EventGrid(TopicEndpointUri = "MyEventGridTopicUriSetting", TopicKeySetting = "MyEventGridTopicKeySetting")]IAsyncCollector<EventGridEvent> outputEvents, ILogger log)
        {
            _log = log;
            _log.LogInformation($"The event data is: {eventGridEvent.Data.ToString()}");

            var validators = new List<IAPIMValidator>();
            var exceptions = new List<PolicyException>();
            var apiEventGridData = new ApiEventGridData();

            try
            {
                var eventData = JsonConvert.DeserializeObject<ApiEventGridData>(eventGridEvent.Data.ToString());
                apiEventGridData = ApiEventGridData.CreateFromUrl(eventData.ResourceUri);
                
                var apiPolicy = await _apiPolicyService.GetAPIPolicies(apiEventGridData);

                if(apiPolicy == null)
                {
                    exceptions.Add(new MissingPolicyException());
                    //return;
                }

                _log.LogInformation($"The API policy id: {JsonConvert.SerializeObject(apiPolicy)}");

                validators.Add(new RateLimitPolicyValidator());

                var validatedExceptions = _apiPolicyService.GetExceptions(apiPolicy, validators);
                exceptions.AddRange(validatedExceptions);

            }catch(Exception e)
            {
               _log.LogInformation($"Unable to get the API. The error is: {e.Message}");
            }

            foreach(var exception in exceptions)
            {
                var myEvent = new EventGridEvent(apiEventGridData.ApiName +"-" + exception.ExceptionMessage,$"API POLICY EXCEPTION ({apiEventGridData.ApimServiceName}) -{eventGridEvent.Subject} : { exception.ExceptionMessage}", new PolicyExceptionFlat(exception), "Apim-Policy-Exception", DateTime.UtcNow, "1.0"); //, "subject-name", "event-data", "event-type", DateTime.UtcNow, "1.0");
                await outputEvents.AddAsync(myEvent);
                 _log.LogInformation($"Event for exception: {exception.ExceptionMessage} was created");
            }

           _log.LogInformation($"Found Exceptions are: {JsonConvert.SerializeObject(exceptions)}");
        }
    }
}
