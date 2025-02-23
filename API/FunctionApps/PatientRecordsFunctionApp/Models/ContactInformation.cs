using System.ComponentModel.DataAnnotations;

namespace PatientRecordsFunctionApp.Models;

public class ContactInformation
{
    // Home address is required
    [Required]
    public InternationalAddress HomeAddress { get; set; }

    // Work address is optional
    public InternationalAddress? WorkAddress { get; set; }

    // Primary phone number in international format
    [Required]
    public InternationalPhoneNumber PhoneNumber { get; set; }

    // Optional alternate phone number
    public InternationalPhoneNumber? AlternatePhoneNumber { get; set; }

    // Optional email address
    [EmailAddress]
    public string? Email { get; set; } 

    // Constructor
    public ContactInformation(InternationalAddress homeAddress, InternationalPhoneNumber phoneNumber)
    {
        HomeAddress = homeAddress;
        PhoneNumber = phoneNumber;
    }
}
