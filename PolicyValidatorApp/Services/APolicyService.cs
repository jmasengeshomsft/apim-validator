using System.Collections.Generic;
using System.Threading.Tasks;
using APIM.Validation.Modoles;
using Microsoft.Azure.Management.ApiManagement.Models;

namespace APIM.Validation.Services
{
    public class APIPolicyService : IAPIPolicyService
    {
        public Task<PolicyContract> GetAPIPolicies(ApiEventGridData apiData)
        {
            throw new System.NotImplementedException();
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