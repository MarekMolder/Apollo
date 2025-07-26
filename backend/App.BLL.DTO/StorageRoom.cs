using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.BLL.DTO;

public class StorageRoom : IDomainId
{
    public Guid Id { get; set; }
    
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public Guid AddressId { get; set; }
    public Address? Address { get; set; }
    
    public List<string>? AllowedRoles { get; set; }
    public DateTime? EndedAt { get; set; }
    
    public ICollection<CurrentStock>? CurrentStocks { get; set; }
    
    
    public ICollection<ActionEntity>? Actions { get; set; }
}