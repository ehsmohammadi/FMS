using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
    public class Issue
    {
        #region Prop
        public long Id { get; set; }

        public long InventoryItemId { get; private set; }
        public long GoodId { get; private set; }

        public string GoodName { get; private set; }

        public string CurrencyName { get; set; }
        public long CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }

        public decimal IssueQuantity { get; private set; }
        public decimal IssueFee { get; private set; }

        
        public decimal Coefficient { get; private set; }


        public DateTime IssueDate { get; set; }  
        public string UnitName { get; set; }

        #endregion

        #region ctor

        public Issue()
        {
            
        }

        public Issue(
            long id,
            long goodId,
            decimal issueQuantity,
            decimal issueFee,
            decimal coefficient, 
            string unitName,
            string goodName,
            DateTime issueDate,
            long currencyId,
            string currencyName,long inventoryItemId)
        {
            Id = id;
            GoodId = goodId;
            IssueFee = issueFee;
            IssueQuantity = issueQuantity;
            Coefficient = coefficient;
            UnitName = unitName;
            GoodName = goodName;
            IssueDate = issueDate;
            CurrencyId = currencyId;
            CurrencyName=currencyName;
            InventoryItemId = inventoryItemId;
        }

        #endregion


    }
}
