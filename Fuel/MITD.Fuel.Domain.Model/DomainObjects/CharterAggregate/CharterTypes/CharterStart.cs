using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Specifications;
using MITD.Fuel.Domain.Model.Exceptions;

namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.CharterTypes
{
    public class CharterStart<T>: CharterTypeBase<T> 
    {
        internal override void Add(T charter)
        {
            if (charter is CharterIn)
            {
                if (!(new IsCharterInValid().IsSatisfiedBy(charter as CharterIn))) 
                    throw new BusinessRuleException("", "CharterIn is not valid"); 
            }
            else if(charter is CharterOut)
            {
                if (!(new IsCharterOutValid().IsSatisfiedBy(charter as CharterOut))) 
                throw new BusinessRuleException("", "CharterOut is not valid");
                
            }
           
        }

        internal override void Update(T charter)
        {
            if (charter is CharterIn)
            {
                if (!(new IsCharterInValid().IsSatisfiedBy(charter as CharterIn)))
                    throw new BusinessRuleException("", "CharterIn is not valid");
            }
            else if (charter is CharterOut)
            {
                if (!(new IsCharterOutValid().IsSatisfiedBy(charter as CharterOut)))
                    throw new BusinessRuleException("", "CharterOut is not valid");

            }
        }

    }
}
