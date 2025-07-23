using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.DTO.v1.ApiEntities;

public class EnrichedInventory : IDomainId
{
    public Guid Id { get; set; }
    
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public DateTime? EndedAt { get; set; }
    
    public Guid AddressId { get; set; }
    
    public string FullAddress { get; set; } = default!;
    
    public List<string>? AllowedRoles { get; set; }
}