using System.Collections.Generic;

namespace MITD.FuelSecurity.Domain.Model
{
    public class IntegrationUser : User
    {


        public IntegrationUser(string firstName, string lastName, string email, string userName)
            : base(0, "IntegrationUser", firstName, lastName, email, userName)
        {

        }

    }
}
