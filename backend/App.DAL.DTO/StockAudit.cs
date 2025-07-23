using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.DAL.DTO;

public class StockAudit : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid StorageRoomId { get; set; }
    
    public StorageRoom? StorageRoom { get; set; }
    
    public ICollection<ActionEntity>? Actions { get; set; }
}