using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Logic;

public class StorageRoom : BaseEntity
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public Guid AddressId { get; set; }
    
    public Address? Address { get; set; }
    
    public List<string> AllowedRoles { get; set; } = new();
    
    public DateTime? EndedAt { get; set; }
    
    public ICollection<CurrentStock>? CurrentStocks { get; set; }
    
    
    public ICollection<ActionEntity>? Actions { get; set; }
}