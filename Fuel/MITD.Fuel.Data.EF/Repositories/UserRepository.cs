using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MITD.Core;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;

using MITD.FuelSecurity.Domain.Model;
using MITD.FuelSecurity.Domain.Model.Repository;

namespace MITD.Fuel.Data.EF.Repositories
{
    public class UserRepository : EFRepository<Party>, IUserRepository
    {
        private EFUnitOfWork efx;
        private IUnitOfWorkScope _unitOfWorkScope;
        public UserRepository(EFUnitOfWork efUnitOfWork)
            : base(efUnitOfWork)
        {
            efx = efUnitOfWork;
        }

        public UserRepository(IUnitOfWorkScope iUnitOfWorkScope)
            : base(iUnitOfWorkScope)
        {
            _unitOfWorkScope = iUnitOfWorkScope;
        }

        public List<User> FindUsers(Expression<Func<User, bool>> predicate, ListFetchStrategy<User> fs, string frname, string lsName, string username, int pageSize, int pageIndex)
        {

            var q = this.Context.CreateObjectSet<Party>().OfType<User>().Where(c =>

                (c.FirstName.Contains(frname) || string.IsNullOrEmpty(frname)) &&
                (c.LastName.Contains(lsName) || string.IsNullOrEmpty(lsName)) &&
                (c.PartyName.Contains(username) || string.IsNullOrEmpty(username))
                ).OrderBy(c => c.Id).AsQueryable();//.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
               // ).OrderBy(c => c.Id).Skip(fs.PageCriteria.SkipCount).Take(fs.PageCriteria.PageSize).ToList();


            //Where(predicate).OrderBy(c=>c.Id).ToList();
            //  q.Skip(fs.PageCriteria.SkipCount).Take(fs.PageCriteria.PageSize);

            pageIndex = (pageIndex == 0) ? 1 : pageIndex;
            fs.PageCriteria.PageResult.Result = q.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

            fs.PageCriteria.PageResult.TotalCount = q.Count();
            fs.PageCriteria.PageResult.TotalPages = Convert.ToInt32(Math.Ceiling(decimal.Divide(fs.PageCriteria.PageResult.TotalCount, pageSize)));
            fs.PageCriteria.PageResult.CurrentPage = pageIndex;
           


            return fs.PageCriteria.PageResult.Result.ToList();


        }

        public IList<Party> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public void FindUser(System.Linq.Expressions.Expression<Func<Party, bool>> predicatExpression, ListFetchStrategy<Party> fetchStrategy)
        {
            throw new NotImplementedException();
        }

        public Party GetUserById(long id)
        {

            var q = this.Context.CreateObjectSet<Party>().Include("CustomActions").OfType<User>()
                .AsQueryable();
            return q.Single(c => c.Id == id);

        }

        public User GetUserById(string username)
        {
            try
            {
                var q = this.Context.CreateObjectSet<Party>().OfType<User>()
                .AsQueryable();
                return q.SingleOrDefault(c => c.UserName == username ||c.PartyName==username);
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public IList<Group> GetAllGroups()
        {
            throw new NotImplementedException();
        }

        public Group GetGroupById(long id)
        {
            var q = this.Context.CreateObjectSet<Party>().OfType<Group>()
                .AsQueryable();
            return q.Single(c => c.Id == id);
        }

        public void Delete(Party user)
        {
            throw new NotImplementedException();
        }


        public void Add(Group group)
        {
            this.Add(group);

            //var ef = new EFRepository<Group>(_unitOfWorkScope);
            //var x = this.Context.CreateObjectSet<Party>();//.OfType<Group>();
            //x.AddObject(group);
            // ((EFRepository<Group>)x).Add(group);
        }
    }
}
