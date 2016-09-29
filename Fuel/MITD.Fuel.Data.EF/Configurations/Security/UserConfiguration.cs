using System.CodeDom;
using MITD.Domain.Model;
using MITD.FuelSecurity.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Data.EF.Configurations.Security
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users");

            Property(p => p.Id).HasColumnName("Id");
            Property(p => p.FirstName);
            Property(p => p.UserName);
            Property(p => p.LastName);
            Property(p => p.Email);
            Property(p => p.Active);
           // Property(p => p.CompanyId);
            Ignore(p => p.Actions);

            HasMany(p => p.Groups).WithMany(g => g.Users)
                .Map(map =>
                    map.ToTable("Users_Groups")
                    .MapLeftKey("UserId")
                    .MapRightKey("GroupId"));
        }
    }
}
