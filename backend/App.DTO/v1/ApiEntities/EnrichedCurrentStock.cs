using Base.Contracts;

namespace App.DTO.v1.ApiEntities;

public class EnrichedCurrentStock : IDomainId
{
    public Guid Id { get; set; }
    
    public decimal Quantity { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public Guid ProductId { get; set; }
    
    public string ProductName { get; set; } = default!;
    
    public string ProductCode { get; set; } = default!;
    
    public Guid StorageRoomId { get; set; }
}