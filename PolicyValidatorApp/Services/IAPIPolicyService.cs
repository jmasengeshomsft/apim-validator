using System.Collections.Generic;
using System.Threading.Tasks;
using APIM.Validation.Modoles;
using Microsoft.Azure.Management.ApiManagement.Models;

namespace APIM.Validation.Services
{
    public interface IAPIPolicyService
    {
        List<PolicyException> GetExceptions(PolicyContract policy, List<IAPIMValidator> validators);
        Task<PolicyContract> GetAPIPolicies(ApiEventGridData apiData);
    }
}