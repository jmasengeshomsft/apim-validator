using APIM.Validation.Modoles;
using Microsoft.Azure.Management.ApiManagement.Models;
using System.Collections.Generic;

namespace APIM.Validation.Services
{
    public interface IAPIMValidator
    {
        PolicyException Validate(PolicyContract policy);
    }
}