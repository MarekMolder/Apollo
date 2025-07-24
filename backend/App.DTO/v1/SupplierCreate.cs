using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1;

public class SupplierCreate
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    [MaxLength(20)]
    public string TelephoneNr { get; set; } = default!;
    
    [MaxLength(128)]
    public string Email { get; set; } = default!;
    
    public Guid AddressId { get; set; }
}