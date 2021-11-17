namespace APIM.Validation.Modoles
{
    public class MissingPolicyException : PolicyException
    {
       public override string ExceptionMessage {get;set;} = "This API does not have policy configuration";
    }
}