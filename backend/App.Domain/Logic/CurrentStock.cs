using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Logic;

public class CurrentStock : BaseEntity
{
    public decimal Quantity { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public Guid ProductId { get; set; }
    
    public Product? Product { get; set; }
    
    public Guid StorageRoomId { get; set; }
    
    public StorageRoom? StorageRoom { get; set; }
}