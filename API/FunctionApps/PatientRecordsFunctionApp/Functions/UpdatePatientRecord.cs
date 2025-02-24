using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using PcmsApi.Core.Models;
using PcmsApi.Core.Persistence;

namespace PcmsApi.Functions
{
    /// <summary>
    /// Azure Function to update a patient record.
    /// </summary>
    /// <remarks>
    /// ToDo: Refactor to write to a target state RDBMS.
    /// </remarks>
    public class UpdatePatientRecord
    {
        private readonly PcmsDbContext _dbContext;
        private readonly ILogger<UpdatePatientRecord> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePatientRecord"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public UpdatePatientRecord(ILogger<UpdatePatientRecord> logger, PcmsDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Runs the function to update a patient record.
        /// </summary>
        /// <param name="req">The HTTP request.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [Function("UpdatePatientRecord")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("UpdatePatientRecord request received.");

            // Read and deserialize the request body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Record? record;
            try
            {
                record = JsonSerializer.Deserialize<Record>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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

            // Find the existing record
            var existingRecord = await _dbContext.Records.FindAsync(record.Id);
            if (existingRecord == null)
            {
                return new NotFoundObjectResult("Record not found.");
            }

            // Update the existing record's properties
            existingRecord.Description = record.Description;
            existingRecord.CreatedBy = record.CreatedBy;
            existingRecord.CreatedDate = record.CreatedDate;
            existingRecord.LastUpdatedBy = record.LastUpdatedBy;
            existingRecord.LastUpdatedDate = record.LastUpdatedDate;
            existingRecord.Patient = record.Patient;
            existingRecord.Attachments = record.Attachments;

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

            return new OkObjectResult("Record updated successfully.");
        }
    }
}
