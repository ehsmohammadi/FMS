#region

using MITD.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class FuelUser
    {
        public FuelUser()
        {
        }

        public FuelUser(long id, string name, /*long roleId,*/ long companyId)
        {
            Id = id;
            Name = name;
            //RoleId = roleId;
            CompanyId = companyId;
        }

        public long Id { get; set; }

        public long IdentityId { get; set; }

        public string Name { get; set; }

        public long CompanyId { get; set; }

        public bool IsFRApprover { get; set; }

        public static string UserIdToUserName(long? userId)
        {
            return "کاربر " + userId.ToString();
        }

        public virtual Company Company { get; set; }
    }
}