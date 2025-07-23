using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.DTO.v1;

public class StockAudit : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid StorageRoomId { get; set; }
}