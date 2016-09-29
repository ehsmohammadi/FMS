using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Presentation.Contracts.Infrastructure;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
   public partial class CharterItemDto
    {                  
        long id;
        
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        long charterId;

        public long CharterId
        {
            get { return charterId; }
            set { this.SetField(p => p.CharterId, ref charterId, value); }
        }

        private GoodDto good;
        public virtual GoodDto Good
        {
            get { return good; }
            set
            {
                this.SetField(p => p.Good, ref good, value);
            }
        }


        private TankDto _tankDto;
        public virtual TankDto TankDto
        {
            get { return _tankDto; }
            set
            {
                this.SetField(p => p.TankDto, ref _tankDto, value);
            }
        }

        private long _tankId;
       public virtual long TankId
        {
            get
            {
                return _tankId;
            }
            set
            {
                this.SetField(c => c.TankId, ref _tankId, value);
            }
        }


       private decimal _rob;
     
       [CustomValidation(typeof(ValidationDto), "IsGreaterZero")]
       public  decimal Rob
       {
           get
           {
               return _rob;
           }
           set
           {
               this.SetField(c => c.Rob, ref _rob, value);
              
           }
       }

       
       private decimal _fee;
       [CustomValidation(typeof(ValidationDto), "IsGreaterZero")]
       public  decimal Fee
       {
           get
           {
               return _fee;
           }
           set
           {
               this.SetField(c => c.Fee, ref _fee, value);
           }
       }

       private decimal _feeOffhire;
      // [CustomValidation(typeof(ValidationDto), "IsGreaterZero")]
       public decimal FeeOffhire
       {
           get
           {
               return _feeOffhire;
           }
           set
           {
               this.SetField(c => c.FeeOffhire, ref _feeOffhire, value);
           }
       }


      

      
    }
}
