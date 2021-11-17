using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIM.Validation.Modoles;
using Microsoft.Azure.Management.ApiManagement;
using Microsoft.Azure.Management.ApiManagement.Models;

namespace APIM.Validation.Services
{
    public class APIPolicyService : IAPIPolicyService
    {
        private readonly ApiManagementClient _apiManagementClient;
 

        public APIPolicyService(ApiManagementClient apiManagementClient)
        {
            _apiManagementClient = apiManagementClient;
        }
        public async Task<PolicyContract> GetAPIPolicies(ApiEventGridData apiData)
        {
            try
            {
                _apiManagementClient.SubscriptionId = apiData.Subscription;

                ///var api = await apimClient.Api.GetAsync("apim-rg", "jm-demo-apim", "nonehoeventstats");
        
                var apiPolicy = await _apiManagementClient.ApiPolicy.GetAsync(apiData.ResourceGroup, apiData.ApimServiceName, apiData.ApiName);

                return apiPolicy;

            }catch(Exception ex)
            {
               Console.WriteLine($"{apiData.ResourceUri} does not have a policy configuration");
               return null;
            }  
        }

        public List<PolicyException> GetExceptions(PolicyContract policy, List<IAPIMValidator> validators)
        {
            var exceptions = new List<PolicyException> ();
            foreach(var validator in validators)
            {
                var exception = validator.Validate(policy);
                if(exception != null)
                {
                    exceptions.Add(exception);
                }
            }          
           return exceptions;
        }
    }
}