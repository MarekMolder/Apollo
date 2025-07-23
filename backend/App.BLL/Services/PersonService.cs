using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DTO.v1;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class PersonService : BaseService<App.BLL.DTO.Person, App.DAL.DTO.Person, App.DAL.Contracts.IPersonRepository>, IPersonService
{
    public PersonService(
        IAppUOW serviceUOW,
        PersonBLLMapper bllMapper) : base(serviceUOW, serviceUOW.PersonRepository, bllMapper)
    {
    }
    
}