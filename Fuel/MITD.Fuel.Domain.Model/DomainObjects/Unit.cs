#region

using System.Collections.Generic;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class Unit
    {
        public Unit()
        {
        }

        public Unit(long id, string name)
        {
            Id = id;
            Name = name;

        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }
}