using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.Enums;
using Base.Contracts;

namespace App.BLL.DTO;

public class ActionTypeEntity : IDomainId 
{
    public Guid Id { get; set; }
    
    [MaxLength(255)]
    [Display(Name = nameof(Name), Prompt = nameof(Name), ResourceType = typeof(App.Resources.Domain.ActionType))]
    public string Name { get; set; } = default!;
    
    
    public DateTime? EndedAt { get; set; }
    
    public App.BLL.DTO.Enums.ActionTypeEnum Code { get; set; }
    
    
    public ICollection<ActionEntity>? Actions { get; set; }
}