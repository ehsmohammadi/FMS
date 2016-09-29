using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.FuelSecurity.Domain.Model
{
    public class TestRole : User
    {
        public override List<ActionType> Actions
        {
            get
            {
                return new List<ActionType>()
    {
                       
                       ActionType.AddCharterIn,
                        ActionType.EditCharterIn,
                         ActionType.AddCharterOut,
                          ActionType.EditCharterOut,
                           ActionType.AddCharterInItem,
                            ActionType.EditCharterInItem,
                             ActionType.AddCharterOutItem,
                              ActionType.EditCharterOutItem,
                              ActionType.ChangePassWord

                   };
            }
        }

        public TestRole()
        {

        }
        public TestRole(long id, string firstName, string lastName, string email, string userName)
            : base(id, "TestRole", firstName, lastName, email, userName)
        {

        }

    }
}
