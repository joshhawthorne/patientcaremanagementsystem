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
    /// Azure Function to retrieve patient records.
    /// </summary>
    /// <remarks>
    /// ToDo: Refactor to write to a target state RDBMS.
    /// </remarks>
    public class RetrievePatientRecords
    {
        private readonly PcmsDbContext _dbContext;
        private readonly ILogger<RetrievePatientRecords> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RetrievePatientRecords"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public RetrievePatientRecords(ILogger<RetrievePatientRecords> logger, PcmsDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Runs the function to retrieve patient records.
        /// </summary>
        /// <param name="req">The HTTP request.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [Function("RetrievePatientRecords")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            _logger.LogInformation("RetrievePatientRecords request received.");

            // Get query parameters for pagination
            int pageSize = int.TryParse(req.Query["pageSize"], out var ps) ? ps : 10;
            int currentPage = int.TryParse(req.Query["currentPage"], out var cp) ? cp : 1;

            // Calculate the number of records to skip
            int skip = (currentPage - 1) * pageSize;

            // Retrieve paginated records from the database
            var records = await _dbContext.Records
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new OkObjectResult(records);
        }
    }
}
