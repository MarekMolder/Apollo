using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.DTO.v1;

public class StorageRoom : IDomainId
{
    public Guid Id { get; set; }
    
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public Guid AddressId { get; set; }
    
    public List<string>? AllowedRoles { get; set; }
    public DateTime? EndedAt { get; set; }
}