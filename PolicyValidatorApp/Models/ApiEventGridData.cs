using Newtonsoft.Json;

namespace APIM.Validation.Modoles
{
    public class ApiEventGridData
    {
        public string Subscription {get;set;}
        public string ResourceGroup {get;set;}
        public string ApimServiceName {get;set;}
        public string ApiName {get;set;}
        public string Revision {get;set;}

        [JsonProperty("resourceUri")]
        public string ResourceUri {get;set;}

        public ApiEventGridData()
        {
            
        }

        public ApiEventGridData(string resourceUrl)
        {
            ResourceUri = resourceUrl;
        }

        public static ApiEventGridData CreateFromUrl(string url)
        {
           var api = new ApiEventGridData(url);

           var urlElements = url.Split("/");
           api.Subscription = urlElements[2];
           api.ResourceGroup = urlElements[4];
           api.ApimServiceName = urlElements[8];
           api.ApiName = urlElements[10].Split(";")[0];
           api.Revision = urlElements[10].Split(";")[1];       
           return api;
        }
    }
}