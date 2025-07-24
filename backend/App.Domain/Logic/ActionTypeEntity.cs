using System.ComponentModel.DataAnnotations;
using App.Domain.Enums;
using App.Resources.Domain;
using Base.Domain;

namespace App.Domain.Logic;

public class ActionTypeEntity : BaseEntity
{
    [MaxLength(255)]
    [Display(Name = nameof(Name), Prompt = nameof(Name), ResourceType = typeof(ActionType))]
    public string Name { get; set; } = default!;
    
    public DateTime? EndedAt { get; set; }
    
    public ActionTypeEnum Code { get; set; }
    
    
    public ICollection<ActionEntity>? Actions { get; set; }
}