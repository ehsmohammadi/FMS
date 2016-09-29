using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
   public class SegmentType
    {
       public int Id { get; set; }
       public string Name { get; set; }
       public string Code { get; set; }

       public static SegmentType None {
           get { return new SegmentType(0,"None","0"); }
       }
       public static SegmentType Vessel { get { return new SegmentType(1,"Vessel", "1"); } }
       public static SegmentType Port { get { return new SegmentType(2, "Port", "2"); } }
       public static SegmentType Voayage { get { return new SegmentType(3, "Voayage", "3"); } }
       public static SegmentType Company { get { return new SegmentType(4, "Compony", "4"); } }
       public static SegmentType Fix { get { return new SegmentType(5, "Fix", "5"); } }
       public SegmentType()
       {
           
       }
        
       public SegmentType(int id,string name,string code)
       {
           this.Id = id;
           this.Name = name;
           this.Code = code;

       }
       public static SegmentType FindSegmentType(int segmentTypeId)
       {
           var segmentTypes = new List<SegmentType>() {
                SegmentType.None,
                SegmentType.Company,
                SegmentType.Fix,
                SegmentType.Port,
                SegmentType.Vessel
            };

           return segmentTypes.Single(s => s.Id == segmentTypeId);
       }


    }
}
