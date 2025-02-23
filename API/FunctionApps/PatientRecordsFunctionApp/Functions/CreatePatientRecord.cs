using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using PcmsApi.Core.Models;
using PcmsApi.Core.Persistence;

namespace Pcms.Api.Functions
{
    /// <summary>
    /// Azure Function to create a patient record.
    /// </summary>
    /// <remarks>
    /// ToDo: Refactor to write to a target state RDBMS.
    /// </remarks>
    public class CreatePatientRecord
    {
        private const string PatientCareDatabaseConnectionString = "Data Source=patientcare.db";

        private readonly PcmsDbContext _dbContext;
        private readonly ILogger<CreatePatientRecord> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreatePatientRecord"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public CreatePatientRecord(ILogger<CreatePatientRecord> logger, PcmsDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Runs the function to create a patient record.
        /// </summary>
        /// <param name="req">The HTTP request.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
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

            await _dbContext.Records.AddAsync(record);
            await _dbContext.SaveChangesAsync();

            return new OkObjectResult("Record created successfully.");
        }
    }

}
