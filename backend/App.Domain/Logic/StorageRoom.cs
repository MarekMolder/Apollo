using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain.Logic;

public class StorageRoom : BaseEntity
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    [MaxLength(255)]
    public string Location { get; set; } = default!;
    
    
    public DateTime? EndedAt { get; set; }
    
    public ICollection<StockAudit>? StockAudits { get; set; }
    
    public ICollection<StorageRoomInInventory>? StorageRoomInInventories { get; set; }
    
    public ICollection<CurrentStock>? CurrentStocks { get; set; }
    
    [InverseProperty("FromStorageRoom")]
    public ICollection<StockMovement>? FromStockMovements { get; set; }
    
    [InverseProperty("ToStorageRoom")]
    public ICollection<StockMovement>? ToStockMovements { get; set; }
    
    public ICollection<ActionEntity>? Actions { get; set; }
}