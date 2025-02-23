using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using PatientRecordsFunctionApp.Models;

namespace PatientRecordsFunctionApp.Functions
{
    public class CreatePatientRecord
    {
        private readonly ILogger<CreatePatientRecord> _logger;

        public CreatePatientRecord(ILogger<CreatePatientRecord> logger)
        {
            _logger = logger;
        }

        [Function("CreatePatientRecord")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("CreatePatientRecord request received.");

            // Read and deserialize the request body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Record record;
            try
            {
                record = JsonSerializer.Deserialize<Record>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Invalid JSON format.");
                return new BadRequestObjectResult("Invalid JSON format.");
            }

            // Validate the Record object
            if (record == null || record.Patient == null)
            {
                return new BadRequestObjectResult("Invalid record data.");
            }

            // Here you would typically save the record to a database

            return new OkObjectResult("Record created successfully.");
        }
    }
}
