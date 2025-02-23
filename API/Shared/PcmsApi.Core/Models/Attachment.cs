using System.ComponentModel.DataAnnotations;

namespace PcmsApi.Core.Models;

public class Attachment
{
    // Unique identifier for the attachment.
    [Key]
    public Guid Id { get; set; }

    // The name of the file (e.g., "scan1.jpg", "report.pdf").
    [Required]
    public string FileName { get; set; }

    // Display name for the file (e.g., "Scan of X-ray", "Patient Report").
    public string? DisplayName { get; set; }

    // MIME type of the file (e.g., "image/jpeg", "application/pdf").
    public string? ContentType { get; set; }

    // The type of the attachment.
    [Required]
    public AttachmentType Type { get; set; }

    // Date and time when the attachment was uploaded.
    public DateTime? UploadedDate { get; set; }

    // Optional property for the url of the file in cloud storage.
    public string? Url { get; set; }

    // The status of the attachment.
    [Required]
    public AttachmentStatus Status { get; set; }

    // Identifier for the user who created the record.
    public string? CreatedBy { get; set; }

    // Timestamp for when the record was created.
    public DateTime? CreatedDate { get; set; }

    // Identifier for the user who last updated the record.
    public string? LastUpdatedBy { get; set; }
    
    // Timestamp for when the record was last updated.
    public DateTime? LastUpdatedDate { get; set; }

    // Constructor with all properties
    public Attachment(Guid id, string fileName, string? displayName, string? contentType, AttachmentType type, DateTime? uploadedDate, string? url, AttachmentStatus status, string? createdBy, DateTime? createdDate, string? lastUpdatedBy, DateTime? lastUpdatedDate)
    {
        Id = id;
        FileName = fileName;
        DisplayName = displayName;
        ContentType = contentType;
        Type = type;
        UploadedDate = uploadedDate;
        Url = url;
        Status = status;
        CreatedBy = createdBy;
        CreatedDate = createdDate;
        LastUpdatedBy = lastUpdatedBy;
        LastUpdatedDate = lastUpdatedDate;
    }
}
