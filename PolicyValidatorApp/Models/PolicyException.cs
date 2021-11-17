using System.Security;
using Microsoft.Azure.Management.ApiManagement.Models;

namespace APIM.Validation.Modoles
{
    public class PolicyException
    {
        public virtual string ExceptionMessage {get;set;}
        public PolicyContract Policy {get;set;}
        //public void SetMessage()
    }

    public class PolicyExceptionFlat
    {
        public string ExceptionMessage {get;set;}
        public string PolicyName {get;set;}
        public string PolicyValue {get;set;}
        public string PolicyFormat {get;set;}

        public PolicyExceptionFlat(PolicyException policy)
        {
            ExceptionMessage = policy.ExceptionMessage;
            PolicyFormat = policy.Policy?.Format;
            PolicyName = policy.Policy?.Name;
            if(policy.Policy != null)
            {
               PolicyValue =  SecurityElement.Escape(policy.Policy.Value.Substring(policy.Policy.Value.IndexOf("-->") + 3));
            }
        }
    }
}