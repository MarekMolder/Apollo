using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.DTO.v1.ApiEntities;

public class EnrichedSupplier : IDomainId
{
    public Guid Id { get; set; }
    
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    [MaxLength(20)]
    public string TelephoneNr { get; set; } = default!;
    
    [MaxLength(128)]
    public string Email { get; set; } = default!;
    
    public Guid AddressId { get; set; }
    
    public string FullAddress { get; set; } = default!;
}