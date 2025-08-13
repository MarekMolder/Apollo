using Base.Contracts;

namespace App.BLL.DTO;

/// <summary>
/// Represents an action performed on a product (e.g. Add, Remove), with contextual metadata.
/// </summary>
public class ActionEntity : IDomainId
{
    /// <summary>
    /// Unique identifier of the action.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Quantity of the product affected by this action.
    /// </summary>
    public decimal Quantity { get; set; }
    
    /// <summary>
    /// Status of the action (e.g. "Accepted", "Declined").
    /// </summary>
    public String Status { get; set; } = default!;
    
    /// <summary>
    /// ID of the associated action type (e.g. Add, Remove).
    /// Related ActionType entity.
    /// </summary>
    public Guid ActionTypeId { get; set; }
    public BLL.DTO.ActionTypeEntity? ActionType { get; set; }
    
    /// <summary>
    /// Optional ID of the reason explaining the action.
    /// Related Reason entity (optional).
    /// </summary>
    public Guid? ReasonId { get; set; }
    public BLL.DTO.Reason? Reason { get; set; }
    
    /// <summary>
    /// ID of the affected product.
    /// Related Product entity.
    /// </summary>
    public Guid ProductId { get; set; }
    public BLL.DTO.Product? Product { get; set; }
    
    /// <summary>
    /// ID of the storage room where the action occurred.
    /// Related StorageRoom entity.
    /// </summary>
    public Guid StorageRoomId { get; set; }
    public BLL.DTO.StorageRoom? StorageRoom { get; set; }
    
    /// <summary>
    /// META values
    /// </summary>
    public string CreatedBy { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string? ChangedBy { get; set; }
    public DateTime? ChangedAt { get; set; }
}