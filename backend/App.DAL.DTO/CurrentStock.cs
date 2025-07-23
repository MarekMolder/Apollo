using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.DAL.DTO;

public class CurrentStock : IDomainId
{
    public Guid Id { get; set; }
    
    public decimal Quantity { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public Guid ProductId { get; set; }
    
    public Product? Product { get; set; }
    
    
    public Guid StorageRoomId { get; set; }
    
    public StorageRoom? StorageRoom { get; set; }
}