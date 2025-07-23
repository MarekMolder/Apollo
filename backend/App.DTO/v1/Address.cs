using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.DTO.v1;

public class Address : IDomainId
{
    public Guid Id { get; set; }
    
    [MaxLength(255)]
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
}