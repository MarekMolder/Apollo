using Base.Contracts;

namespace App.DTO.v1;

public class StorageRoomInInventory : IDomainId
{
    public Guid Id { get; set; }
    
    public DateTime? EndedAt { get; set; }
    
    public Guid InventoryId { get; set; }

    public Guid StorageRoomId { get; set; }
}