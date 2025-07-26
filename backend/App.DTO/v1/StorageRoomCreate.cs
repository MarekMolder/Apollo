using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1;

public class StorageRoomCreate
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public Guid AddressId { get; set; }
    
    public List<string>? AllowedRoles { get; set; }
    public DateTime? EndedAt { get; set; }
}