using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.FuelSecurity.Domain.Model;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface IUserToUserDto : IFacadeMapper<User, UserDto>
    {
        UserDto MapToModel(User entity);
        User MapToEntity(UserDto model);
    }
}
