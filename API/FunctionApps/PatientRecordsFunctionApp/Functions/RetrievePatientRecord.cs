using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using PcmsApi.Core.Models;
using PcmsApi.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PcmsApi.Functions
{
    /// <summary>
    /// Azure Function to retrieve a single patient record.
    /// </summary>
    /// <remarks>
    /// ToDo: Refactor to write to a target state RDBMS.
    /// </remarks>
    public class RetrievePatientRecord
    {
        private readonly PcmsDbContext _dbContext;
        private readonly ILogger<RetrievePatientRecord> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RetrievePatientRecord"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public RetrievePatientRecord(ILogger<RetrievePatientRecord> logger, PcmsDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Runs the function to retrieve a single patient record.
        /// </summary>
        /// <param name="req">The HTTP request.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [Function("RetrievePatientRecord")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            _logger.LogInformation("RetrievePatientRecord request received.");

            // Get the id parameter from the query string
            string id = req.Query["id"];
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out Guid recordId))
            {
                return new BadRequestObjectResult("Invalid or missing id parameter.");
            }

            // Retrieve the record from the database
            var record = await _dbContext.Records.FindAsync(recordId);
            if (record == null)
            {
                return new NotFoundObjectResult("Record not found.");
            }

            return new OkObjectResult(record);
        }
    }
}
