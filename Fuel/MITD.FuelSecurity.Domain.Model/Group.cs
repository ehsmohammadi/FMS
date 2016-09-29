using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.FuelSecurity.Domain.Model
{
    public class Group : Party
    {
        public string Description { get; private set; }
        public virtual List<User> Users { get; set; }
        public Group()
            : base()
        {
            Users = new List<User>();
        }

        public Group(long id, string partyName, string description)
            : base(id, partyName)
        {
            this.Description = description;
            Users = new List<User>();
        }

        public virtual void Update(string description, Dictionary<ActionType, bool> customActions)
        {
            this.Description = description;
       // UpdateCustomActions(customActions);
        }
    }
}
