using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
    public class Receipt
    {
        #region Prop
        public long  Id { get; private set; }
       // public string ReceiptNumber { get; private set; }

        public long GoodId { get; private set; }

        public long InventoryItemId { get; private set; }

        public long CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }
        public string GoodName { get; private set; }
        public decimal ReceiptQuantity { get; private set; }
        public decimal ReceiptFee { get; private set; }

      //  public string ReceiptWarehouseCode { get; private set; }

        public decimal Coefficient { get; private set; }

        public string UnitName { get; set; }

        public string CurrencyName { get; set; }

        public DateTime ReceiptDate { get; set; }   


        #endregion

        #region Ctor

        public Receipt()
        {
                
        }

        public Receipt(
            long id,long goodId,
            decimal receiptQuantity,
            decimal receiptFee,
            decimal coefficient, string unitName, string goodName, DateTime recieptDate, long currencyId, string currencyName,long inventoryItemId)
        {

            Id = id;
            GoodId = goodId;
            ReceiptFee = receiptFee;
            ReceiptQuantity = receiptQuantity;
            CurrencyId = currencyId;
            Coefficient = coefficient;
            UnitName = unitName;
            GoodName = goodName;
            ReceiptDate = recieptDate;
            CurrencyName = currencyName;
            InventoryItemId = inventoryItemId;
        }
        #endregion
    }
}
