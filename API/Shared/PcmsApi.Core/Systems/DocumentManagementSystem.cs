namespace PcmsApi.Core.Systems;

using PcmsApi.Core.Models;

/// <summary>
/// Provides functionality for managing documents and attachments within the patient care management system.
/// </summary>
/// <remarks>
/// This class is a facade to the Document Management System.
/// ToDo: Replace the writing to Azure Blob Storage with a API calls to the Document Management System.
/// </remarks>/// 
public class DocumentManagementSystem
{
    /// <summary>
    /// Creates an attachment and associates it with a specified record.
    /// </summary>
    /// <param name="record">The record to which the attachment will be added.</param>
    /// <param name="attachment">The attachment to be created.</param>
    /// <param name="createdBy">The user who is creating the attachment.</param>
    /// <exception cref="ArgumentNullException">Thrown when the record or attachment is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the createdBy parameter is null or empty.</exception>
    public void CreateAttachment(Record record, Attachment attachment, string createdBy)
    {
        if (record == null)
        {
            throw new ArgumentNullException(nameof(record));
        }

        if (attachment == null)
        {
            throw new ArgumentNullException(nameof(attachment));
        }

        if (string.IsNullOrWhiteSpace(createdBy))
        {
            throw new ArgumentException("Created by cannot be null or empty", nameof(createdBy));
        }

        // Set the status, created by, and created date for the attachment
        attachment.Status = AttachmentStatus.Submitted; // Submitted is the beginning of the workflow for uploading an attachment.

        // There is an assumption the user requesting the document to be uploaded is authorized to add attachments to the given record. This should have been validated at the service boundary context.
        attachment.CreatedBy = createdBy; 

        attachment.UploadedDate = DateTime.Now;

        // Add the attachment to the record's attachments collection
        if (record.Attachments == null)
        {
            record.Attachments = new List<Attachment>();
        }

        // ToDo: Write the attachment to Azure Blob Storage (for today).

        // Intentionally not adding the attachment to the record, as it could be misleading if the attachment fails to write to the Document Management System.
        // record.Attachments.Add(attachment);
    }
}
