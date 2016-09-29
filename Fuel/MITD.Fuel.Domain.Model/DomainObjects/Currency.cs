#region

using MITD.Fuel.Domain.Model.Exceptions;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class Currency
    {
        public Currency()
        {
            //To be used as parameter 'TEntity' in the generic type or method 'MITD.DataAccess.EF.EntityTypeConfigurationBase<TEntity>'	                        
        }

        public Currency(
            long id,
            string name,
            string abbreviation,
            string code)
        {
            Id = id;


            if (string.IsNullOrEmpty(abbreviation))
                throw new InvalidArgument("Abbreviation");

            Abbreviation = abbreviation;
            this.Code = code;

            if (string.IsNullOrEmpty(name))
                throw new InvalidArgument("Name");

            Name = name;
        }

        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Abbreviation { get; private set; }
        public string Code { get; private set; }
    }
}
