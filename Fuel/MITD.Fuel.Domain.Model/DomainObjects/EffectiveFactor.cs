using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class EffectiveFactor
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public EffectiveFactorTypes EffectiveFactorType { get; private set; }

        public string VoucherDescription { get; set; }

        public string VoucherRefDescription { get; set; }
        public byte[] TimeStamp { get; private set; }

        public EffectiveFactor()
        {

        }

        public EffectiveFactor(string name, EffectiveFactorTypes effectiveFactorType, string voucherDescription, string voucherRefDescription)
        {
            Name = name;
            EffectiveFactorType = effectiveFactorType;
            VoucherDescription = voucherDescription;
            VoucherRefDescription = voucherRefDescription;
        }

       
        public virtual List<Segment> Segments { get; set; }

        public virtual Account Account { get; set; }
        public  int? AccountId { get; set; }

    }
}