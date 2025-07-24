using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Logic;

public class Inventory : BaseEntity
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public DateTime? EndedAt { get; set; }
    
    public Guid AddressId { get; set; }
    
    public Address? Address { get; set; }
    
    public List<string> AllowedRoles { get; set; } = new();
    
    public ICollection<StorageRoomInInventory>? StorageRoomInInventories { get; set; }
    
}