using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MCG.PatientRecordsFunctionApp
{
    public class CreatePatientRecord
    {
        private readonly ILogger<CreatePatientRecord> _logger;

        public CreatePatientRecord(ILogger<CreatePatientRecord> logger)
        {
            _logger = logger;
        }

        [Function("CreatePatientRecord")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
