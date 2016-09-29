using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.IDomainServices
{

    public interface IUpdateCountSubmitedReciptFactory
    {

    }

    public interface IUpdateCountSubmitedReciptFactory<T, L> : IUpdateCountSubmitedReciptFactory where T : class
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
