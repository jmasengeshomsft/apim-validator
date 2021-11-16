using APIM.Validation.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(APIM.Validation.Functions.Startup))]
namespace APIM.Validation.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {  
            builder.Services.AddSingleton<IAPIPolicyService,APIPolicyService>();
        }
    }
}