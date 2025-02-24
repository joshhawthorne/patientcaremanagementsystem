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
    /// Azure Function to soft delete a patient record.
    /// </summary>
    /// <remarks>
    /// ToDo: Refactor to write to a target state RDBMS.
    /// ToDo: Refactor to have the current user passed in.
    /// </remarks>
    public class DeletePatientRecord
    {
        private readonly PcmsDbContext _dbContext;
        private readonly ILogger<DeletePatientRecord> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeletePatientRecord"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public DeletePatientRecord(ILogger<DeletePatientRecord> logger, PcmsDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Runs the function to soft delete a patient record.
        /// </summary>
        /// <param name="req">The HTTP request.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [Function("DeletePatientRecord")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("DeletePatientRecord request received.");

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

            // Set the IsActive property to false
            existingRecord.IsActive = false;
            existingRecord.LastUpdatedDate = DateTime.UtcNow;
//            existingRecord.LastUpdatedBy = currentUser.Name; // Todo: Need to get the current user

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

            return new OkObjectResult("Record deleted successfully.");
        }
    }
}
