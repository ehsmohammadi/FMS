using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.FuelSecurity.Domain.Model.Repository;

namespace MITD.FuelSecurity.Domain.Model
{
    public class User : Party
    {
        #region Prop
        public  virtual List<ActionType> Actions
        {
            get
            {
                return new List<ActionType>();
            }
        }

        public  long CompanyId { get; private set; }
                            

        public virtual List<Group> Groups { get; set; }

        public virtual string FirstName { get; private set; }

        public virtual string UserName { get; private set; }

        public virtual string LastName { get; private set; }

        public virtual string Email { get; private set; }

        public virtual bool Active { get; private set; }

        #endregion

        #region ctor

        public User()
            : base()
        {
            this.Groups = new List<Group>();
        }

        public User(long id, string partyName, string firstName, string lastName, string email, bool active,string userName)
            : base(id, partyName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Active = active;
            this.UserName = userName;
            this.Groups = new List<Group>();

        }

        public User(long id, string partyName, string firstName, string lastName, string email,string userName)
            : base(id, partyName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Active = true;
            this.UserName = userName;
            this.Groups = new List<Group>();
        }
        #endregion

        #region Method

        public virtual void Update(string firstName, string lastName, string email, bool active,
            Dictionary<int, bool> acions, List<Group> groups,long companyId)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Active = active;
            this.CompanyId = companyId;
            UpdateGroup(groups);
        }
        public virtual void Update(long companyId,string firstName, string lastName, string email, bool active)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Active = active;
            this.CompanyId = companyId;

        }
          //public void UpdateCustomActions(List<PartyCustomAction> originList ,Dictionary<int, bool> acions,long userId,List<ActionType> roleActionTypes)
        public void UpdateCustomActions(Dictionary<int, bool> acions,long userId,List<ActionType> roleActionTypes)
        {

          

            var newList = new List<PartyCustomAction>();
            acions.ToList().ForEach(c =>
            {
                newList.Add(new PartyCustomAction(userId, c.Key, c.Value));
            });

            //roleActionTypes.ForEach(r =>
            //{
            //    var act = newList.Find(c => c.ActionTypeId == r.Id);
            //    if (act!=null && !act.IsGranted)
            //    {
            //        this.CustomActions.Add(act);
            //    }
            //});
          
            newList.ForEach(d =>
            {
                if (!roleActionTypes.Exists(c => c.Id == d.ActionTypeId))
                {
                    this.CustomActions.Add(d);
                }
                else
                {
                    if(!d.IsGranted)
                        this.CustomActions.Add(d);
                }
            });




            //var total = originList.Count;
            //for (int i = 0; i <total ; i++)
            //{
            //    if (!newList.Exists(c => c.ActionTypeId == this.CustomActions[i].ActionTypeId))
            //    {
            //        originList.Remove(this.CustomActions[i]);
            //        total--;
            //    }
            //}

            //newList.ForEach(c =>
            //{

            //    var act = roleActionTypes.Find(d => d.Id == c.ActionTypeId);
            //    if (act==null)
            //    {
            //        originList.Add(c);
            //    }
            //    else
            //    {
            //         var act1 = originList.Find(f => f.ActionTypeId == c.ActionTypeId);
            //         if (act1!=null)
            //         {
            //             act1 = c;
            //         }
            //         else
            //         {
            //             if(!c.IsGranted)
            //                 originList.Add(c);
            //         }

            //    }


            //var act = originList.Find(d => d.ActionTypeId == c.ActionTypeId);
            //if (act==null)
            //{
            //    if (!roleActionTypes.Exists(d=>d.Id==c.ActionTypeId ))
            //    {
            //        originList.Add(c);
            //    }
            //    else
            //    {
            //        act = c;
            //    }

            //}
            //else
            //{
            //    act = c;
            //}
     //   });

           // this.CustomActions = originList;

        }
        public virtual void Update(string firstName, string lastName, string email)
        {
           if(!string.IsNullOrEmpty(firstName))
               this.FirstName = firstName;
           if (!string.IsNullOrEmpty(lastName))
            this.LastName = lastName;
           if (!string.IsNullOrEmpty(email))
            this.Email = email;
       

        }

        public virtual void UpdateGroup(List<Group> groups)
        {
            if (groups == null)
                return;

            foreach (var group in groups)
            {
                AssignGroup(group);
            }


            this.Groups.RemoveAll(localg => groups.Count(gparam => gparam.Id == localg.Id) == 0);
        }

        public virtual void AssignGroup(Group group)
        {
            if (group == null)
                throw new NullReferenceException();


            if (this.Groups.Count(g => g.Id == group.Id) == 0)
                this.Groups.Add(group);
        }

        public virtual void RemoveGroup(Group group)
        {
            this.Groups.RemoveAll(g => g.Id == group.Id);
        }

        public void UpdateCompany(long companyId)
        {
            this.CompanyId = companyId;
        }
        public  IEnumerable<ActionType> GetAllActions()
        {
            return Actions;
        }
        #endregion
    }
}
