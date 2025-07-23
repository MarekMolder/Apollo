using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.BLL.DTO;

public class Address : IDomainId
{
    public Guid Id { get; set; }
    
    public string StreetName { get; set; } = default!;

    public int BuildingNr { get; set; }
    
    [MaxLength(20)]
    public string PostalCode { get; set; } = default!;
    
    
    [MaxLength(255)]
    public string City { get; set; } = default!;
    
    
    [MaxLength(255)]
    public string Province { get; set; } = default!;
    
    
    [MaxLength(255)]
    public string Country { get; set; } = default!;
    
    
    [MaxLength(255)]
    public string Name { get; set; } = default!;
    
    
    public int? UnitNr { get; set; }
    
    
    public ICollection<Inventory>? Inventories { get; set; }
    
    
    public ICollection<Supplier>? Suppliers { get; set; }
}