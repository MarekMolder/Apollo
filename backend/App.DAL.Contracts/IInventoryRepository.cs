﻿using App.DAL.DTO;
using Base.DAL.Contracts;

namespace App.DAL.Contracts;

public interface IInventoryRepository: IBaseRepository<Inventory>
{
    Task<IEnumerable<Inventory?>> GetEnrichedInventories();
}