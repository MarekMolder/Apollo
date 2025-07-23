using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Logic;

public class ProductCategory : BaseEntity
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public DateTime? EndedAt { get; set; }
    
    public ICollection<Product>? Products { get; set; }
}