using App.BLL.Contracts;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;

namespace App.BLL.Services;

public class PersonService : BaseService<Person, DAL.DTO.Person, IPersonRepository>, IPersonService
{
    public PersonService(
        IAppUOW serviceUOW,
        PersonBLLMapper bllMapper) : base(serviceUOW, serviceUOW.PersonRepository, bllMapper)
    {
    }
    
}