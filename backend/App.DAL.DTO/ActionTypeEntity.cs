using System.ComponentModel.DataAnnotations;
using App.DAL.DTO.Enums;
using Base.Contracts;
using Base.Domain;

namespace App.DAL.DTO;

public class ActionTypeEntity : IDomainId 
{
    public Guid Id { get; set; }
    
    [MaxLength(255)]
    public string Name { get; set; } = default!;
    
    
    public DateTime? EndedAt { get; set; }
    
    public App.DAL.DTO.Enums.ActionTypeEnum Code { get; set; }
    
    
    public ICollection<ActionEntity>? Actions { get; set; }
}