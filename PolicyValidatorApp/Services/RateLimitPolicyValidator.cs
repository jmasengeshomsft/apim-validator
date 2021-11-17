using APIM.Validation.Modoles;
using Microsoft.Azure.Management.ApiManagement.Models;

namespace APIM.Validation.Services
{
    public class RateLimitPolicyValidator : IAPIMValidator
    {
        public PolicyException Validate(PolicyContract policy)
        {
            if(!(policy.Value.Contains("rate-limit-by-key") || policy.Value.Contains("quota-by-key")))
            {
               var exception = new PolicyException
                {
                    ExceptionMessage = "This API does not have a rate limit or quota limit by key",
                    Policy = policy
                };

               return exception;
            }
            return null;
        }
    }
}