using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class OffhireDetailDto
    {
        long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        private decimal quantity;
        //[Required(ErrorMessage = "Quantity must be set.")]
        [Range(typeof(decimal), "0.001", "5000", ErrorMessage = "Quantity value is incorrect.")]
        public decimal Quantity
        {
            get { return quantity; }
            set { this.SetField(p => p.Quantity, ref quantity, value); }
        }

        private decimal? feeInVoucherCurrency;
        //[Required(ErrorMessage = "FeeInVoucherCurrency must be set.")]
        //[Range(typeof(decimal), "0.001", "1000000000000", ErrorMessage = "FeeInVoucherCurrency value is incorrect.")]
        public decimal? FeeInVoucherCurrency
        {
            get { return feeInVoucherCurrency; }
            set { this.SetField(p => p.FeeInVoucherCurrency, ref feeInVoucherCurrency, value); }
        }

        private decimal? feeInMainCurrency;
        //[Required(ErrorMessage = "FeeInMainCurrency must be set.")]
        //[Range(typeof(decimal), "0.001", "1000000000000", ErrorMessage = "FeeInMainCurrency value is incorrect.")]
        public decimal? FeeInMainCurrency
        {
            get { return feeInMainCurrency; }
            set { this.SetField(p => p.FeeInMainCurrency, ref feeInMainCurrency, value); }
        }

        private GoodDto good;
        //[Required(ErrorMessage = "Good must be set.")]
        public GoodDto Good
        {
            get { return good; }
            set { this.SetField(p => p.Good, ref good, value); }
        }

        private GoodUnitDto unit;
        //[Required(ErrorMessage = "Unit must be set.")]
        public GoodUnitDto Unit
        {
            get { return unit; }
            set { this.SetField(p => p.Unit, ref unit, value); }
        }

        private TankDto tank;
        //[Required(ErrorMessage = "Tank must be set.")]
        public TankDto Tank
        {
            get { return tank; }
            set { this.SetField(p => p.Tank, ref tank, value); }
        }

        private OffhireDto offhire;
        public OffhireDto Offhire
        {
            get { return offhire; }
            set { this.SetField(p => p.Offhire, ref offhire, value); }
        }

        private bool isFeeInVoucherCurrencyReadOnly;
        public bool IsFeeInVoucherCurrencyReadOnly
        {
            get { return this.isFeeInVoucherCurrencyReadOnly; }
            set { this.SetField(p => p.IsFeeInVoucherCurrencyReadOnly, ref this.isFeeInVoucherCurrencyReadOnly, value); }
        }
    }
}
