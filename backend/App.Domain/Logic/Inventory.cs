using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain.Logic;

public class Inventory : BaseEntity
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public DateTime? EndedAt { get; set; }
    
    public Guid AddressId { get; set; }
    
    public Address? Address { get; set; }
    
    public ICollection<string>? AllowedRoles { get; set; } = new List<string>();
    
    public ICollection<StorageRoomInInventory>? StorageRoomInInventories { get; set; }
    
    [InverseProperty("FromInventory")]
    public ICollection<StockMovement>? FromStockMovements { get; set; }
    
    [InverseProperty("ToInventory")]
    public ICollection<StockMovement>? ToStockMovements { get; set; }
}