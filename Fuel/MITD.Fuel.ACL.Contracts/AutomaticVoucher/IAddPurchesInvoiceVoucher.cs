using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.ACL.Contracts.AutomaticVoucher
{
   public interface IAddPurchesInvoiceVoucher:IAutomaticVoucher
   {
       void Execute(Invoice invoice, List<Receipt> receipts, string receiptwarehousecode, decimal aditionalCoeff, FuelReport fuelReport,string inventoryActionNumber, long userId);
   }
}
