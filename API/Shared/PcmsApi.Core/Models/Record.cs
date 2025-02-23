using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PcmsApi.Core.Models;

public class Record
{
    // Unique identifier for the record.
    [Key]
    public Guid Id { get; set; }

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
    public Patient Patient { get; set; }

    // Collection of attachments.
    public IList<Attachment>? Attachments { get; set; }

    // Constructor
    public Record(Guid id, string description, string createdBy, DateTime createdDate, string lastUpdatedBy, DateTime lastUpdatedDate, Patient patient, IList<Attachment> attachments)
    {
        Id = id;
        Description = description;
        CreatedBy = createdBy;
        CreatedDate = createdDate;
        LastUpdatedBy = lastUpdatedBy;
        LastUpdatedDate = lastUpdatedDate;
        Patient = patient;
        Attachments = attachments;
    }
}
