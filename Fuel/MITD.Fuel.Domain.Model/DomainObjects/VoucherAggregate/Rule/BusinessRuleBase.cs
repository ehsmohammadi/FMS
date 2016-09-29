using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate.Rule
{
    public class BusinessRuleBase<T> : IBusinessRule
    {
        private Expression<Func<string, bool>> _expression;
        public T Entity { get; set; }
        public BusinessRuleBase(Expression<Func<string, bool>> expression, T t)
        {
            _expression = expression;
            Entity = t;
        }
        public virtual void Validate(string typeAction)
        {
            throw new NotImplementedException();
        }

        public bool IsValidExpression(string typeAction)
        {
            if (_expression != null)
                return _expression.Compile().Invoke(typeAction);
            else
                return false;
        }
    }
}
