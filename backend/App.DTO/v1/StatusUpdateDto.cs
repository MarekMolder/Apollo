namespace App.DTO.v1;

/// <summary>
/// DTO used for updating the status field of an entity (e.g. ActionEntity).
/// </summary>
public class StatusUpdateDto
{
    public string Status { get; set; } = default!;
}