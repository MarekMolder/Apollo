using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts;
using Base.Domain;

namespace App.Domain.Logic;

public class StockAudit : BaseEntityUser<AppUser>
{
    public Guid StorageRoomId { get; set; }
    
    public StorageRoom? StorageRoom { get; set; }
    
    public ICollection<ActionEntity>? Actions { get; set; }
}