using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1;

public class ReasonCreate
{
    [MaxLength(256)]
    public string Description { get; set; } = default!;
    
    public DateTime? EndedAt { get; set; }
}