using System.Linq;
using System.Reflection;
using System.Windows.Media;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    
    public partial class FuelReportDto:ViewModelBase
    {
        public void SetValues(FuelReportDto dto)
        {
            var properties = dto.GetType().GetProperties();

            properties.ToList().ForEach(pi =>{
                if (pi.MemberType == MemberTypes.Property && pi.CanWrite)
                {
                    var destProperty = this.GetType().GetProperty(pi.Name, BindingFlags.Public | BindingFlags.Instance);
                    if (destProperty != null && destProperty.PropertyType.IsAssignableFrom(pi.PropertyType))
                        destProperty.SetValue(this, pi.GetValue(dto, new object[] { }), new object[] { });
                }
            });
        }
    }
}
