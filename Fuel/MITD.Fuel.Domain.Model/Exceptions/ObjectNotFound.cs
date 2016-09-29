#region

using System;

#endregion

namespace MITD.Fuel.Domain.Model.Exceptions
{
    public class ObjectNotFound : FuelException
    {
        public string EntityName { get; private set; }
        public long EntityId { get; set; }

        public ObjectNotFound(string entityName)
            : base(string.Format("{0} Object Not Found", entityName))

        {
            EntityName = entityName;
        }

        public ObjectNotFound(string entityName, long entityId)
            : base(string.Format("{0} Object Not Found with Id {1}", entityName, entityId))
        {
            EntityName = entityName;
            EntityId = entityId;
        }
    }
}