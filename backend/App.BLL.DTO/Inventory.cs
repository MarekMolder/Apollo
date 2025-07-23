using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Contracts;

namespace App.BLL.DTO;

public class Inventory : IDomainId
{
    public Guid Id { get; set; }
    
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    
    public DateTime? EndedAt { get; set; }
    
    
    public Guid AddressId { get; set; }
    public Address? Address { get; set; }
    
    
    public ICollection<StorageRoomInInventory>? StorageRoomInInventories { get; set; }
    
    public List<string>? AllowedRoles { get; set; }
    
    [InverseProperty("FromInventory")]
    public ICollection<StockMovement>? FromStockMovements { get; set; }
    
    
    [InverseProperty("ToInventory")]
    public ICollection<StockMovement>? ToStockMovements { get; set; }
}