
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using Castle.Core;
using Castle.Core.Internal;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Fuel.Application.Facade.Mappers;
using MITD.FuelSecurity.Domain.Model.Service;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;
using MITD.FuelSecurity.Domain.Model.Repository;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Application.Service.Security;
using MITD.FuelSecurity.Domain.Model;
using Omu.ValueInjecter;
using MITD.Fuel.Application.UserManagement;

namespace MITD.Fuel.Application.Facade
{
     [Interceptor(typeof(SecurityInterception))]
    public class UserFacadeService : IUserFacadeService
    {
        private readonly IFuelUserRepository fuelUserRepository;
        private readonly IFacadeMapper<FuelUser, UserDto> _mapper;
        private readonly IFacadeMapper<Company, CompanyDto> _companyMapper;
        private readonly IFacadeMapper<Group, UserGroupDescriptionDto> _mapperGroup;
        private IMapper<User, UserDTOWithActions> _userDTOWithActionsMapper;
        private IFacadeMapper<User, UserDto> _userDTOMapper;
        private IUserRepository userRepository;
        private readonly IFuelUserDomainService fuelUserDomainService;
        private ICompanyRepository _companyRepository;
        private IFacadeMapper<Group, UserGroupDtoWithActions> _userGroupDTOWithActionsMapper;
        private ISecurityApplicationService _securityApplicationService;
        private IFacadeMapper<Group, UserGroupDto> _userGroupDTOMapper;
        private IActionToDtoMapper _actionTypeDTOMapper;
        private ISecurityServiceChecker _securityServiceChecker;
        #region props

        #endregion

        #region ctor

        public UserFacadeService(IFuelUserRepository fuelUserRepository,
            IFacadeMapper<User, UserDto> userDTOMapper,
            IFacadeMapper<FuelUser, UserDto> mapper,
            IFacadeMapper<Company, CompanyDto> companyMapper,
            IFacadeMapper<Group, UserGroupDescriptionDto> mapperGroup,
            IUserRepository userRepository,
            IMapper<User, UserDTOWithActions> userDTOWithActionsMapper,
            IFuelUserDomainService fuelUserDomainService, ICompanyRepository companyRepository,
            IFacadeMapper<Group, UserGroupDtoWithActions> userGroupDTOWithActionsMapper,
            ISecurityApplicationService securityApplicationService,
            IFacadeMapper<Group, UserGroupDto> userGroupDTOMapper,
           IActionToDtoMapper actionTypeDTOMapper,
            ISecurityServiceChecker securityServiceChecker)
        {
            this.fuelUserRepository = fuelUserRepository;
            this.fuelUserDomainService = fuelUserDomainService;
            _mapper = mapper;
            _companyMapper = companyMapper;
            _userDTOMapper = userDTOMapper;
            this.userRepository = userRepository;
            _companyRepository = companyRepository;
            _mapperGroup = mapperGroup;
            _userDTOWithActionsMapper = userDTOWithActionsMapper;
            _userGroupDTOWithActionsMapper = userGroupDTOWithActionsMapper;
            _securityApplicationService = securityApplicationService;
            _userGroupDTOMapper = userGroupDTOMapper;
            _actionTypeDTOMapper = actionTypeDTOMapper;
            this._securityServiceChecker = securityServiceChecker;
        }

        #endregion

        #region methods

        public void Add(UserDto data)
        {
            throw new NotImplementedException();


            _securityApplicationService.AddUser(
              data.FirstName,
              data.LastName,
              data.Active,
              data.Email,
              data.CustomActions,
              data.Groups.Select(c => c.Id).ToList(),
              data.UserName);


        }

        public void Update(UserDto data)
        {
            _securityApplicationService.UpdateUser(data.CompanyDto.Id,data.Id, data.FirstName, data.LastName, data.Active, data.Email,
                new Dictionary<int, bool>(), new List<long>());

        }

        public void UpdateUserAccess(long userId,Dictionary<int, bool> actionTyps)
        {
            _securityApplicationService.UpdateUserAccess(userId,actionTyps);

        }

        public void Delete(UserDto data)
        {
            throw new NotImplementedException();

        }

        public UserDto GetUserWithCompany(int id)
        {
            var fetch = new SingleResultFetchStrategy<FuelUser>().Include(c => c.Company);

            var ent = fuelUserRepository.Single(c => c.Id == id, fetch);

            var ou = _mapper.MapToModel(ent);
            ou.CompanyDto = _companyMapper.MapToModel(ent.Company);
            return ou;
        }

        public UserDto GetById(int id)
        {

            var ent = fuelUserRepository.FindByKey(id);

            var ou = _mapper.MapToModel(ent);

            return ou;
        }

        public PageResultDto<UserDto> GetAll(int pageSize, int pageIndex)
        {
            var fetch = new ListFetchStrategy<FuelUser>().WithPaging(pageSize, pageIndex);

            fuelUserRepository.GetAll(fetch);

            var finalResult = new PageResultDto<UserDto>
                                  {
                                      CurrentPage = pageIndex,
                                      PageSize = pageSize,
                                      Result = _mapper.MapToModel(fetch.PageCriteria.PageResult.Result).ToList(),
                                      TotalCount = fetch.PageCriteria.PageResult.TotalCount,
                                      TotalPages = fetch.PageCriteria.PageResult.TotalPages
                                  };

            foreach (var user in finalResult.Result)
                user.Code = user.FirstName;

            return finalResult;

        }

        public List<UserDto> GetAll()
        {
            var fetch = new ListFetchStrategy<FuelUser>();

            var result = _mapper.MapToModel(fuelUserRepository.GetAll()).ToList();

            foreach (var user in result)
                user.Code = user.FirstName;

            return result;

        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();

        }

        public UserStateDTO GetUserState(ClaimsPrincipal u)
        {
            throw new NotImplementedException();

            //if (u == null)
            //    throw new NullReferenceException("Principal is null");
            //var fmsUsers = _userMapper.MapToEntity(u);
            //if (fmsUsers == null || fmsUsers.Count == 0)
            //    throw new Exception("pmsUsers is null or pmsUsers.count is zero");

            //var userState = _userStateMapper.MapToModel(u);
            //return userState;
        }

        #endregion


        public UserDto GetUserByUserName(string username)
        {
            var user = userRepository.GetUserById(username);
            var userDto = _userDTOMapper.MapToModel(user);
            userDto.CompanyDto=new CompanyDto(){Id=user.CompanyId};
            userDto.Groups = user.Groups.Select(g => _mapperGroup.MapToModel(g)).ToList();
            return userDto;
        }

        public UserGroupDto GetUserGroupByName(string name)
        {
            var userGroups = userRepository.GetAll().OfType<Group>().Single(c => c.PartyName == name);
            return _userGroupDTOMapper.MapToModel(userGroups);
        }

        public UserGroupDto AddUserGroup(UserGroupDto userGroupDto)
        {
            Group u = _securityApplicationService.AddGroup(0,userGroupDto.PartyName, userGroupDto.Description, userGroupDto.CustomActions);
            return _userGroupDTOMapper.MapToModel(u);
        }

        public UserGroupDto UpdateUserGroup(UserGroupDto userGroupDto)
        {
            Group u = _securityApplicationService.UpdateGroup(userGroupDto.Id, userGroupDto.Description, userGroupDto.CustomActions);
            return _userGroupDTOMapper.MapToModel(u);
        }

        public string DeleteUserGroup(string name)
        {
            throw new NotImplementedException();
        }

        public List<ActionTypeDto> GetAllActionTypes()
        {
            var actionTypes = ActionType.GetAllActions();
            var res = actionTypes.Select(p => _actionTypeDTOMapper.MapToDtoModel(p)).ToList();
            return res;
        }
        public List<ActionTypeDto> GetAllUserActionTypes(long userId)
        {
            var actionTypes = _securityServiceChecker.GetAllAuthorizedActionTypes(new List<User>()
            {
                new User(userId,"","","","",true,"")
                
            });
            var res = actionTypes.Select(p => _actionTypeDTOMapper.MapToModel(p)).ToList();
            return res;
        }

        public FuelUserDto GetCurrentFuelUser()
        {
            var currentUserId = SecurityApplicationService.GetCurrentUserId();
            var currentCompanyId = SecurityApplicationService.GetCurrentUserCompanyId();
            var company = _companyRepository.Find(c => c.Id == currentCompanyId).First();


            var user = userRepository
                .Find(u => u.Id == currentUserId)
                .Cast<User>().Single(
                    u => ((User)u).CompanyId == currentCompanyId);



            var result = new FuelUserDto()
            {
                Id = fuelUserDomainService.GetCurrentFuelUserId(),
                User = new UserDtoMapper().MapToModel(user),

            };

            result.User.CompanyDto = new CompanyDto()
                                   {
                                       Id = currentCompanyId,
                                       Name = company.Name,
                                       Code = company.Code
                                   };

            return result;
        }

        public long GetCurrentUserId()
        {
            var currentUserId = SecurityApplicationService.GetCurrentUserId();
            return currentUserId;
        }

        public long GetCurrentFuelUserId()
        {
            var currentFuelUserId = fuelUserDomainService.GetCurrentFuelUserId();
            return currentFuelUserId;
        }

        public long GetCurrentFuelUserCompanyId()
        {
            var currentCompanyId = SecurityApplicationService.GetCurrentUserCompanyId();
            return currentCompanyId;
        }

        public bool GetCurrentFuelUserAccessToHolding()
        {
            var currentCompanyId = SecurityApplicationService.GetCurrentUserCompanyId();
            return currentCompanyId == 1;
        }

        public PageResultDto<UserDTOWithActions> GetAllUsers(int pageSize, int pageIndex, string filter)
        {
            var repUser = ServiceLocator.Current.GetInstance<IUserRepository>();
            var fs = new ListFetchStrategy<User>(Enums.FetchInUnitOfWorkOption.NoTracking);
            fs.WithPaging(pageSize, pageIndex);
            //  fs.OrderBy(e => e.LastName);
            var criterias = filter.Split(';');
            var predicate = getUserPredicate(criterias);
            var rs = getPredicate(criterias);
            if (criterias.Count() > 1)
            {//criterias.ToList()
                repUser.FindUsers(predicate, fs, rs[0], rs[1], rs[2],pageSize,pageIndex);
            }
            else
            {
                repUser.FindUsers(predicate, fs, "", "", "", pageSize, pageIndex);
            }
            var res = new PageResultDto<UserDTOWithActions>();
            res.InjectFrom(fs.PageCriteria.PageResult);
            res.Result = new List<UserDTOWithActions>();
                fs.PageCriteria.PageResult.Result.ForEach(p =>
                {
                   var u= _userDTOWithActionsMapper.MapToModel(p);
                    u.CompanyDto=new CompanyDto(){Id=p.CompanyId};
                    res.Result.Add(u);

                });
            return res;

        }
        private Expression<Func<User, bool>> getUserPredicate(IEnumerable<string> criterias)
        {
            Expression<Func<User, bool>> res = user => true;

            foreach (var criteria in criterias)
            {
                var sp = criteria.Split(':');
                if (sp[0] == "FirstName" && !string.IsNullOrEmpty(sp[1]))
                    res = res.And(e => e.FirstName.Contains(sp[1]));
                if (sp[0] == "LastName" && !string.IsNullOrEmpty(sp[1]))
                    res = res.And(e => e.LastName.Contains(sp[1]));
                if (sp[0] == "PartyName" && !string.IsNullOrEmpty(sp[1]))
                    res = res.And(e => e.PartyName.Contains(sp[1]));
            }
            return res;
        }

        private List<string> getPredicate(IEnumerable<string> criterias)
        {
            var res=new List<string>();
            var fname = "";
            var lname = "";
            var uname = "";
            foreach (var criteria in criterias)
            {
                var sp = criteria.Split(':');
                if (sp[0] == "FirstName" && !string.IsNullOrEmpty(sp[1]))
                    fname=sp[1];
                else if (sp[0] == "LastName" && !string.IsNullOrEmpty(sp[1]))
                   lname=sp[1];
                else  if (sp[0] == "PartyName" && !string.IsNullOrEmpty(sp[1]))
                    uname=sp[1];
               
            }

            res.Add(fname);
            res.Add(lname);
            res.Add(uname);
            return res;
        }


        public List<UserGroupDtoWithActions> GetAllUserGroups()
        {

            var userGroups = userRepository.GetAll().OfType<Group>();
            return userGroups.Select(p => _userGroupDTOWithActionsMapper.MapToModel(p)).ToList();
        }
        public List<ActionTypeDto> GetGroupActionType(long groupId)
        {
            var res = new List<ActionTypeDto>();
            var userGroup = userRepository.GetAll().OfType<Group>().SingleOrDefault(d => d.Id == groupId);
            if (userGroup != null)
                userGroup.CustomActions.ForEach(c => res.Add(_actionTypeDTOMapper.MapToDtoModel(c.ActionType)));
            return res;
        }

         public void ChangePassWord(string newPassWord, string oldPassWord)
         {
             ServicePointManager.ServerCertificateValidationCallback

                += (sender, certificate, chain, errors) => true;
             var usm = new UserManagementServiceClient();
             usm.SetPassword(ClaimsPrincipal.Current.Claims.First(c => c.Type == "CurrentUsername").Value, newPassWord, oldPassWord);
         }
    }
}
