using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
   public interface IUpdatePriceSubmitedReciptFactory
    {
    }

    public interface IUpdatePriceSubmitedReciptFactory<T, L> : IUpdatePriceSubmitedReciptFactory where T : class
    {
        IAutomaticVoucher CreateReciptVoucher();

        IAutomaticVoucher CreateReciptDiffVoucher();

        T GetEntity();
        long GetEntityId();

        decimal GetEntityItemRob();

        string GetOperationReferenceType();
        string GetRevertOperationReferenceType();

        string GetRefreceNumber();
        Good Getgood();

        T GetEntityType();
        L GetEntityItemType();
    }

}
