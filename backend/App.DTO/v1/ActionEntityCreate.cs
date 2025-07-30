namespace App.DTO.v1;

public class ActionEntityCreate
{
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
    /// </summary>
    public Guid ActionTypeId { get; set; }
    
    /// <summary>
    /// Optional ID of the reason explaining the action.
    /// </summary>
    public Guid? ReasonId { get; set; }
    
    /// <summary>
    /// ID of the affected product.
    /// </summary>
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// ID of the storage room where the action occurred.
    /// </summary>
    public Guid StorageRoomId { get; set; }
}