using Base.Contracts;

namespace App.DTO.v1;

public class CurrentStock : IDomainId
{
    public Guid Id { get; set; }
    
    public decimal Quantity { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public Guid ProductId { get; set; }
    
    public Guid StorageRoomId { get; set; }
    
}