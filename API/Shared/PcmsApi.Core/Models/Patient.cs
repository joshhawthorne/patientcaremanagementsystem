using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PcmsApi.Core.Models;

public class Patient
{
    // Unique identifier for the patient.
    [Key]
    public Guid Id { get; set; }

    // Patient's first name.
    [Required]
    public string FirstName { get; set; }

    // Patient's middle name (optional).
    public string? MiddleName { get; set; }

    // Patient's last name.
    [Required]
    public string LastName { get; set; }

    // Patient's preferred name.
    public string? PreferredName { get; set; }

    // Collection of patient records.
    public IList<Record>? Records { get; set; }

    // Patient's contact information.
    public ContactInformation? ContactInformation { get; set; }

    // Identifier for the user who created the record.
    public string? CreatedBy { get; set; }

    // Timestamp for when the record was created.
    public DateTime? CreatedDate { get; set; }

    // Identifier for the user who last updated the record.
    public string? LastUpdatedBy { get; set; }
    
    // Timestamp for when the record was last updated.
    public DateTime? LastUpdatedDate { get; set; }

    // Default constructor
    public Patient()
    {
        Records = new List<Record>();
        CreatedDate = DateTime.Now;
        LastUpdatedDate = DateTime.Now;
    }

    // Constructor initializes the Records collection and required fields.
    public Patient(string firstName, string lastName)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Records = new List<Record>();
        CreatedDate = DateTime.Now;
        LastUpdatedDate = DateTime.Now;
    }

    // Constructor with all properties
    public Patient(Guid id, string firstName, string? middleName, string lastName, string? preferredName, IList<Record>? records, ContactInformation? contactInformation, string? createdBy, DateTime? createdDate, string? lastUpdatedBy, DateTime? lastUpdatedDate)
    {
        Id = id;
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        PreferredName = preferredName;
        Records = records ?? new List<Record>();
        ContactInformation = contactInformation;
        CreatedBy = createdBy;
        CreatedDate = createdDate ?? DateTime.Now;
        LastUpdatedBy = lastUpdatedBy;
        LastUpdatedDate = lastUpdatedDate ?? DateTime.Now;
    }
}
