using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.FuelSecurity.Domain.Model.ErrorException;
using MITD.FuelSecurity.Domain.Model.Repository;

namespace MITD.FuelSecurity.Domain.Model.Service
{
    public class SecurityServiceChecker : ISecurityServiceChecker
    {

        private readonly IUserRepository _userRepository;

        public SecurityServiceChecker(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<ActionType> GetAllAuthorizedActionTypes(List<User> users)
        {
            if (users == null || users.Count == 0)
                throw new FuelSecurityAccessException(701, "You are not authorize to access to system ");

            var userId = users[0].Id;
            if (users.Any(c=>c.Id!=userId))
            throw new Exception("user name must same");

            var user = _userRepository.GetUserById(users[0].UserName);
            // user = _userRepository.GetUserById(users[0].Id);
            if(user==null)
                throw new NullReferenceException("User");

            var authorizedActionsUser = new List<ActionType>();
            users.ForEach(c=>authorizedActionsUser.AddRange(c.Actions));

         
            user.CustomActions.Where(ca => ca.IsGranted).ToList().ForEach(ca => authorizedActionsUser.Add(ActionType.FromValue(ca.ActionTypeId)));

            user.CustomActions.Where(ca => !ca.IsGranted).ToList().ForEach(ca => authorizedActionsUser.RemoveAll(at=>at.Id == ca.ActionTypeId));

            authorizedActionsUser = authorizedActionsUser.Distinct().ToList();

            return authorizedActionsUser;


        }


        public List<ActionType> GetAllAuthorizedActionTypes(long userId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null)
                throw new NullReferenceException("User");

            var authorizedActionsUser = new List<ActionType>();
            authorizedActionsUser.AddRange(((User) user).Actions);

            user.CustomActions.Where(ca => ca.IsGranted).ToList().ForEach(ca => authorizedActionsUser.Add(ActionType.FromValue(ca.ActionTypeId)));
            user.CustomActions.Where(ca => !ca.IsGranted).ToList().ForEach(ca => authorizedActionsUser.RemoveAll(at => at.Id == ca.ActionTypeId));

            authorizedActionsUser = authorizedActionsUser.Distinct().ToList();

            return authorizedActionsUser;
        }

        public bool IsActionPossible(long userId, ActionType actionType)
        {
            var authorizedActionList = this.GetAllAuthorizedActionTypes(userId);

            return authorizedActionList.Any(a => a.Id == actionType.Id);
        }

        public bool IsAuthorize(List<ActionType> authorizeActions, List<ActionType> actions)
        {
            return actions.All(authorizeActions.Contains);
        }


        public List<ActionType> GetAllAuthorizedActionTypesForRole(List<string> rols)
        {
            var res = new List<ActionType>();
            Type t;
            t = typeof(User);
            foreach (var rol in rols)
            {
                if (rol=="Admin")
                {
                   // t = typeof (AdminUser);
                    foreach (var act in new AdminUser().GetAllActions())
                    {
                        if (res.All(c => c.Id != act.Id))
                            res.Add(act);
                    }

                    
                }

                if (rol == "Commercial")
                {
                  //  t = typeof(CommercialUser);
                    foreach (var act in new CommercialUser().GetAllActions())
                    {
                        if (res.All(c => c.Id != act.Id))
                            res.Add(act);
                    }
                
                }
                if (rol == "CommercialManager")
                {
                    //t = typeof(CommercialManagerUser);
                    foreach (var act in new CommercialManagerUser().GetAllActions())
                    {
                        if (res.All(c => c.Id != act.Id))
                            res.Add(act);
                    }
                  
                }
                if (rol == "Financial")
                {
                    //t = typeof(FinancialUser);
                    foreach (var act in new FinancialUser().GetAllActions())
                    {
                        if (res.All(c => c.Id != act.Id))
                            res.Add(act);
                    }
                    
                }
                if (rol == "FinancialManager")
                {
                  //  t = typeof(FinancialManagerUser);
                    foreach (var act in new FinancialManagerUser().GetAllActions())
                    {
                        if (res.All(c => c.Id != act.Id))
                            res.Add(act);
                    }
                   
                }
                if (rol == "ReadOnly")
                {
                   // t = typeof(ReadOnlyUser);
                    foreach (var act in new ReadOnlyUser().GetAllActions())
                    {
                        if (res.All(c => c.Id != act.Id))
                            res.Add(act);
                    }
                  
                }

                if (rol == "TestRole")
                {
                    // t = typeof(ReadOnlyUser);
                    foreach (var act in new TestRole().GetAllActions())
                    {
                        if (res.All(c => c.Id != act.Id))
                            res.Add(act);
                    }

                }
                //List<ActionType> acts = (List<ActionType>)t.GetMethod("GetAllActions").Invoke(t.GetType(), null);
                //foreach (var act in acts)
                //{
                //    if (res.All(c => c.Id != act.Id))
                //        res.Add(act);
                //}
            }
            return res;
        }
    }
}
