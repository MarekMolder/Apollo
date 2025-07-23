using App.Resources.Domain;
using Base.DAL.Contracts;
using Reason = App.Domain.Logic.Reason;

namespace App.DAL.Contracts;

public interface IReasonRepository: IBaseRepository<App.DAL.DTO.Reason>
{
    
}