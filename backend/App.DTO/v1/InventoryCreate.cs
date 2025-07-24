using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1;

public class InventoryCreate
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public DateTime? EndedAt { get; set; }
    
    public Guid AddressId { get; set; }
    
    public List<string>? AllowedRoles { get; set; }
    
}