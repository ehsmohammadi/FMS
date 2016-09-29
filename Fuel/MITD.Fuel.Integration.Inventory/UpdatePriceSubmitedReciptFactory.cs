using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Enums.Inventory;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Integration.Inventory
{
  public  class UpdatePriceSubmitedReciptFactory : IUpdatePriceSubmitedReciptFactory<CharterIn,CharterItem>
    {

        private CharterIn _charterIn;
        private CharterItem _charterItem;

        public UpdatePriceSubmitedReciptFactory(CharterIn charterIn, CharterItem charterItem)
      {
          _charterIn = charterIn;
          _charterItem = charterItem;
      }

        public IAutomaticVoucher CreateReciptVoucher()
        {
            return ServiceLocator.Current.GetInstance<IAddCharterInStartReceiptVoucher>();
        }

        public IAutomaticVoucher CreateReciptDiffVoucher()
        {
            throw new NotImplementedException();
        }

        public CharterIn GetEntity()
        {
            return _charterIn;
        }

        public long GetEntityId()
        {
            return _charterIn.Id;
        }

        public decimal GetEntityItemRob()
        {
            return _charterItem.Rob;
        }

        public string GetOperationReferenceType()
        {
            if (_charterIn.CharterType == CharterType.Start)
            {
                return InventoryOperationReferenceTypes.CHARTER_IN_START_RECEIPT;

            }
            return null;
        }

        public string GetRevertOperationReferenceType()
        {
            if (_charterIn.CharterType == CharterType.Start)
            {
                return InventoryOperationReferenceTypes.CHARTER_IN_START_ADJUSTMENT_RECEIPT;

            }
            return null;
        }

        public string GetRefreceNumber()
        {
            return _charterIn.Id.ToString();
        }

        public Domain.Model.DomainObjects.Good Getgood()
        {
            return _charterItem.Good;  
        }

        public CharterIn GetEntityType()
        {
            return new CharterIn();
        }

        public CharterItem GetEntityItemType()
        {
            return new CharterItem();
        }
    }
}
