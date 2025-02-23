using System.ComponentModel.DataAnnotations;

namespace PatientRecordsFunctionApp.Models;

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
}
