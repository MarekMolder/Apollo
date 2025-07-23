using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Contracts;

namespace App.DTO.v1;

public class StorageRoomCreate
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    [MaxLength(255)]
    public string Location { get; set; } = default!;
    public DateTime? EndedAt { get; set; }
}