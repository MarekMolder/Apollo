using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1;

public class ProductCreate
{
    [MaxLength(20)]
    public string Unit { get; set; } = default!;
    
    public decimal Volume { get; set; } = default!;
    
    [MaxLength(255)]
    public string Code { get; set; } = default!;
    
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public decimal Price { get; set; }
    
    public decimal Quantity { get; set; }
    
    public Guid ProductCategoryId { get; set; }
    
    public bool IsComponent { get; set; } = false;
}