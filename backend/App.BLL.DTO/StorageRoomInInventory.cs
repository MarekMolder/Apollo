using Base.Contracts;

namespace App.BLL.DTO;

public class StorageRoomInInventory : IDomainId
{
    public Guid Id { get; set; }
    
    public DateTime? EndedAt { get; set; }
    
    
    public Guid InventoryId { get; set; }
    public Inventory? Inventory { get; set; }
    
    public Guid StorageRoomId { get; set; }
    public StorageRoom? StorageRoom { get; set; }
}