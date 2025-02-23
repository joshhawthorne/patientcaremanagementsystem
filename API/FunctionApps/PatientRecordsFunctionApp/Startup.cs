using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Pcms.Api.Functions;
using PcmsApi.Core.Persistence;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Pcms.Api.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Register services here
            builder.Services.AddLogging();
            // Add other services as needed
            builder.Services.AddDbContext<PcmsDbContext>(options =>
                options.UseSqlite("Data Source=patientcare.db")); // Todo: Use configuration for the connectin string
        }
    }
}