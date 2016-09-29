using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Presentation.Contracts.Infrastructure
{
    public class ValidationDto //: System.ComponentModel.DataAnnotations.ValidationAttribute
    {
        //public override bool IsValid(object value)
        //{
        //    return base.IsValid(value);
        //}
        
       public static ValidationResult IsGreaterZero(double d)
       {
           return (d > 0 ? null : new ValidationResult("مقدار ورودی می بایست بزگتر از صفر باشد"));

       }

   

       public static ValidationResult IsComboSelected(long d)
       {
           return (d > 0 ? null : new ValidationResult("مقدار لیست باید انتخاب گردد"));

       }
    }
}
