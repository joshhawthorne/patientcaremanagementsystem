using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using PcmsApi.Core.Models;
using PcmsApi.Core.Persistence;
using PcmsApi.Core.Systems;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PcmsApi.Functions
{
    /// <summary>
    /// Azure Function to upload an attachment.
    /// </summary>
    /// <remarks>
    /// ToDo: Refactor to write to a target state RDBMS.
    /// </remarks>
    public class UploadAttachment
    {
        private readonly PcmsDbContext _dbContext;
        private readonly ILogger<UploadAttachment> _logger;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly DocumentManagementSystem _documentManagementSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadAttachment"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="blobServiceClient">The BLOB service client.</param>
        /// <param name="documentManagementSystem">The document management system.</param>
        public UploadAttachment(ILogger<UploadAttachment> logger, PcmsDbContext dbContext, BlobServiceClient blobServiceClient, DocumentManagementSystem documentManagementSystem)
        {
            _dbContext = dbContext;
            _logger = logger;
            _blobServiceClient = blobServiceClient;
            _documentManagementSystem = documentManagementSystem;
        }

        /// <summary>
        /// Runs the function to upload an attachment.
        /// </summary>
        /// <param name="req">The HTTP request.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [Function("UploadAttachment")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("UploadAttachment request received.");

            // Get the record ID from the query string
            string recordIdString = req.Query["recordId"];
            if (string.IsNullOrEmpty(recordIdString) || !Guid.TryParse(recordIdString, out Guid recordId))
            {
                return new BadRequestObjectResult("Invalid or missing recordId parameter.");
            }

            // Get the attachment type from the query string
            string attachmentTypeString = req.Query["attachmentType"];
            if (string.IsNullOrEmpty(attachmentTypeString) || !Enum.TryParse(attachmentTypeString, out AttachmentType attachmentType))
            {
                return new BadRequestObjectResult("Invalid or missing attachmentType parameter.");
            }

            // Find the existing record
            var existingRecord = await _dbContext.Records.FindAsync(recordId);
            if (existingRecord == null)
            {
                return new NotFoundObjectResult("Record not found.");
            }

            // Get the file from the request
            var formCollection = await req.ReadFormAsync();
            var file = formCollection.Files.GetFile("file");
            if (file == null || file.Length == 0)
            {
                return new BadRequestObjectResult("No file uploaded.");
            }

            // Upload the file to BLOB storage
            var containerClient = _blobServiceClient.GetBlobContainerClient("attachments");
            var blobClient = containerClient.GetBlobClient(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
            }

            // Add the attachment to the record
            var attachment = new Attachment(
                id: Guid.NewGuid(),
                fileName: file.FileName,
                displayName: null,
                url: blobClient.Uri.ToString(),
                type: attachmentType,
                status: AttachmentStatus.Submitted,
                createdBy: null,
                createdDate: DateTime.UtcNow,
                lastUpdatedBy: null,
                lastUpdatedDate: DateTime.UtcNow,
                contentType: file.ContentType,
                uploadedDate: DateTime.UtcNow
            );

            if (existingRecord.Attachments == null)
            {
                existingRecord.Attachments = new List<Attachment>();
            }
            existingRecord.Attachments.Add(attachment);

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

            // Create the attachment in the DocumentManagementSystem
            _documentManagementSystem.CreateAttachment(existingRecord, attachment, string.Empty); // ToDo: Add the current user, and make the call async

            return new OkObjectResult("Attachment uploaded successfully.");
        }
    }
}
