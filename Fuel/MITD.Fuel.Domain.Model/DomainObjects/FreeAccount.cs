using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class FreeAccount
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }

        public virtual List<Segment> Segments { get; set; }
        public FreeAccount()
        {

        }
        public FreeAccount(long id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;

        }
    }
}
