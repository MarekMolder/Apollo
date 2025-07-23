using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.BLL.DTO;

public class ProductCategory : IDomainId
{
    public Guid Id { get; set; }
    
    [MaxLength(128)]

    public string Name { get; set; } = default!;
    
    
    public DateTime? EndedAt { get; set; }
    
    
    public ICollection<Product>? Products { get; set; }
}