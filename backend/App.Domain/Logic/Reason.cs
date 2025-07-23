using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Logic;

public class Reason : BaseEntity
{
    [MaxLength(256)]
    public string Description { get; set; } = default!;
    
    public DateTime? EndedAt { get; set; }
    
    public ICollection<ActionEntity>? Actions { get; set; }
}