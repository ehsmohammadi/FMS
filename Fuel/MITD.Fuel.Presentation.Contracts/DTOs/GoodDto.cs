using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Presentation;
namespace MITD.Fuel.Presentation.Contracts.DTOs
{
   public partial class GoodDto
    {
        private long id;
        private string name;
        private string code;

        public long Id
        {
            get { return id; }
            set { this.SetField(P => P.Id, ref id, value); }
        }
        public string Name
        {
            get { return name; }
            set { this.SetField(P => P.Name, ref name, value); }
        }

        public string Code
        {
            get { return code; }
            set { this.SetField(P => P.Code, ref code, value); }
        }

        private GoodUnitDto unit;
        public GoodUnitDto Unit
        {
            get { return unit; }
            set { this.SetField(P => P.Unit, ref unit, value); }
        }

       private List<GoodUnitDto> units;
        //   [Required(AllowEmptyStrings = false, ErrorMessage = "Unit is required")]
        public List<GoodUnitDto> Units
        {
            get { return units; }
            set { this.SetField(p => p.Units, ref units, value); }
        }

        //private List<BrandDto> brands;
        ////   [Required(AllowEmptyStrings = false, ErrorMessage = "Unit is required")]
        //public List<BrandDto> Brands
        //{
        //    get { return brands; }
        //    set { this.SetField(p => p.Brands, ref brands, value); }
        //}
      
    }
}
