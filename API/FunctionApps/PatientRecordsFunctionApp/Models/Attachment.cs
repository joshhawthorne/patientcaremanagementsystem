using System.ComponentModel.DataAnnotations;
namespace PatientRecordsFunctionApp.Models;

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

    // Optional property for the binary content of the file.
    // In a production scenario, this might be stored in a file system or cloud storage.
    public byte[]? FileContent { get; set; }

    // The status of the attachment.
    [Required]
    public AttachmentStatus Status { get; set; }

    // Constructor
    public Attachment(Guid id, string fileName, AttachmentType type, AttachmentStatus status)
    {
        Id = id;
        FileName = fileName;
        Type = type;
        UploadedDate = DateTime.Now;
        Status = status;
    }
}
