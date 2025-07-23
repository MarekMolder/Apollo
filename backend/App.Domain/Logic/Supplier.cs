using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Logic;

public class Supplier : BaseEntity
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    
    [MaxLength(20)]
    public string TelephoneNr { get; set; } = default!;
    
    
    [MaxLength(128)]
    public string Email { get; set; } = default!;
    

    public Guid AddressId { get; set; }
    
    public Address? Address { get; set; }
    
    public ICollection<ActionEntity>? Actions { get; set; }
}