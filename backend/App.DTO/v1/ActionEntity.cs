using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.DTO.v1;

public class ActionEntity : IDomainId
{
    public Guid Id { get; set; }
    public decimal Quantity { get; set; }
    
    public String Status { get; set; } = default!;
    
    public Guid ActionTypeId { get; set; }
    
    public Guid? ReasonId { get; set; }
    
    public Guid? SupplierId { get; set; }
    
    public Guid ProductId { get; set; }

    public Guid? StockAuditId { get; set; }
    
    public Guid StorageRoomId { get; set; }
}