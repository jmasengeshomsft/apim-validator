using APIM.Validation.Modoles;
using Microsoft.Azure.Management.ApiManagement.Models;

namespace APIM.Validation.Services
{
    public class PolicyExistenceValidator : IAPIMValidator
    {
        public PolicyException Validate(PolicyContract policy)
        {
            var exception = new PolicyException
            {
                ExceptionMessage = "This API does not have a policy configuration"
            };

            return exception;
        }
    }
}