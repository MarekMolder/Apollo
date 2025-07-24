using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.BLL.DTO;

public class StorageRoom : IDomainId
{
    public Guid Id { get; set; }
    
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    
    [MaxLength(255)]
    public string Location { get; set; } = default!;
    
    
    public DateTime? EndedAt { get; set; }
    
    
    public ICollection<StorageRoomInInventory>? StorageRoomInInventories { get; set; }
    
    
    public ICollection<CurrentStock>? CurrentStocks { get; set; }
    
    
    public ICollection<ActionEntity>? Actions { get; set; }
}