using App.DTO.v1.ApiEntities;
using Base.Contracts;

namespace App.DTO.v1.ApiMappers;

public class EnrichedSupplierApiMapper : IMapper<EnrichedSupplier, BLL.DTO.Supplier>
{
    public EnrichedSupplier? Map(BLL.DTO.Supplier? entity)
    {
        if (entity == null) return null;

        var address = entity.Address;

        var fullAddress = address != null
            ? $"{(string.IsNullOrWhiteSpace(address.Name) ? "" : address.Name + ", ")}{address.StreetName} {address.BuildingNr}{(address.UnitNr != null ? "-" + address.UnitNr : "")}, {address.PostalCode} {address.City}, {address.Province}, {address.Country}"
            : "Unknown";

        return new EnrichedSupplier
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            AddressId = entity.AddressId,
            FullAddress = fullAddress
        };
    }

    public BLL.DTO.Supplier? Map(EnrichedSupplier? entity)
    {
        throw new NotImplementedException("Mapping from API to BLL is not required.");
    }
    
}