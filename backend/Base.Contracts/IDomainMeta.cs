namespace Base.Contracts;

/// <summary>
/// Defines metadata fields for a domain entity.
/// Useful for tracking creation and modification details, as well as system-level notes.
/// </summary>
public interface IDomainMeta
{
    /// <summary>
    /// Username or identifier of the user who created the entity.
    /// </summary>
    public string CreatedBy { get; set; }
    
    /// <summary>
    /// Timestamp indicating when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Username or identifier of the user who last modified the entity (nullable).
    /// </summary>
    public string? ChangedBy { get; set; }
    
    /// <summary>
    /// Timestamp indicating when the entity was last modified (nullable).
    /// </summary>
    public DateTime? ChangedAt { get; set; }
    
    /// <summary>
    /// System-level notes or internal metadata about the entity (e.g., audit trails, error flags).
    /// </summary>
    public string? SysNotes { get; set; }
}