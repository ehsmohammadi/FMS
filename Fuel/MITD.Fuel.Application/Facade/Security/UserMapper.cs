using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MITD.Core;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.FuelSecurity.Domain.Model;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade
{
    public class UserStateDtoMapper : BaseMapper<ClaimsPrincipal, UserStateDTO>, IFacadeMapper<ClaimsPrincipal, UserStateDTO>
    {
        public override UserStateDTO MapToModel(ClaimsPrincipal user)
        {
            UserStateDTO userStateDto = new UserStateDTO
            {
                Username = user.Identity.Name,
                CurrentWorkListUserName = user.Claims.Single(c => c.Type == "CurrentUsername").Value
            };

            string fName = "", lName = "", jobPositionName = "";

            var fNameClaim = user.Claims.SingleOrDefault(c => c.Type == "http://identityserver.thinktecture.com/claims/profileclaims/firstname");
            if (fNameClaim != null)
                fName = fNameClaim.Value;
            userStateDto.FirstName = fName;


            var lNameClaim = user.Claims.SingleOrDefault(c => c.Type == "http://identityserver.thinktecture.com/claims/profileclaims/lastname");
            if (lNameClaim != null)
                lName = lNameClaim.Value;
            userStateDto.LastName = lName;

            var jobPositionNameClaim = user.Claims.SingleOrDefault(c => c.Type == "http://identityserver.thinktecture.com/claims/profileclaims/jobpositionnames");
            if (jobPositionNameClaim != null)
                jobPositionName = jobPositionNameClaim.Value;


            var claimRoles = user.Claims.Single(c => c.Type == "CurrentUserRoles").Value.Split(',');

            var res = new List<User>();
            foreach (var role in claimRoles)
            {
                userStateDto.RoleNames.Add(role);
                if (role == "Employee")
                {
                    var employeeNoClaim =
                        user.Claims.Single(
                            c => c.Type == "http://identityserver.thinktecture.com/claims/profileclaims/employeeno");

                    if (employeeNoClaim != null)
                        userStateDto.EmployeeNo = employeeNoClaim.Value;

                    userStateDto.JobPositionNames = jobPositionName;
                }
            }

            var claimUserActions = user.Claims.SingleOrDefault(c => c.Type == "CurrentUserActions");
            if (claimUserActions != null && !string.IsNullOrWhiteSpace(claimUserActions.Value))
            {
                foreach (var actionCode in claimUserActions.Value.Split(','))
                {
                    var actionType = ActionType.FromValue(int.Parse(actionCode));
                    if (actionType != null)
                        userStateDto.PermittedActions.Add(new ActionTypeDto
                        {
                            Id = actionType.Id,
                            ActionName = actionType.Name,
                            Description = actionType.Description
                        });
                }
            }
            //if (claimUserActions != null && !string.IsNullOrWhiteSpace(claimUserActions.Value))
            //{
            //    foreach (var actionCode in claimUserActions.Value.Split(','))
            //    {
            //        var actionType = ActionType.FromValue(int.Parse(actionCode));
            //        if (actionType != null)
            //            userStateDto.PermittedActions.Add(new ActionTypeDto
            //            {
            //                Id = actionType.Id,
            //                ActionName = actionType.Name,
            //                Description = actionType.Description
            //            });
            //    }
                //  }




            return userStateDto;
        }

        public override ClaimsPrincipal MapToEntity(UserStateDTO model)
        {
            throw new NotSupportedException("map UserStateDTO not supported ");
        }
    }

    public class UserSecurityMapper : BaseMapper<List<User>, ClaimsPrincipal>, IMapper<List<User>, ClaimsPrincipal>
    {
        public override ClaimsPrincipal MapToModel(List<User> entity)
        {
            throw new NotSupportedException("map List<User> to ClaimsPrincipal not supported  ");
        }

        public override List<User> MapToEntity(ClaimsPrincipal user)
        {
            string fName = "", lName = "";
            var fNameClaim = user.Claims.SingleOrDefault(c => c.Type == "http://identityserver.thinktecture.com/claims/profileclaims/firstname");
            var lNameClaim = user.Claims.SingleOrDefault(c => c.Type == "http://identityserver.thinktecture.com/claims/profileclaims/lastname");

            if (fNameClaim != null)
                fName = fNameClaim.Value;
            if (lNameClaim != null)
                lName = lNameClaim.Value;

            var username = user.Claims.Single(c => c.Type == "CurrentUsername").Value;

            string email = string.Empty;
            var claimEmail = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email);
            if (claimEmail != null)
                email = claimEmail.Value;



            var claimRoles = user.Claims.Single(c => c.Type == "CurrentUserRoles").Value.Split(',');

            var res = new List<User>();
            foreach (var role in claimRoles)
            {

                switch (role)
                {
                    case "Admin":
                        res.Add(new AdminUser(fName, lName, email, username));
                        break;
                    case "Integration":
                        res.Add(new IntegrationUser(fName, lName, email, username));
                        break;

                    case "Commercial":
                        res.Add(new CommercialUser(0, fName, lName, email, username));
                        break;
                    case "CommercialManager":
                        res.Add(new CommercialManagerUser(0, fName, lName, email, username));
                        break;
                    case "FinancialManager":
                        res.Add(new FinancialManagerUser(0, fName, lName, email, username));
                        break;
                    case "Financial":
                        res.Add(new FinancialUser(0, fName, lName, email, username));
                        break;
                    case "ReadOnly":
                        res.Add(new ReadOnlyUser(fName, lName, email, username));
                        break;
                    case "TestRole":
                        res.Add(new TestRole(0,fName, lName, email, username));
                        break;
                }
                //if (role == "Employee")
                //{
                //    var employeeNoClaim = user.Claims.Single(c => c.Type == "http://identityserver.thinktecture.com/claims/profileclaims/employeeno");
                //    if (employeeNoClaim != null)
                //        res.Add(new EmployeeUser(new PartyId(username), employeeNoClaim.Value, fName, lName, email));
                //}
            }
            return res;
        }
    }

    public class UserDtoMapper : BaseFacadeMapper<User, UserDto>, IUserToUserDto// BaseMapper<User, UserDto>, IMapper<User, UserDto>
    {

        public override UserDto MapToModel(User entity)
        {
            var res = new UserDto
            {
                Id = entity.Id,
                PartyName = entity.PartyName,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                //Email = entity.Email,
                Active = entity.Active,
                UserName = entity.UserName,
                CompanyDto = new CompanyDto() { Id = entity.CompanyId }


            };
            res.CustomActions = new Dictionary<int, bool>();
            entity.CustomActions.ForEach(c =>
            {
                if (res.CustomActions.ContainsKey(c.ActionTypeId))
                {
                    if (res.CustomActions[c.ActionTypeId] == true && c.IsGranted == false)
                        res.CustomActions[c.ActionTypeId] = false;
                }
                else
                    res.CustomActions.Add(c.ActionTypeId, c.IsGranted);
            });
            return res;

        }

        public override User MapToEntity(UserDto model)
        {
            var res = new User(model.Id, model.PartyName, model.FirstName, model.LastName, "", model.Active, model.UserName);
            return res;

        }

    }

    public class UserDtoWithActionsMapper : BaseMapper<User, UserDTOWithActions>, IMapper<User, UserDTOWithActions>
    {

        public override UserDTOWithActions MapToModel(User entity)
        {
            var res = new UserDTOWithActions
            {
                PartyName = entity.PartyName,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                IsActive = entity.Active,

                //Privileges = entity.CustomActions
                //  ActionCodes = new List<int>() { (int)ActionType.AddUser, (int)ActionType.ModifyUser, (int)ActionType.DeleteUser }
            };
            return res;

        }

        public override User MapToEntity(UserDTOWithActions model)
        {
            throw new Exception("UserDTOWithActions to User mapping not supported");

        }

    }

    public class UserDescriptionDtoMapper : BaseMapper<User, UserDescriptionDTO>, IMapper<User, UserDescriptionDTO>
    {

        public override UserDescriptionDTO MapToModel(User entity)
        {
            var res = new UserDescriptionDTO
            {
                PartyName = entity.PartyName,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
            };
            return res;

        }

        public override User MapToEntity(UserDescriptionDTO model)
        {
            throw new NotSupportedException("map UserDescriptionDTO to User not supported");

        }

    }

}
