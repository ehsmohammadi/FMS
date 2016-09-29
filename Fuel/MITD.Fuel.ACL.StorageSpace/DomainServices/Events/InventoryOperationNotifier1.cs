using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Connectivity;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events
{
    public class InventoryOperationNotifier : IInventoryOperationNotifier
    {
        public List<InventoryOperation> NotifySubmittingFuelReportDetail(FuelReportDetail source)
        {
            return new List<InventoryOperation>(new InventoryOperation[]
                    {
                     new InventoryOperation(
                     "INV# - " +DateTime.Now.Ticks,
                        DateTime.Now,
                        InventoryActionType.Issue,
                        (long? )null,
                        (long? )null)});
        }

        public List<InventoryOperation> NotifySubmittingScrap(Scrap source)
        {
            return new List<InventoryOperation>(new InventoryOperation[]
                    {
                     new InventoryOperation(
                     "INV# - " +DateTime.Now.Ticks,
                        DateTime.Now,
                        InventoryActionType.Issue,
                        (long? )null,
                        (long? )null)});
        }

        public List<InventoryOperation> NotifySubmittingInvoice(Invoice source)
        {
            return new List<InventoryOperation>(new InventoryOperation[]
                    {
                     new InventoryOperation(
                     "INV# - " +DateTime.Now.Ticks,
                        DateTime.Now,
                        InventoryActionType.Issue,
                        (long? )null,
                        (long? )null)});
        }

        public List<InventoryOperation> NotifySubmittingCharterInStart(CharterIn charterInStart)
        {
            return new List<InventoryOperation>(new InventoryOperation[]
                    {
                     new InventoryOperation(
                     "INV# - " +DateTime.Now.Ticks,
                        DateTime.Now,
                        InventoryActionType.Issue,
                        (long? )null,
                        (long? )null)});
        }

        public List<InventoryOperation> NotifySubmittingCharterInEnd(CharterIn charterInEnd)
        {
            return new List<InventoryOperation>(new InventoryOperation[]
                    {
                     new InventoryOperation(
                     "INV# - " +DateTime.Now.Ticks,
                        DateTime.Now,
                        InventoryActionType.Issue,
                        (long? )null,
                        (long? )null)});
        }

        public List<InventoryOperation> NotifySubmittingCharterOutStart(CharterOut charterOutStart)
        {
            return new List<InventoryOperation>(new InventoryOperation[]
                    {
                     new InventoryOperation(
                     "INV# - " +DateTime.Now.Ticks,
                        DateTime.Now,
                        InventoryActionType.Issue,
                        (long? )null,
                        (long? )null)});
        }

        public List<InventoryOperation> NotifySubmittingCharterOutEnd(CharterOut charterOutEnd)
        {
            return new List<InventoryOperation>(new InventoryOperation[]
                    {
                     new InventoryOperation(
                     "INV# - " +DateTime.Now.Ticks,
                        DateTime.Now,
                        InventoryActionType.Issue,
                        (long? )null,
                        (long? )null)});
        }
    }


}
