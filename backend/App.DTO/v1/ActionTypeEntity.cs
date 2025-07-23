using System.ComponentModel.DataAnnotations;
using App.DTO.v1.Enums;
using Base.Contracts;

namespace App.DTO.v1;

public class ActionTypeEntity : IDomainId 
{
    public Guid Id { get; set; }
    
    [MaxLength(255)]
    public string Name { get; set; } = default!;
    
    public DateTime? EndedAt { get; set; }
    
    public ActionTypeEnum Code { get; set; }
}