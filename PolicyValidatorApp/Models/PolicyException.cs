using Microsoft.Azure.Management.ApiManagement.Models;

namespace APIM.Validation.Modoles
{
    public class PolicyException
    {
        public virtual string ExceptionMessage {get;set;}
        public PolicyContract Policy {get;set;}
        //public void SetMessage()
    }
}