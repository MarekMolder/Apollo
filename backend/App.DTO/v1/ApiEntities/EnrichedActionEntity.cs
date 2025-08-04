using Base.Contracts;

namespace App.DTO.v1.ApiEntities;

/// <summary>
/// Data Transfer Object (DTO) representing an enriched view of an ActionEntity.
/// Includes related entity names (e.g., product, action type, reason) for API consumption,
/// </summary>
public class EnrichedActionEntity : IDomainId
{
    /// <summary>
    /// Unique identifier for the action.
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
    /// Name of the action type (e.g., "Remove", "Add").
    /// </summary>
    public Guid ActionTypeId { get; set; }
    public string ActionTypeName { get; set; } = default!;
    
    /// <summary>
    /// Optional ID of the reason explaining the action.
    /// Description of the reason, if applicable.
    /// </summary>
    public Guid? ReasonId { get; set; }
    public string? ReasonDescription { get; set; } = default!;
    
    /// <summary>
    /// ID of the affected product.
    /// Name of the related product.
    /// </summary>
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    
    /// <summary>
    /// ID of the storage room where the action occurred.
    /// Name of the storage room.
    /// </summary>
    public Guid StorageRoomId { get; set; }
    public string StorageRoomName { get; set; } = default!;
}