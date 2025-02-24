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
    public string FirstName { get; set; } = string.Empty;

    // Patient's middle name (optional).
    public string? MiddleName { get; set; }

    // Patient's last name.
    [Required]
    public string LastName { get; set; } = string.Empty;

    // Patient's preferred name.
    public string? PreferredName { get; set; }

    // Collection of patient records.
    public IList<Record>? Records { get; set; }

    // Patient's contact information.
    public ContactInformation? ContactInformation { get; set; }

    // Patient's medical conditions.
    public string? MedicalConditions { get; set; } // ToDo: Reconsider this as something more robust, either through a relation to a medication table or a more structured format.

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
    {}

    // Constructor with all properties
    public Patient(Guid id, string firstName, string? middleName, string lastName, string? preferredName, IList<Record>? records, ContactInformation? contactInformation, string? medicalConditions, string? createdBy, DateTime? createdDate, string? lastUpdatedBy, DateTime? lastUpdatedDate)
    {
        Id = id;
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        PreferredName = preferredName;
        Records = records ?? new List<Record>();
        ContactInformation = contactInformation;
        MedicalConditions = medicalConditions;
        CreatedBy = createdBy;
        CreatedDate = createdDate ?? DateTime.Now;
        LastUpdatedBy = lastUpdatedBy;
        LastUpdatedDate = lastUpdatedDate ?? DateTime.Now;
    }
}
