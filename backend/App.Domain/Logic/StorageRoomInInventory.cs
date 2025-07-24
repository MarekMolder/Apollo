using Base.Domain;

namespace App.Domain.Logic;

public class StorageRoomInInventory : BaseEntity
{
    public DateTime? EndedAt { get; set; }
    
    public Guid InventoryId { get; set; }
    
    public Inventory? Inventory { get; set; }
    
    public Guid StorageRoomId { get; set; }
    
    public StorageRoom? StorageRoom { get; set; }
}