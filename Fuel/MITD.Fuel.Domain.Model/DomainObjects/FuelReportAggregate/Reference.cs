using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class Reference
    {
        public static Reference Empty
        {
            get { return new Reference(); }
        }

        private ReferenceType? referenceType;

        public ReferenceType? ReferenceType
        {
            get { return referenceType; }
            set { referenceType = value; }
        }

        private long? referenceId;

        public long? ReferenceId
        {
            get { return referenceId; }
            set { referenceId = value; }
        }

        private string code;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public bool IsEmpty()
        {
            return !referenceId.HasValue || !referenceType.HasValue || string.IsNullOrWhiteSpace(code);
        }
    }

}
