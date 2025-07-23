using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.DTO.v1;

public class StockMovementCreate
{
    public decimal Amount { get; set; }

    public Guid ProductId { get; set; }
    
    public Guid? FromStorageRoomId { get; set; }
    
    public Guid? ToStorageRoomId { get; set; }
    
    public Guid FromInventoryId { get; set; }

    public Guid ToInventoryId { get; set; }
}