using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;

namespace MITD.FuelSecurity.Domain.Model.Repository
{
    public interface IUserRepository : IRepository<Party>
    {
        IList<Party> GetAllUsers();

        List<User> FindUsers(Expression<Func<User, bool>> predicate, ListFetchStrategy<User> fs, string frname,
            string lsName, string username,int pageSize,int pageIndex);
       
        Party GetUserById(long id);

        User GetUserById(string username);
        IList<Group> GetAllGroups();
        Group GetGroupById(long id);
        void Delete(Party user);
        void Add(Party group);
    //   void Add(Group group);


    }
}
