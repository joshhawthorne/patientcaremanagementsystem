namespace PatientRecordsFunctionApp.Models;

public class Record
{
    // Unique identifier for the record.
    public Guid RecordId { get; set; }

    // A description or summary of the record.
    public string Description { get; set; }

    // Identifier for the user who created the record.
    public string CreatedBy { get; set; }

    // Timestamp for when the record was created.
    public DateTime CreatedDate { get; set; }

    // Identifier for the user who last updated the record.
    public string LastUpdatedBy { get; set; }
    
    // Timestamp for when the record was last updated.
    public DateTime LastUpdatedDate { get; set; }

    // Constructor
    public Record(Guid recordId, string description, string createdBy)
    {
        RecordId = recordId;
        Description = description;
        CreatedBy = createdBy;
        CreatedDate = DateTime.Now;
        LastUpdatedBy = createdBy;
        LastUpdatedDate = DateTime.Now;
    }
}
