using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.BLL.DTO;

public class Reason : IDomainId
{
    public Guid Id { get; set; }
    
    [MaxLength(256)]
    public string Description { get; set; } = default!;
    

    public DateTime? EndedAt { get; set; }
    
    
    public ICollection<ActionEntity>? Actions { get; set; }
}