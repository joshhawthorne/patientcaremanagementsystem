using System.ComponentModel.DataAnnotations;

namespace PcmsApi.Core.Models;

public class InternationalAddress
{
    // Primary address line (e.g., street address or PO Box)
    [Required]
    public string AddressLine1 { get; set; }

    // Optional secondary address line (e.g., apartment, suite, unit)
    public string? AddressLine2 { get; set; }

    // City or locality name
    [Required]
    public string City { get; set; }

    // State, province, or region. Not all countries use this field.
    public string? StateOrProvince { get; set; }

    // Postal code. Some countries might not have postal codes.
    public string? PostalCode { get; set; }

    // Country name or ISO country code (recommended)
    [Required]
    public string Country { get; set; }

    public string? District { get; set; }

    // Constructor
    public InternationalAddress(string addressLine1, string city, string country)
    {
        AddressLine1 = addressLine1;
        City = city;
        Country = country;
    }
}
