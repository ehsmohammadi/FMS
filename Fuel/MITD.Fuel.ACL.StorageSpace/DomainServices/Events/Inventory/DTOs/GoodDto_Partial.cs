using MITD.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.ACL.StorageSpace.InventoryServiceReference
{
    public partial class GoodDto : DTOBase
    {
        private long sharedGoodId;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public long SharedGoodId
        {
            get { return sharedGoodId; }
            set
            {

                sharedGoodId = value;
                this.RaisePropertyChanged("SharedGoodId");
            }
        }
    }
}
