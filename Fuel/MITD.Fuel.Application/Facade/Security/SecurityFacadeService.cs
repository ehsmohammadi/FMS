using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Application.Service.Security;
using MITD.Fuel.Application.UserManagement;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.FuelSecurity.Domain.Model;
using MITD.FuelSecurity.Domain.Model.Repository;
using MITD.FuelSecurity.Domain.Model.Service;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade
{
    public class SecurityFacadeService : ISecurityFacadeService
    {
        //  private readonly IMapper<List<ActionType>, ClaimsPrincipal> _userActionMapper;
        private readonly UserActionsMapper _userActionMapper;
        private readonly UserSecurityMapper _userSecurityMapper;
        private readonly ISecurityApplicationService _securityApplicationService;
        private IUserRepository _userRepository;
        private IActionToDtoMapper _actionTypeDTOMapper;
        private ISecurityServiceChecker _securityServiceChecker;
        IMapper<List<User>, ClaimsPrincipal> _userMapper;
        public SecurityFacadeService(//(IMapper<List<ActionType>, ClaimsPrincipal> userActionMapper,
            ISecurityApplicationService securityApplicationService,
            IUserRepository userRepository, IActionToDtoMapper actionTypeDTOMapper, ISecurityServiceChecker securityServiceChecker,
            IMapper<List<User>, ClaimsPrincipal> userMapper)
        {
            this._userActionMapper = new UserActionsMapper();
            this._securityApplicationService = securityApplicationService;
            this._userSecurityMapper = new UserSecurityMapper();
            this._userRepository = userRepository;
            this._actionTypeDTOMapper = actionTypeDTOMapper;
            this._securityServiceChecker = securityServiceChecker;
            this._userMapper = userMapper;
        }

        public List<ActionTypeDto> GetUserAuthorizedActions(string userName)
        {
            var result = new List<ActionTypeDto>();
            var user = _userRepository.GetUserById(userName);
            ServicePointManager.ServerCertificateValidationCallback

                   += (sender, certificate, chain, errors) => true;

            var ums = new UserManagementServiceClient();
            ums.Open();
            var res = ums.GetRolesForUser(userName);

            var actionRole = _actionTypeDTOMapper.MapToDtoModel(_securityServiceChecker.GetAllAuthorizedActionTypesForRole(res.ToList())).ToList();

            result.AddRange(actionRole);
            user.Groups.ForEach(c =>
            {
                foreach (var action in c.CustomActions)
                {
                    if (result.All(d => d.Id != action.ActionTypeId))
                    {
                        result.Add(_actionTypeDTOMapper.MapToDtoModel(action.ActionType));
                    }
                    else
                    {
                        if (!action.IsGranted)
                        {
                            result.Remove(result.Find(f => f.Id == action.ActionType.Id));
                        }
                    }
                }
            });

            foreach (var action in user.CustomActions)
            {
                if (result.All(d => d.Id != action.ActionTypeId))
                {
                    result.Add(_actionTypeDTOMapper.MapToDtoModel(ActionType.FromValue(action.ActionTypeId)));
                }
                else
                {
                    if (!action.IsGranted)
                    {
                        result.Remove(result.Find(f => f.Id == action.ActionTypeId));
                    }
                }
            }




            return result;


        }

        public List<ActionTypeDto> GetUserAuthorizedActionsById(long userId)
        {
           
            var user = _userRepository.GetUserById(userId);
            return GetUserAuthorizedActions(user.PartyName);
        }
        public bool IsAuthorize(ActionEntityTypeEnum workflowEntities, DecisionTypeEnum decisionTypeEnum, ClaimsPrincipal userClaimsPrincipal)
        {
            var methodMapper = new MethodMapper();
            var methodRequiredActions = methodMapper.MapWF(workflowEntities, decisionTypeEnum);
            List<ActionType> userActions = _userActionMapper.MapToEntity(userClaimsPrincipal);
            return _securityApplicationService.IsAuthorize(userActions, methodRequiredActions);
        }
        public bool IsAuthorize(string className, string methodName, ClaimsPrincipal userClaimsPrincipal)
        {
            var methodMapper = new MethodMapper();
            var methodRequiredActions = methodMapper.Map(className, methodName);
            List<ActionType> userActions = _userActionMapper.MapToEntity(userClaimsPrincipal);
            return _securityApplicationService.IsAuthorize(userActions, methodRequiredActions);
        }

        public List<ActionType> GetUserAuthorizedActions(ClaimsPrincipal userClaimsPrincipal)
        {
            return this._securityApplicationService.GetAllAuthorizedActions(_userSecurityMapper.MapToEntity(userClaimsPrincipal));
        }

        public void AddUpdateUser(ClaimsPrincipal userClaimsPrincipal)
        {
            var users = _userMapper.MapToEntity(userClaimsPrincipal);

            User u = users.First();
            _securityApplicationService.AddUpdate(u.UserName, u.FirstName, u.LastName, u.Email);
        }

        public User GetLogonUser()
        {
            throw new NotImplementedException();
        }


        public UserDto GetUser(string userName)
        {

            var user = _userRepository.GetUserById(userName) as User;

            var res = new UserDtoMapper().MapToModel((user != null) ? user : new User());

            return res;
        }

        //public FuelUserDto GetCurrentFuelUser()
        //{
        //    var user = _userRepository.Single(u => u.Id == SecurityApplicationService.GetCurrentUserId() &&
        //        ((User) u).CompanyId == SecurityApplicationService.GetCurrentUserCompanyId()) as User;

        //    var result = new FuelUserDto()
        //                 {
        //                     Id = fuelUserDomainService.GetCurrentFuelUserId(),
        //                     User =  new UserDtoMapper().MapToModel(user)
        //                 };

        //    return result;
        //}
    }
}
