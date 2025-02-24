using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PcmsApi.Core.Models;

public class Record
{
    // Unique identifier for the record.
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    // A description or summary of the record.
    public string? Description { get; set; }

    // Identifier for the user who created the record.
    public string? CreatedBy { get; set; }

    // Timestamp for when the record was created.
    public DateTime? CreatedDate { get; set; }

    // Identifier for the user who last updated the record.
    public string? LastUpdatedBy { get; set; }
    
    // Timestamp for when the record was last updated.
    public DateTime? LastUpdatedDate { get; set; }

    // The patient the record is for.
    [Required]
    public Patient Patient { get; set; } = new Patient();

    // Collection of attachments.
    public IList<Attachment>? Attachments { get; set; }

    // Indicates whether the record is active.
    public bool IsActive { get; set; } = true;

    // Default constructor
    public Record()
    {}

    // Constructor with all properties
    public Record(Guid id, string? description, string? createdBy, DateTime? createdDate, string? lastUpdatedBy, DateTime? lastUpdatedDate, Patient patient, IList<Attachment>? attachments, bool isActive)
    {
        Id = id;
        Description = description;
        CreatedBy = createdBy;
        CreatedDate = createdDate;
        LastUpdatedBy = lastUpdatedBy;
        LastUpdatedDate = lastUpdatedDate;
        Patient = patient;
        Attachments = attachments;
        IsActive = isActive;
    }
}
