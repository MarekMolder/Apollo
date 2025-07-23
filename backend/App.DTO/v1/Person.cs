using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace App.DTO.v1;

public class Person: IDomainId
{
    public Guid Id { get; set; }

    [Required]
    public string PersonName { get; set; } = default!;
}