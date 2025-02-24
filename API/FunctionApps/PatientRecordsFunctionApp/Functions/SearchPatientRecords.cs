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
    /// Azure Function to search patient records.
    /// </summary>
    /// <remarks>
    /// ToDo: Refactor to write to a target state RDBMS.
    /// ToDo: Make search more robust. Possibly add a Title field to the Record model. And/or add other fields into the search query.
    /// </remarks>
    public class SearchPatientRecords
    {
        private readonly PcmsDbContext _dbContext;
        private readonly ILogger<SearchPatientRecords> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchPatientRecords"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public SearchPatientRecords(ILogger<SearchPatientRecords> logger, PcmsDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Runs the function to search patient records.
        /// </summary>
        /// <param name="req">The HTTP request.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [Function("SearchPatientRecords")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            _logger.LogInformation("SearchPatientRecords request received.");

            // Get query parameters for pagination
            int pageSize = int.TryParse(req.Query["pageSize"], out var ps) ? ps : 10;
            int currentPage = int.TryParse(req.Query["currentPage"], out var cp) ? cp : 1;

            // Get the keyword parameter from the query string
            string keyword = req.Query["keyword"];

            // Calculate the number of records to skip
            int skip = (currentPage - 1) * pageSize;

            // Search paginated records from the database
            var recordsQuery = _dbContext.Records.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                recordsQuery = recordsQuery.Where(r => r.Description.Contains(keyword));
            }

            var records = await recordsQuery
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new OkObjectResult(records);
        }
    }
}
