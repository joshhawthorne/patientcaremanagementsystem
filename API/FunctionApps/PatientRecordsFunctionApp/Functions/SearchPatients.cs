using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PcmsApi.Core.Models;
using PcmsApi.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace PcmsApi.Functions
{
    /// <summary>
    /// Azure Function to search patients.
    /// </summary>
    /// <remarks>
    /// ToDo: Refactor to write to a target state RDBMS.
    /// ToDo: Make search more robust. Possibly utilize Azure Cognitive Search. 
    /// </remarks>
    public class SearchPatients
    {
        private readonly PcmsDbContext _dbContext;
        private readonly ILogger<SearchPatients> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchPatients"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public SearchPatients(ILogger<SearchPatients> logger, PcmsDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Runs the function to search patients.
        /// </summary>
        /// <param name="req">The HTTP request.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [Function("SearchPatients")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            _logger.LogInformation("SearchPatients request received.");

            // Get query parameters for pagination
            int pageSize = int.TryParse(req.Query["pageSize"], out var ps) ? ps : 10;
            int currentPage = int.TryParse(req.Query["currentPage"], out var cp) ? cp : 1;

            // Get the filter parameters from the query string
            string lastName = req.Query["lastName"];
            string medicalCondition = req.Query["medicalCondition"];
            string attachmentTypeString = req.Query["attachmentType"];
            AttachmentType? attachmentType = null;
            if (!string.IsNullOrEmpty(attachmentTypeString) && Enum.TryParse(attachmentTypeString, out AttachmentType at))
            {
                attachmentType = at;
            }

            // Calculate the number of records to skip
            int skip = (currentPage - 1) * pageSize;

            // Search paginated patients from the database
            var patientsQuery = _dbContext.Patients.AsQueryable();

            if (!string.IsNullOrEmpty(lastName))
            {
                patientsQuery = patientsQuery.Where(p => p.LastName.Contains(lastName));
            }

            if (!string.IsNullOrEmpty(medicalCondition))
            {
                patientsQuery = patientsQuery.Where(p => p.MedicalConditions.Contains(medicalCondition));
            }

            if (attachmentType.HasValue)
            {
                patientsQuery = patientsQuery.Where(p => p.Records.Any(r => r.Attachments.Any(a => a.Type == attachmentType)));
            }

            var patients = await patientsQuery
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new OkObjectResult(patients);
        }
    }
}
