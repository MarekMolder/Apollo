using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Contracts;

namespace App.DAL.DTO;

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