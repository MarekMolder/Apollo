using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Logic;

public class StockMovement : BaseEntity
{
    public decimal Amount { get; set; }
    
    public Guid ProductId { get; set; }
    
    public Product? Product { get; set; }
    
    public Guid? FromStorageRoomId { get; set; }
    
    public StorageRoom? FromStorageRoom { get; set; }
    
    public Guid? ToStorageRoomId { get; set; }
    
    public StorageRoom? ToStorageRoom { get; set; }
    
    public Guid FromInventoryId { get; set; }
    
    public Inventory? FromInventory { get; set; }
    
    public Guid ToInventoryId { get; set; }

    public Inventory? ToInventory { get; set; }
    
}