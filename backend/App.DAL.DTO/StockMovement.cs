using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.DAL.DTO;

public class StockMovement : IDomainId
{
    public Guid Id { get; set; }
    
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