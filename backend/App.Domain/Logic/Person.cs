using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain.Logic;

public class Person : BaseEntityUser<AppUser>
{
    [MaxLength(128)]
    public string PersonName { get; set; } = default!;
    
    public ICollection<ActionEntity>? Actions { get; set; }
    
    public ICollection<StockAudit>? StockAudits { get; set; }
}