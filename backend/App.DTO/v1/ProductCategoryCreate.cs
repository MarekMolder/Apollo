using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.DTO.v1;

public class ProductCategoryCreate
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public DateTime? EndedAt { get; set; }
}