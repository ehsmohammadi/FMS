using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class VoucherEntityDto
    {
        private long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

       
        string _entityTypeName;
        public string EntityTypeName
        {
            get { return _entityTypeName; }
            set { this.SetField(p => p.EntityTypeName, ref _entityTypeName, value); }
        }






    }
}
