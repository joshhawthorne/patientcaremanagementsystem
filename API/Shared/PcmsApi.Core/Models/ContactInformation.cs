using System.ComponentModel.DataAnnotations;

namespace PcmsApi.Core.Models;

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

    // Constructor with all properties
    public ContactInformation(InternationalAddress homeAddress, InternationalAddress? workAddress, InternationalPhoneNumber phoneNumber, InternationalPhoneNumber? alternatePhoneNumber, string? email)
    {
        HomeAddress = homeAddress;
        WorkAddress = workAddress;
        PhoneNumber = phoneNumber;
        AlternatePhoneNumber = alternatePhoneNumber;
        Email = email;
    }
}
