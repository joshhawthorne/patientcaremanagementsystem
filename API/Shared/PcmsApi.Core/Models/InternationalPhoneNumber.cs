using System.ComponentModel.DataAnnotations;

namespace PcmsApi.Core.Models;

public class InternationalPhoneNumber
{
    // Country code in international format (e.g., "+1", "+44")
    [Required]
    public string CountryCode { get; set; }

    // Local or national number (without the country code)
    [Required]
    public string NationalNumber { get; set; }

    // Constructor
    public InternationalPhoneNumber(string countryCode, string nationalNumber)
    {
        CountryCode = countryCode;
        NationalNumber = nationalNumber;
    }
}
