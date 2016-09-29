#region

using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;

#endregion

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class EffectiveFactorDto
    {
        #region props

        private long id;
        private string name;

        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        public string Name
        {
            get { return name; }
            set { this.SetField(p => p.Name, ref name, value); }
        }

        private EffectiveFactorTypeEnum effectiveFactorType;

        public EffectiveFactorTypeEnum EffectiveFactorType
        {
            get { return effectiveFactorType; }
            set { this.SetField(p => p.EffectiveFactorType, ref effectiveFactorType, value); }
        }

        #endregion
    }
}