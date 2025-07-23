using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.BLL.DTO;

public class Person: IDomainId
{
    public Guid Id { get; set; }
    
    public string PersonName { get; set; } = default!;
    
    public ICollection<ActionEntity>? Actions { get; set; }
    
    public ICollection<StockAudit>? StockAudits { get; set; }
}