using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.FuelSecurity.Domain.Model.Repository;
using MITD.FuelSecurity.Domain.Model.Service;
using MITD.FuelSecurity.Domain.Model;
using MITD.Fuel.Application.UserManagement;

namespace MITD.Fuel.Application.Service.Security
{
    public class SecurityApplicationService : ISecurityApplicationService
    {

        private IUserRepository _userRepository;
        private ISecurityServiceChecker _securityServiceChecker;
        private readonly IUnitOfWorkScope _unitOfWorkScope;

        #region  ctor

       
        public SecurityApplicationService(IUserRepository userRepository, ISecurityServiceChecker securityServiceChecker,IUnitOfWorkScope unitOfWorkScope)
        {

            this._userRepository = userRepository;
            this._securityServiceChecker = securityServiceChecker;
            _unitOfWorkScope = unitOfWorkScope;
        }

        #endregion

        public bool IsAuthorize(List<ActionType> userActionTypes, List<ActionType> methodRequiredActionTypes)
        {
           
            return _securityServiceChecker.IsAuthorize(userActionTypes, methodRequiredActionTypes);
        }

        public List<ActionType> GetAllAuthorizedActions(List<User> users)
        {
          
            return _securityServiceChecker.GetAllAuthorizedActionTypes(users);
        }

        public User GetUser(long id)
        {
            return _userRepository.GetUserById(id)as User;
        }
        public User GetUser(string partyName)
        {
            return _userRepository.GetUserById(partyName) as User;
        }
        public User GetLogonUser()
        {
            if (ClaimsPrincipal.Current == null)
                return null;
            return GetUser(long.Parse(ClaimsPrincipal.Current.Identity.Name));
        }

        public User AddUser(string firstName, string lastName, bool isActive, string email, Dictionary<int, bool> customActions, List<long> groups,string userName)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var u = new User(0,userName, firstName, lastName, email,isActive, userName);
                    assignCustomActionsToParty(u,customActions);
                    assignUserGroupsToUser(u,groups);
                    _userRepository.Add(u);
                    _unitOfWorkScope.Commit();
                    scope.Complete();
                    return u;

                }
            }
            catch (Exception exp)
            {

                throw;
            }
        }


        public User UpdateUserAccess(long id,Dictionary<int, bool> customActions)
        {
            using (var scope = new TransactionScope())
            {
                 var ums=new UserManagementServiceClient();
                
                var u = _userRepository.GetUserById(id);
                var rols = ums.GetRolesForUser(u.PartyName);
                var roleActions=_securityServiceChecker.GetAllAuthorizedActionTypesForRole(rols.ToList());

                var rep = ServiceLocator.Current.GetInstance<IPartyCustomActionRepository>();
                using (var scope1 = new TransactionScope())
                {
                    rep.DeleteAllByPartyId(id);
                    _unitOfWorkScope.Commit();
                    scope1.Complete();
                }

               

                ((User)u).UpdateCustomActions( customActions, id,  roleActions);
                _unitOfWorkScope.Commit();
                scope.Complete();
                return u as User;
            }
        }

        public User UpdateUser(long companyId,long id, string firstName, string lastName, bool isActive, string email, Dictionary<int, bool> customActions, List<long> groups)
        {
            using (var scope = new TransactionScope())
            {
                var u = _userRepository.GetUserById(id);
                ((User)u).Update(companyId,firstName, lastName, email, isActive);
                _unitOfWorkScope.Commit();
                scope.Complete();
                return u as User;
            }
        }

        public Group UpdateGroup(long id, string description, Dictionary<int, bool> customActions)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var g = _userRepository.GetGroupById(id);
                    g.Update(description, null);
                    _unitOfWorkScope.Commit();
                    scope.Complete();
                    return g as Group;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


      


        public Group AddGroup(long id,string partyName, string description, Dictionary<int, bool> customActions)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var g = new Group(id, partyName, description);
                    assignCustomActionsToParty(g,customActions);
                    _userRepository.Add(g);
                   _unitOfWorkScope.Commit();
                    scope.Complete();
                    return g;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void assignCustomActionsToParty(Party party, Dictionary<int, bool> customActions)
        {
            foreach (var actid in customActions)
            {
                //ActionType act = ActionType.FromValue(actid.Key.ToString());

                //party.AssignCustomActions(act,actid.Value);
            }
        }

        private void assignUserGroupsToUser(User user, List<long> groupIds)
        {
            foreach (var groupId in groupIds)
            {
                var group = _userRepository.GetGroupById(groupId);
                user.AssignGroup(group);
            }
        }

        public static long GetCurrentUserId()
        {
            return long.Parse(ClaimsPrincipal.Current.Claims.First(c => c.Type == "CurrentUserId").Value);
        }

        public static long GetCurrentUserCompanyId()
        {
            return long.Parse(ClaimsPrincipal.Current.Claims.First(c => c.Type == "CurrentUserCompanyId").Value);
        }

        public User AddUpdate(string partyName, string firstName, string lastName, string email)
        {
            User u;
            u = GetUser(partyName) == null ? AddUser(partyName, firstName, lastName, email) : UpdateUser(partyName, firstName, lastName, email);
            return u;
        }
        public User AddUser(string partyName, string firstName, string lastName, string email)
        {
            using (var scope = new TransactionScope())
            {
                var u = new User(0,partyName, firstName, lastName, email, true, partyName);
                _userRepository.Add(u);
                _unitOfWorkScope.Commit();
                scope.Complete();
                return u;
            }
        }
        public User UpdateUser(string partyName, string firstName, string lastName, string email)
        {
            using (var scope = new TransactionScope())
            {
                 var u = _userRepository.GetUserById(partyName);
                ((User)u).Update(firstName, lastName, email);
                _unitOfWorkScope.Commit();
                scope.Complete();
                return u as User;

            
    }
}
    }
}
