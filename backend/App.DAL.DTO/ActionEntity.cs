using Base.Contracts;

namespace App.DAL.DTO;

public class ActionEntity : IDomainId
{
    public Guid Id { get; set; }
    
    public decimal Quantity { get; set; }
    
    public String Status { get; set; } = default!;
    
    
    public Guid ActionTypeId { get; set; }
    public ActionTypeEntity? ActionType { get; set; }
    

    public Guid? ReasonId { get; set; }
    public Reason? Reason { get; set; }
    
    
    public Guid? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    
    
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    

    public Guid StorageRoomId { get; set; }
    public StorageRoom? StorageRoom { get; set; }
}