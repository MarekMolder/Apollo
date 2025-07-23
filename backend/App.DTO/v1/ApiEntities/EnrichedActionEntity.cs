using Base.Contracts;

namespace App.DTO.v1.ApiEntities;

public class EnrichedActionEntity : IDomainId
{
    public Guid Id { get; set; }
    public decimal Quantity { get; set; }
    
    public String Status { get; set; } = default!;
    
    public Guid ActionTypeId { get; set; }
    public string ActionTypeName { get; set; } = default!;
    
    public Guid? ReasonId { get; set; }
    public string? ReasonDescription { get; set; } = default!;
    
    public Guid? SupplierId { get; set; }
    public string? SupplierName { get; set; } = default!;
    
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;

    public Guid? StockAuditId { get; set; }
    
    public Guid StorageRoomId { get; set; }
    public string StorageRoomName { get; set; } = default!;
    
}