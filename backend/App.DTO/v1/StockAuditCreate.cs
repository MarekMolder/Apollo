using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.DTO.v1;

public class StockAuditCreate
{
    public Guid StorageRoomId { get; set; }
}