using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1;

public class PersonCreate
{
    [Required]
    public string PersonName { get; set; } = default!;
}