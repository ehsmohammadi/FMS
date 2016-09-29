using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using MITD.Core;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.Factories;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;
using MITD.Fuel.Domain.Model.ReportObjects;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Domain.Model.Specifications;

namespace MITD.Fuel.Domain.Model.DomainServices
{
    public class FuelReportDomainService : IFuelReportDomainService
    {
        private readonly IFuelReportRepository fuelReportRepository;
        private readonly IRepository<FuelReportDetail> fuelReportDetailRepository;
        private readonly IOrderDomainService orderDomainService;
        private readonly IInventoryManagementDomainService inventoryManagementDomainService;
        private readonly ICharteringDomainService charteringDomainService;
        private readonly IVoyageDomainService voyageDomainService;
        private readonly IInventoryOperationDomainService inventoryOperationDomainService;
        private readonly IInventoryOperationRepository inventoryOperationRepository;
        private readonly IInventoryOperationFactory inventoryOperationFactory;

        private readonly IsFuelReportIssued isFuelReportIssued;
        private readonly IsFuelReportClosed isFuelReportClosed;
        private readonly IsFuelReportOperational isFuelReportOperational;
        private readonly IsFuelReportNotCancelled isFuelReportNotCancelled;
        private readonly IsFuelReportSubmitted isFuelReportSubmitted;
        private readonly IGoodUnitConvertorDomainService goodUnitConvertorDomainService;

        private readonly IRepository<VesselEventReportsView> vesselEventReportsViewRep;

        private IWorkflowLogRepository workflowLogRepository;

        public FuelReportDomainService(
            IFuelReportRepository fuelReportRepository,
            IVoyageDomainService voyageDomainService,
            IInventoryOperationDomainService inventoryOperationDomainService,
            IInventoryOperationRepository inventoryOperationRepository,
            IInventoryOperationFactory inventoryOperationFactory,
            IOrderDomainService orderDomainService,
            IInventoryManagementDomainService inventoryManagementDomainService,
            ICharteringDomainService charteringDomainService,
            IRepository<FuelReportDetail> fuelReportDetailRepository,
            IVoyageRepository voyageRepository,
            IGoodUnitConvertorDomainService goodUnitConvertorDomainService,
             IRepository<VesselEventReportsView> vesselEventReportsViewRep, IWorkflowLogRepository workflowLogRepository)
        {
            this.fuelReportRepository = fuelReportRepository;
            this.voyageDomainService = voyageDomainService;
            this.inventoryOperationDomainService = inventoryOperationDomainService;
            this.inventoryOperationRepository = inventoryOperationRepository;
            this.inventoryOperationFactory = inventoryOperationFactory;
            this.orderDomainService = orderDomainService;
            this.inventoryManagementDomainService = inventoryManagementDomainService;
            this.charteringDomainService = charteringDomainService;
            this.fuelReportDetailRepository = fuelReportDetailRepository;
            this.goodUnitConvertorDomainService = goodUnitConvertorDomainService;

            this.isFuelReportNotCancelled = new IsFuelReportNotCancelled();
            this.isFuelReportIssued = new IsFuelReportIssued(inventoryOperationDomainService);
            this.isFuelReportClosed = new IsFuelReportClosed();
            this.isFuelReportOperational = new IsFuelReportOperational();
            this.isFuelReportSubmitted = new IsFuelReportSubmitted();

            this.vesselEventReportsViewRep = vesselEventReportsViewRep;
            this.workflowLogRepository = workflowLogRepository;
        }

        //================================================================================

        public FuelReport Get(long id)
        {
            var fetchStrategy = new SingleResultFetchStrategy<FuelReport>()
                .Include(fr => fr.FuelReportDetails)
                .Include(fr => fr.ApproveWorkFlows)
                .Include(fr => fr.ConsumptionInventoryOperations)
                .Include(fr => fr.ActivationCharterContract)
                .Include(fr => fr.VesselInCompany)
                .Include(fr => fr.Voyage);


            return this.fuelReportRepository.First(
                MITD.Domain.Model.Extensions.And(this.isFuelReportNotCancelled.Predicate, fr => fr.Id == id),
                fetchStrategy);
        }

        //================================================================================

        public List<FuelReport> GetAll()
        {
            return this.fuelReportRepository.Find(this.isFuelReportNotCancelled.Predicate).ToList();
        }

        //================================================================================

        public PageResult<FuelReport> GetByFilter(long? companyId, long? vesselInCompanyId, string vesselReportCode, DateTime? fromDate, DateTime? toDate, string fuelReportIds, string fuelReportDetailIds, int pageSize, int pageIndex)
        {
            var fetchStrategy = new ListFetchStrategy<FuelReport>()
                .WithPaging(pageSize, pageIndex + 1)
                .Include(fr => fr.FuelReportDetails)
                .Include(fr => fr.FuelReportDetails.First().CorrectionPriceCurrency)
                .Include(fr => fr.FuelReportDetails.First().Good)
                .Include(fr => fr.FuelReportDetails.First().InventoryOperations)
                .Include(fr => fr.FuelReportDetails.First().MeasuringUnit)
                .Include(fr => fr.FuelReportDetails.First().Tank)
                .Include(fr => fr.ApproveWorkFlows)
                .Include(fr => fr.ConsumptionInventoryOperations)
                .Include(fr => fr.ActivationCharterContract)
                .Include(fr => fr.VesselInCompany)
                .Include(fr => fr.Voyage)
                .OrderByDescending(c => c.EventDate);

            var fuelReportIdsList = string.IsNullOrWhiteSpace(fuelReportIds) ? new List<long>() : fuelReportIds.Split(',').Select(long.Parse).ToList();
            var fuelReportDetailIdsList = string.IsNullOrWhiteSpace(fuelReportDetailIds) ? new List<long>() : fuelReportDetailIds.Split(',').Select(long.Parse).ToList();

            var data = this.fuelReportRepository.Find(
                    f =>
                        (vesselInCompanyId == null || f.VesselInCompanyId == vesselInCompanyId) &&
                        (companyId == null || f.VesselInCompany.CompanyId == companyId) &&
                        (string.IsNullOrEmpty(vesselReportCode) || f.Code == vesselReportCode) &&
                        (fromDate == null || f.EventDate >= fromDate) &&
                        (toDate == null || f.EventDate <= toDate) &&
                        (!fuelReportIdsList.Any() || fuelReportIdsList.Contains(f.Id)) &&
                        (!fuelReportDetailIdsList.Any() || f.FuelReportDetails.Any(frd => fuelReportDetailIdsList.Contains(frd.Id))), fetchStrategy);

            return fetchStrategy.PageCriteria.PageResult;
        }

        //================================================================================

        public FuelReportDetail GetFuelReportDetailById(long id, long detailId)
        {
            return this.fuelReportDetailRepository.Single(frd => frd.Id == detailId);
        }

        //================================================================================

        public List<FuelReport> GetPreviousNotFinalApprovedReports(FuelReport fuelReport)
        {
            var fetchStrategy = new ListFetchStrategy<FuelReport>()
                .Include(fr => fr.FuelReportDetails)
                .Include(fr => fr.ApproveWorkFlows)
                .Include(fr => fr.ConsumptionInventoryOperations)
                .Include(fr => fr.ActivationCharterContract)
                .Include(fr => fr.VesselInCompany)
                .Include(fr => fr.Voyage)
                .OrderBy(c => c.EventDate);

            var result = this.fuelReportRepository.Find(
                MITD.Domain.Model.Extensions.And(
                    MITD.Domain.Model.Extensions.And(
                        MITD.Domain.Model.Extensions.Not(this.isFuelReportClosed.Predicate), this.isFuelReportOperational.Predicate),
                        f => f.EventDate < fuelReport.EventDate && f.VesselInCompanyId == fuelReport.VesselInCompanyId),
                fetchStrategy);

            return result.ToList();
        }

        //================================================================================

        public FuelReport GetPreviousFuelReport(long currentFuelReportId)
        {
            var fetchStrategy = new SingleResultFetchStrategy<FuelReport>()
                .Include(fr => fr.FuelReportDetails)
                .Include(fr => fr.ApproveWorkFlows)
                .Include(fr => fr.ConsumptionInventoryOperations)
                .Include(fr => fr.ActivationCharterContract)
                .Include(fr => fr.VesselInCompany)
                .Include(fr => fr.Voyage)
                .OrderByDescending(c => c.EventDate);

            var currentFuelReport = this.fuelReportRepository.Single(fr => fr.Id == currentFuelReportId);

            if (currentFuelReport == null) return null;

            var result = this.fuelReportRepository.First(
                                    MITD.Domain.Model.Extensions.And(this.isFuelReportNotCancelled.Predicate, f => f.EventDate < currentFuelReport.EventDate && f.VesselInCompanyId == currentFuelReport.VesselInCompanyId), fetchStrategy);

            return result;
        }

        //================================================================================

        public FuelReportDetail GetPreviousFuelReportDetail(long currentFuelReportId, long currentFuelReportDetailId)
        {
            var previousFuelReport = this.GetPreviousFuelReport(currentFuelReportId);

            if (previousFuelReport == null) return null;

            var currentFuelReportDetail = this.fuelReportDetailRepository.Single(frd => frd.Id == currentFuelReportDetailId);

            if (currentFuelReportDetail == null) return null;

            var previousFuelReportDetail = previousFuelReport.FuelReportDetails.SingleOrDefault(frd => frd.GoodId == currentFuelReportDetail.GoodId);

            return previousFuelReportDetail;
        }

        //================================================================================

        public FuelReportDetail GetPreviousFuelReportDetailByGood(long currentFuelReportId, long goodId)
        {
            var previousFuelReport = this.GetPreviousFuelReport(currentFuelReportId);

            if (previousFuelReport == null) return null;

            var previousFuelReportDetail = previousFuelReport.FuelReportDetails.SingleOrDefault(frd => frd.GoodId == goodId);

            return previousFuelReportDetail;
        }

        //================================================================================

        public List<FuelReport> GetVoyagesValidEndOfVoyageFuelReports(List<Voyage> voyages)
        {
            var result = this.fuelReportRepository
                .Find(MITD.Domain.Model.Extensions.And(this.isFuelReportNotCancelled.Predicate,
                    fr => fr.FuelReportType == FuelReportTypes.EndOfVoyage))
                .Join(voyages, report => report.VoyageId, v => v.Id, (fr, v) => fr);
            //.Where(fr => voyages.Any(v => v.Id == fr.VoyageId));

            return result.ToList();
        }

        //================================================================================

        public FuelReport GetVoyageValidEndOfVoyageFuelReport(long voyageId)
        {
            var endOfVoyageFuelReport = this.fuelReportRepository.First(
                MITD.Domain.Model.Extensions.And(this.isFuelReportNotCancelled.Predicate, fr => fr.FuelReportType == FuelReportTypes.EndOfVoyage &&
                        voyageId == fr.VoyageId));

            if (endOfVoyageFuelReport == null)
                throw new ObjectNotFound("Voyage EOV FuelReport", voyageId);

            return endOfVoyageFuelReport;
        }

        //================================================================================

        public FuelReport GetVoyageValidEndOfVoyageFuelReport(Voyage voyage)
        {
            if (voyage == null) return null;

            return this.GetVoyageValidEndOfVoyageFuelReport(voyage.Id);
        }

        //================================================================================

        public List<FuelReport> GetCompanyVesselFuelReports(long vesselInCompanyId, DateTime? startDateTime, DateTime? endDateTime, bool ignoreTime = false)
        {
            var startDateToSearch = DateTime.MinValue;
            if (startDateTime.HasValue)
            {
                if (ignoreTime)
                {
                    startDateToSearch = startDateTime.Value.Date;
                }
                else
                {
                    startDateToSearch = startDateTime.Value;
                }
            }


            var endDateToSearch = DateTime.MaxValue;
            if (endDateTime.HasValue)
            {
                if (ignoreTime)
                {
                    endDateToSearch = endDateTime.Value.Date.AddDays(1).AddMilliseconds(-1);
                }
                else
                {
                    endDateToSearch = endDateTime.Value;
                }
            }

            var result = this.fuelReportRepository.Find(
                MITD.Domain.Model.Extensions.And(this.isFuelReportNotCancelled.Predicate, fr =>
                    fr.VesselInCompanyId == vesselInCompanyId &&
                    fr.EventDate >= startDateToSearch &&
                    fr.EventDate <= endDateToSearch));

            return result.ToList();
        }

        private long getCompanyVesselFuelReportsCounts(long vesselInCompanyId, DateTime? startDateTime, DateTime? endDateTime, bool ignoreTime = false)
        {
            var startDateToSearch = DateTime.MinValue;
            if (startDateTime.HasValue)
            {
                if (ignoreTime)
                {
                    startDateToSearch = startDateTime.Value.Date;
                }
                else
                {
                    startDateToSearch = startDateTime.Value;
                }
            }


            var endDateToSearch = DateTime.MaxValue;
            if (endDateTime.HasValue)
            {
                if (ignoreTime)
                {
                    endDateToSearch = endDateTime.Value.Date.AddDays(1).AddMilliseconds(-1);
                }
                else
                {
                    endDateToSearch = endDateTime.Value;
                }
            }

            var result = this.fuelReportRepository.Count(
                MITD.Domain.Model.Extensions.And(this.isFuelReportNotCancelled.Predicate, fr =>
                    fr.VesselInCompanyId == vesselInCompanyId &&
                    fr.EventDate >= startDateToSearch &&
                    fr.EventDate <= endDateToSearch));
            return result;
        }

        public List<Reference> GetFuelReportDetailInternalTransferTransferReferences(FuelReportDetail fuelReportDetail)
        {
            var result = new List<Reference>();

            var finalApprovedInternalTransferOrder = this.orderDomainService.GetFinalApprovedInternalTransferOrders(fuelReportDetail.FuelReport.VesselInCompany.CompanyId);

            result.AddRange(
                finalApprovedInternalTransferOrder.Where(o =>
                    fuelReportDetail.Transfer.HasValue &&  //This is chacked by assuming that the transfer will not be updated through UI.
                    o.OrderDate.Date <= fuelReportDetail.FuelReport.EventDate.Date &&
                    o.OrderItems.Any(oi => oi.GoodId == fuelReportDetail.GoodId) &&
                    o.OrderItems.Any(oi => oi.IsValidToTransfer(fuelReportDetail.Transfer.Value, fuelReportDetail.MeasuringUnitId, this.goodUnitConvertorDomainService))).Select(
                o => new Reference
                {
                    ReferenceType = ReferenceType.Order,
                    ReferenceId = o.Id,
                    Code = o.Code
                }));

            return result;
        }

        public List<Reference> GetFuelReportDetailSaleTransferTransferReferences(FuelReportDetail fuelReportDetail)
        {
            var result = new List<Reference>();

            var finalApprovedTransferOrder = this.orderDomainService.GetFinalApprovedTransferOrders(fuelReportDetail.FuelReport.VesselInCompany.CompanyId);

            result.AddRange(
                finalApprovedTransferOrder.Where(o =>
                    (fuelReportDetail.Transfer.HasValue) &&  //This is chacked by assuming that the transfer will not be updated through UI.
                    o.OrderDate.Date <= fuelReportDetail.FuelReport.EventDate.Date &&
                    o.OrderItems.Any(oi => oi.GoodId == fuelReportDetail.GoodId) &&
                    o.OrderItems.Any(oi => oi.IsValidToReceive(fuelReportDetail.Transfer.GetValueOrDefault(0), fuelReportDetail.MeasuringUnitId, this.goodUnitConvertorDomainService))).Select(
                    o => new Reference
                    {
                        ReferenceType = ReferenceType.Order,
                        ReferenceId = o.Id,
                        Code = o.Code
                    }));

            return result;
        }

        public List<Reference> GetFuelReportDetailInternalTransferReceiveReferences(FuelReportDetail fuelReportDetail, bool isTrustIssueQuantityAssignmentPossible, decimal? trustQuantity)
        {
            var result = new List<Reference>();

            var finalApprovedInternalTransferOrders = orderDomainService.GetFinalApprovedInternalTransferOrders(fuelReportDetail.FuelReport.VesselInCompany.CompanyId);

            result.AddRange(
               finalApprovedInternalTransferOrders.Where(o =>
                    (fuelReportDetail.Receive.HasValue || isTrustIssueQuantityAssignmentPossible) &&  //This is chacked by assuming that the receive will not be updated through UI.
                    o.OrderDate.Date <= fuelReportDetail.FuelReport.EventDate.Date &&
                    o.OrderItems.Any(oi => oi.GoodId == fuelReportDetail.GoodId) &&
                    o.OrderItems.Any(oi => oi.IsValidToReceive(fuelReportDetail.Receive.GetValueOrDefault(trustQuantity.GetValueOrDefault(0)), fuelReportDetail.MeasuringUnitId, this.goodUnitConvertorDomainService))).Select(
               o => new Reference
               {
                   ReferenceType = ReferenceType.Order,
                   ReferenceId = o.Id,
                   Code = o.Code
               }));

            return result;
        }

        public List<Reference> GetFuelReportDetailTransferPurchaseReceiveReferences(FuelReportDetail fuelReportDetail, bool isTrustIssueQuantityAssignmentPossible, decimal? trustQuantity)
        {
            var result = new List<Reference>();

            var finalApprovedPrurchaseTransferOrders = this.orderDomainService.GetBuyerFinalApprovedPurchaseTransferOrders(fuelReportDetail.FuelReport.VesselInCompany.CompanyId);

            result.AddRange(
               finalApprovedPrurchaseTransferOrders.Where(o =>
                    (fuelReportDetail.Receive.HasValue || isTrustIssueQuantityAssignmentPossible) &&  //This is chacked by assuming that the receive will not be updated through UI.
                    o.OrderDate.Date <= fuelReportDetail.FuelReport.EventDate.Date &&
                    o.OrderItems.Any(oi => oi.GoodId == fuelReportDetail.GoodId) &&
                    o.OrderItems.Any(oi => oi.IsValidToReceive(fuelReportDetail.Receive.GetValueOrDefault(trustQuantity.GetValueOrDefault(0)), fuelReportDetail.MeasuringUnitId, this.goodUnitConvertorDomainService))).Select(
               o => new Reference
               {
                   ReferenceType = ReferenceType.Order,
                   ReferenceId = o.Id,
                   Code = o.Code
               }));

            return result;
        }

        public List<Reference> GetFuelReportDetailPurchaseReceiveReferences(FuelReportDetail fuelReportDetail, bool isTrustIssueQuantityAssignmentPossible, decimal? trustQuantity)
        {
            var result = new List<Reference>();

            var finalApprovedPrurchaseOrders = this.orderDomainService.GetFinalApprovedPrurchaseOrders(fuelReportDetail.FuelReport.VesselInCompany.CompanyId, fuelReportDetail.FuelReport.VesselInCompanyId);

            result.AddRange(
                finalApprovedPrurchaseOrders.Where(o =>
                    (fuelReportDetail.Receive.HasValue || isTrustIssueQuantityAssignmentPossible) &&  //This is chacked by assuming that the receive will not be updated through UI.
                    o.OrderDate.Date <= fuelReportDetail.FuelReport.EventDate.Date &&
                    o.OrderItems.Any(oi => oi.GoodId == fuelReportDetail.GoodId) &&
                    o.OrderItems.Any(oi => oi.IsValidToReceive(fuelReportDetail.Receive.GetValueOrDefault(trustQuantity.GetValueOrDefault(0)), fuelReportDetail.MeasuringUnitId, this.goodUnitConvertorDomainService))).Select(
                o => new Reference
                {
                    ReferenceType = ReferenceType.Order,
                    ReferenceId = o.Id,
                    Code = o.Code
                }));

            return result;
        }

        //================================================================================

        public List<Reference> GetFuelReportDetailTransferReferences(FuelReportDetail fuelReportDetail)
        {
            List<Reference> result = null;

            //if (fuelReportDetail.TransferType.HasValue)
            //{
            //    switch (fuelReportDetail.TransferType.Value)
            //    {
            //        case TransferTypes.TransferSale:

            //            var finalApprovedTransferOrder = orderDomainService.GetFinalApprovedTransferOrders(fuelReportDetail.FuelReport.Vessel.CompanyId);

            //            result = new List<Reference>(
            //                finalApprovedTransferOrder.Select(
            //                    o => new Reference
            //                        {
            //                            ReferenceType = ReferenceType.Order,
            //                            ReferenceId = o.Id,
            //                            Code = o.Code
            //                        }));
            //            break;

            //        case TransferTypes.InternalTransfer:

            //            var finalApprovedInternalTransferOrder = orderDomainService.GetFinalApprovedInternalTransferOrders(fuelReportDetail.FuelReport.Vessel.CompanyId);

            //            result = new List<Reference>(
            //                finalApprovedInternalTransferOrder.Select(
            //                o => new Reference
            //                    {
            //                        ReferenceType = ReferenceType.Order,
            //                        ReferenceId = o.Id,
            //                        Code = o.Code
            //                    }));

            //            break;
            //    }
            //}


            result = new List<Reference>();

            var finalApprovedTransferOrder = this.orderDomainService.GetFinalApprovedTransferOrders(fuelReportDetail.FuelReport.VesselInCompany.CompanyId);

            result.AddRange(
                finalApprovedTransferOrder.Where(o => o.OrderDate.Date <= fuelReportDetail.FuelReport.EventDate.Date).Select(
                    o => new Reference
                    {
                        ReferenceType = ReferenceType.Order,
                        ReferenceId = o.Id,
                        Code = o.Code
                    }));

            var finalApprovedInternalTransferOrder = this.orderDomainService.GetFinalApprovedInternalTransferOrders(fuelReportDetail.FuelReport.VesselInCompany.CompanyId);

            result.AddRange(
                finalApprovedInternalTransferOrder.Where(o => o.OrderDate.Date <= fuelReportDetail.FuelReport.EventDate.Date).Select(
                o => new Reference
                {
                    ReferenceType = ReferenceType.Order,
                    ReferenceId = o.Id,
                    Code = o.Code
                }));

            return result;
        }

        //================================================================================

        public List<Reference> GetFuelReportDetailReceiveReferences(FuelReportDetail fuelReportDetail)
        {
            List<Reference> result = null;

            //if (fuelReportDetail.ReceiveType.HasValue)
            //{
            //    switch (fuelReportDetail.ReceiveType.Value)
            //    {
            //        case ReceiveTypes.Purchase:

            //            var finalApprovedPrurchaseOrders = orderDomainService.GetFinalApprovedPrurchaseOrders(fuelReportDetail.FuelReport.Vessel.CompanyId);

            //            result = new List<Reference>(
            //                finalApprovedPrurchaseOrders.Select(
            //                o => new Reference
            //                {
            //                    ReferenceType = ReferenceType.Order,
            //                    ReferenceId = o.Id,
            //                    Code = o.Code
            //                }));

            //            break;

            //        case ReceiveTypes.TransferPurchase:

            //            var finalApprovedPrurchaseTransferOrders = orderDomainService.GetBuyerFinalApprovedPurchaseTransferOrders(fuelReportDetail.FuelReport.Vessel.CompanyId);

            //            result = new List<Reference>(
            //               finalApprovedPrurchaseTransferOrders.Select(
            //               o => new Reference
            //               {
            //                   ReferenceType = ReferenceType.Order,
            //                   ReferenceId = o.Id,
            //                   Code = o.Code
            //               }));

            //            break;

            //        case ReceiveTypes.InternalTransfer:

            //            var finalApprovedInternalTransferOrders = orderDomainService.GetFinalApprovedInternalTransferOrders(fuelReportDetail.FuelReport.Vessel.CompanyId);

            //            result = new List<Reference>(
            //               finalApprovedInternalTransferOrders.Select(
            //               o => new Reference
            //               {
            //                   ReferenceType = ReferenceType.Order,
            //                   ReferenceId = o.Id,
            //                   Code = o.Code
            //               }));

            //            break;
            //    }
            //}


            result = new List<Reference>();

            var finalApprovedPrurchaseOrders = this.orderDomainService.GetFinalApprovedPrurchaseOrders(fuelReportDetail.FuelReport.VesselInCompany.CompanyId, fuelReportDetail.FuelReport.VesselInCompanyId);

            result.AddRange(
                finalApprovedPrurchaseOrders.Where(o => o.OrderDate.Date <= fuelReportDetail.FuelReport.EventDate.Date).Select(
                o => new Reference
                {
                    ReferenceType = ReferenceType.Order,
                    ReferenceId = o.Id,
                    Code = o.Code
                }));

            var finalApprovedPrurchaseTransferOrders = this.orderDomainService.GetBuyerFinalApprovedPurchaseTransferOrders(fuelReportDetail.FuelReport.VesselInCompany.CompanyId);

            result.AddRange(
               finalApprovedPrurchaseTransferOrders.Where(o => o.OrderDate.Date <= fuelReportDetail.FuelReport.EventDate.Date).Select(
               o => new Reference
               {
                   ReferenceType = ReferenceType.Order,
                   ReferenceId = o.Id,
                   Code = o.Code
               }));

            var finalApprovedInternalTransferOrders = this.orderDomainService.GetFinalApprovedInternalTransferOrders(fuelReportDetail.FuelReport.VesselInCompany.CompanyId);

            result.AddRange(
               finalApprovedInternalTransferOrders.Where(o => o.OrderDate.Date <= fuelReportDetail.FuelReport.EventDate.Date).Select(
               o => new Reference
               {
                   ReferenceType = ReferenceType.Order,
                   ReferenceId = o.Id,
                   Code = o.Code
               }));

            return result;
        }

        //================================================================================

        public List<Reference> GetFuelReportDetailCorrectionReferences(FuelReportDetail fuelReportDetail)
        {
            List<Reference> result = new List<Reference>();

            var lastIssuedEOVFuelReportsOfPreviousVoyages = this.GetLastIssuedEOVFuelReportOfPreviousVoyages(fuelReportDetail.FuelReport);

            if (lastIssuedEOVFuelReportsOfPreviousVoyages != null)
            {
                result.Add(
                    new Reference()
                    {
                        ReferenceType = ReferenceType.Voyage,
                        ReferenceId = lastIssuedEOVFuelReportsOfPreviousVoyages.Voyage.Id,
                        Code = lastIssuedEOVFuelReportsOfPreviousVoyages.Voyage.VoyageNumber
                    });
            }

            return result;
        }

        //================================================================================

        public FuelReport GetLastIssuedEOVFuelReportOfPreviousVoyages(FuelReport fuelReport)
        {
            return this.GetLastIssuedEOVFuelReportOfPreviousVoyages(fuelReport.VesselInCompanyId, fuelReport.EventDate);
        }

        public FuelReport GetLastIssuedEOVFuelReportOfPreviousVoyages(long vesselInCompanyId, DateTime dateTimeToCompare)
        {

            var previousEOVFuelReports = this.getPreviousEOVFuelReports(vesselInCompanyId, dateTimeToCompare);

            var lastIssuedEOVFuelReportOfPreviousVoyages =
                previousEOVFuelReports
                //.FindAll(eovfr => eovfr.ConsumptionInventoryOperations != null && eovfr.ConsumptionInventoryOperations.Count > 0)
                    .OrderBy(eovfr => eovfr.EventDate)
                    .LastOrDefault(eovfr => eovfr.ConsumptionInventoryOperations != null && eovfr.ConsumptionInventoryOperations.Count > 0);

            return lastIssuedEOVFuelReportOfPreviousVoyages;
        }

        //================================================================================

        public List<FuelReport> GetNotIssuedEOVFuelReportsOfPreviousVoyages(FuelReport fuelReport)
        {
            var previousEOVFuelReports = this.getPreviousEOVFuelReports(fuelReport.VesselInCompanyId, fuelReport.EventDate);

            var notIssuedEOVFuelReportsOfPreviousVoyages =
                previousEOVFuelReports
                    .FindAll(eovfr => eovfr.ConsumptionInventoryOperations == null || eovfr.ConsumptionInventoryOperations.Count == 0);

            return notIssuedEOVFuelReportsOfPreviousVoyages;
        }

        //================================================================================

        private List<FuelReport> getPreviousEOVFuelReports(long vesselInCompanyId, DateTime dateTimeToCompare)
        {
            //var voyagesEndedBefore = voyageDomainService.GetVoyagesEndedBefore(vesselInCompanyId, dateTimeToCompare);

            //var endOfVoyageFuelReportsOfPreviousVoyages = this.GetVoyagesValidEndOfVoyageFuelReports(voyagesEndedBefore);
            //return endOfVoyageFuelReportsOfPreviousVoyages.Where(fr => fr.EventDate <= dateTimeToCompare).ToList();

            var endOfVoyageFuelReportsOfPreviousVoyages = this.fuelReportRepository.Find(MITD.Domain.Model.Extensions.And(this.isFuelReportNotCancelled.Predicate,
                fr => fr.VesselInCompanyId == vesselInCompanyId && fr.FuelReportType == FuelReportTypes.EndOfVoyage && fr.EventDate < dateTimeToCompare));

            return endOfVoyageFuelReportsOfPreviousVoyages.ToList();
        }

        //================================================================================

        public List<FuelReport> GetFuelReportsFromYesterday(FuelReport fuelReport)
        {
            DateTime previousDayStart = fuelReport.EventDate.Date.AddDays(-1);

            return this.GetCompanyVesselFuelReports(fuelReport.VesselInCompanyId, previousDayStart, fuelReport.EventDate.AddMilliseconds(-1));
        }

        //================================================================================

        public void SetFuelReportInventoryResults(InventoryResultCommand resultCommand, FuelReport fuelReport)
        {
            foreach (var item in resultCommand.Items)
            {
                var fuelReportDetail = fuelReport.FuelReportDetails.FirstOrDefault(frd => frd.Id == item.FuelReportDetailId);
                if (fuelReportDetail == null)
                {
                    throw new ObjectNotFound("resultBag.Item.DetailId");
                }

                var inventoryOperationToPersist = this.inventoryOperationFactory.Create(fuelReportDetail, item.OperationId, item.ActionNumber, item.ActionType, item.ActionDate);

                this.inventoryOperationRepository.Add(inventoryOperationToPersist);

                //if (fuelReportDetail.TransferReferenceOrderId.HasValue)
                //{
                //     orderDomainService.CloseOrder(fuelReportDetail.TransferReferenceOrderId.Value);
                //}

                //if (fuelReportDetail.ReceiveReferenceOrderId.HasValue)
                //{
                //      orderDomainService.CloseOrder(fuelReportDetail.ReceiveReferenceOrderId.Value);
                //}
            }

            //IFuelReportStateFactory stateFactory = ServiceLocator.Current.GetInstance<IFuelReportStateFactory>();

            //fuelReport.Close(stateFactory.CreateClosedState());
        }

        //================================================================================

        //Commented due to be unusable
        //public void InvalidateAllPreviousEndOfVoyageFuelReports(FuelReport fuelReport)
        //{
        //    if (fuelReport.FuelReportType != FuelReportTypes.EndOfVoyage)
        //        throw new ArgumentException("FuelReport is not of Type EndOfVoyage.");

        //    var previousFuelReports = this.fuelReportRepository.Find(
        //                                    Extensions.And(this.isFuelReportOperational.Predicate, fr =>
        //                                            fr.FuelReportType == Enums.FuelReportTypes.EndOfVoyage &&
        //                                            fr.ReportDate < fuelReport.ReportDate &&
        //                                            fr.VoyageId == fuelReport.VoyageId)
        //        );

        //    IFuelReportStateFactory stateFactory = ServiceLocator.Current.GetInstance<IFuelReportStateFactory>();

        //    foreach (var previousFuelReport in previousFuelReports)
        //    {
        //        previousFuelReport.Invalidate(stateFactory.CreateInvalidState());
        //    }
        //}

        //================================================================================

        public List<FuelReport> GetVoyageAllEndOfVoyageFuelReport(Voyage voyage)
        {
            return this.fuelReportRepository.Find(
                fr =>
                    fr.FuelReportType == FuelReportTypes.EndOfVoyage &&
                    voyage.Id == fr.VoyageId
                ).ToList();
        }

        //================================================================================

        public InventoryOperation GetVoyageConsumptionIssueOperation(long voyageId)
        {
            var endOfVoyageFuelReport = this.GetVoyageValidEndOfVoyageFuelReport(voyageId);

            var endOfVoyageInventoryOperation = this.inventoryOperationDomainService.GetEndOfVoyageFuelReportConsumptionInventoryOperation(endOfVoyageFuelReport);

            if (endOfVoyageInventoryOperation == null)
                throw new ObjectNotFound("VoyageConsumption Inventory Operation");

            return endOfVoyageInventoryOperation;
        }

        //================================================================================

        public ChangingFuelReportDateData GetChangingFuelReportDateData(long fuelReportId, DateTime newDateTime)
        {
            var data = new ChangingFuelReportDateData();

            var changingFuelReport = this.Get(fuelReportId);
            data.ChangingFuelReport = changingFuelReport;

            var ascendingFetchStrategy = new SingleResultFetchStrategy<FuelReport>()
                .Include(fr => fr.FuelReportDetails)
                .Include(fr => fr.ApproveWorkFlows)
                .Include(fr => fr.ConsumptionInventoryOperations)
                .Include(fr => fr.ActivationCharterContract)
                .Include(fr => fr.VesselInCompany)
                .Include(fr => fr.Voyage)
                .OrderBy(fr => fr.EventDate);

            var nextFuelReportBeforeChangingDate = this.fuelReportRepository.First(
                        MITD.Domain.Model.Extensions.And(this.isFuelReportNotCancelled.Predicate,
                        fr => fr.VesselInCompanyId == changingFuelReport.VesselInCompanyId &&
                            fr.EventDate > changingFuelReport.EventDate),
                        ascendingFetchStrategy);

            if (nextFuelReportBeforeChangingDate != null)
                data.NextFuelReportBeforeChangeDate = this.Get(nextFuelReportBeforeChangingDate.Id);

            var nextFuelReportAfterChangingDate = this.fuelReportRepository.First(
                        MITD.Domain.Model.Extensions.And(this.isFuelReportNotCancelled.Predicate,
                        fr => fr.VesselInCompanyId == changingFuelReport.VesselInCompanyId &&
                             fr.EventDate > newDateTime),
                        ascendingFetchStrategy);

            if (nextFuelReportAfterChangingDate != null)
                if ((nextFuelReportBeforeChangingDate == null) || (nextFuelReportAfterChangingDate.Id != nextFuelReportBeforeChangingDate.Id))
                    //Here, is the chance of more than one fuel report to be affected.
                    data.NextFuelReportAfterChangeDate = this.Get(nextFuelReportAfterChangingDate.Id);

            return data;
        }

        //================================================================================

        public List<Reference> GetFuelReportDetailRejectedTransferReferences(FuelReportDetail fuelReportDetail)
        {
            var previousPurchaseFuelReportDetails = this.fuelReportDetailRepository.Find(
                    frd =>
                        frd.FuelReport.VesselInCompanyId == fuelReportDetail.FuelReport.VesselInCompanyId &&
                        frd.FuelReport.EventDate < fuelReportDetail.FuelReport.EventDate &&
                        frd.GoodId == fuelReportDetail.GoodId &&
                        frd.ReceiveType.HasValue && frd.ReceiveType.Value == ReceiveTypes.Purchase
                ).ToList();

            return this.inventoryManagementDomainService.GetFueReportDetailsReceiveOperationReference(previousPurchaseFuelReportDetails);
        }

        //================================================================================

        public decimal CalculateReportingConsumption(FuelReportDetail fuelReportDetail)
        {
            var processingFuelReport = fuelReportDetail.FuelReport;

            if (!(/*processingFuelReport.FuelReportType == FuelReportTypes.EndOfMonth ||
                   * processingFuelReport.FuelReportType == FuelReportTypes.EndOfYear*/
                processingFuelReport.FuelReportType == FuelReportTypes.EndOfVoyage ||
                processingFuelReport.IsEndOfYearReport()
                ))
            {
                throw new BusinessRuleException("", "FuelReport is not of type EOV, EOM or EOY.");
            }

            Charter activationCharterContract = this.charteringDomainService.GetVesselActivationCharterContract(processingFuelReport.VesselInCompany, processingFuelReport.EventDate);

            var startDateOfVesselActivation = activationCharterContract == null ? DateTime.MinValue : activationCharterContract.ActionDate;

            var singleFetchStrategy = new SingleResultFetchStrategy<FuelReport>()
                .Include(fr => fr.FuelReportDetails)
                .Include(fr => fr.VesselInCompany)
                .Include(fr => fr.Voyage)
                .OrderByDescending(fr => fr.EventDate);

            var lastConsumptionIssuingFuelReport = this.fuelReportRepository.First(
                MITD.Domain.Model.Extensions.And(this.isFuelReportNotCancelled.Predicate,
                    fr =>
                        fr.EventDate >= startDateOfVesselActivation &&
                        fr.EventDate < processingFuelReport.EventDate &&
                        fr.VesselInCompany.Id == processingFuelReport.VesselInCompanyId &&
                            (fr.FuelReportType == FuelReportTypes.EndOfVoyage ||
                                (fr.FuelReportType == FuelReportTypes.NoonReport &&
                                    fr.EventDate.Month == 3 &&
                                    fr.EventDate.Day == 21 &&
                                    fr.EventDate.Hour == 12 &&
                                    fr.EventDate.Minute == 0))),
                    singleFetchStrategy);

            if (lastConsumptionIssuingFuelReport != null && !this.isFuelReportIssued.IsSatisfiedBy(lastConsumptionIssuingFuelReport))
                throw new BusinessRuleException("", "Previous EOV, EOM or EOY FuelReport is not issued yet.");

            var startDateToCalculateConsumption = (lastConsumptionIssuingFuelReport != null)
                ? lastConsumptionIssuingFuelReport.EventDate
                : startDateOfVesselActivation;

            Expression<Func<FuelReport, bool>> queryToFindPreviousFuelReportsForCalculation =
                MITD.Domain.Model.Extensions.And(this.isFuelReportNotCancelled.Predicate,
                fr =>
                    fr.VesselInCompanyId == processingFuelReport.VesselInCompanyId &&
                    fr.EventDate > startDateToCalculateConsumption &&
                        fr.EventDate < processingFuelReport.EventDate);


            var notSubmittedFuelReportsCount = this.fuelReportRepository.Count(
                    MITD.Domain.Model.Extensions.And(MITD.Domain.Model.Extensions.Not(this.isFuelReportSubmitted.Predicate),
                        queryToFindPreviousFuelReportsForCalculation));

            if (notSubmittedFuelReportsCount != 0)
                throw new BusinessRuleException("", "There are some not approved FuelReports.");

            var calculatedPreviousConsumptions = this.fuelReportRepository.Find(queryToFindPreviousFuelReportsForCalculation)
                    .SelectMany(fr => fr.FuelReportDetails.Where(frd => frd.GoodId == fuelReportDetail.GoodId))
                    .Sum(frd => frd.Consumption);

            return calculatedPreviousConsumptions + fuelReportDetail.Consumption;
        }

        //================================================================================

        public FuelReportDetail GetLastPurchasingReceiveFuelReportDetailBefore(FuelReportDetail fuelReportDetail)
        {
            var fetchStrategy = new SingleResultFetchStrategy<FuelReport>()
                .Include(fr => fr.FuelReportDetails)
                .Include(fr => fr.ApproveWorkFlows)
                .Include(fr => fr.ConsumptionInventoryOperations)
                .Include(fr => fr.ActivationCharterContract)
                .Include(fr => fr.VesselInCompany)
                .Include(fr => fr.Voyage)
                .OrderByDescending(fr => fr.EventDate);

            var lastFuelReportWithReceivedGood = this.fuelReportRepository.First(
                MITD.Domain.Model.Extensions.And(this.isFuelReportSubmitted.Predicate,
                MITD.Domain.Model.Extensions.And(this.isFuelReportNotCancelled.Predicate,
                    fr =>
                            fr.EventDate < fuelReportDetail.FuelReport.EventDate &&
                            fr.VesselInCompanyId == fuelReportDetail.FuelReport.VesselInCompanyId &&
                            fr.FuelReportDetails.Any(
                                frd =>
                                    frd.GoodId == fuelReportDetail.GoodId &&
                                    frd.Receive.HasValue &&
                                    frd.ReceiveType.HasValue &&
                                    (frd.ReceiveType.Value == ReceiveTypes.Purchase ||
                                    frd.ReceiveType.Value == ReceiveTypes.TransferPurchase)))),
                fetchStrategy);

            return lastFuelReportWithReceivedGood == null ? null : lastFuelReportWithReceivedGood.FuelReportDetails.Single(frd => frd.GoodId == fuelReportDetail.GoodId);
        }

        //================================================================================

        public CharterPreparedData PrepareCharterData(long fuelReportId)
        {
            var fuelReport = this.fuelReportRepository.Single(fr => fr.Id == fuelReportId);

            if (fuelReport.FuelReportType != FuelReportTypes.CharterInEnd &&
                fuelReport.FuelReportType != FuelReportTypes.CharterOutStart)
                throw new InvalidArgument("The given FuelReport is not of Correct Type.", "fuelReport");

            var preparedData = new CharterPreparedData();

            if (fuelReport.FuelReportType == FuelReportTypes.CharterInEnd)
            {
                preparedData.CharteringType = CharteringType.In;
                preparedData.CharterState = CharterType.End;

                preparedData.EndDate = fuelReport.EventDate;
                preparedData.Charterer = fuelReport.VesselInCompany.Company;
                preparedData.VesselInCompany = fuelReport.VesselInCompany;
                preparedData.Owner = fuelReport.VesselInCompany.Vessel.Owner;
            }
            else
            {
                preparedData.CharteringType = CharteringType.Out;
                preparedData.CharterState = CharterType.Start;

                preparedData.StartDate = fuelReport.EventDate;
                preparedData.Owner = fuelReport.VesselInCompany.Company;
                preparedData.VesselInCompany = fuelReport.VesselInCompany;
            }

            preparedData.CharterItems = new ObservableCollection<CharterPreparedDataItem>(
                                    fuelReport.FuelReportDetails.Select(frd => new CharterPreparedDataItem()
                                                     {
                                                         Good = frd.Good,
                                                         Rob = frd.ROB,
                                                         Tank = frd.Tank,
                                                         TankId = frd.TankId
                                                     }));


            return preparedData;
        }

        //================================================================================

        public Charter GetVesselActivationCharterContract(FuelReport processingFuelReport)
        {
            return this.charteringDomainService.GetVesselActivationCharterContract(processingFuelReport.VesselInCompany, processingFuelReport.EventDate);
        }

        //================================================================================

        public List<FuelReport> GetVesselFuelReports(string vesselCode, DateTime? startDateTime, DateTime? endDateTime, bool ignoreTime = false)
        {
            var startDateToSearch = DateTime.MinValue;
            if (startDateTime.HasValue)
            {
                if (ignoreTime)
                {
                    startDateToSearch = startDateTime.Value.Date;
                }
                else
                {
                    startDateToSearch = startDateTime.Value;
                }
            }


            var endDateToSearch = DateTime.MaxValue;
            if (endDateTime.HasValue)
            {
                if (ignoreTime)
                {
                    endDateToSearch = endDateTime.Value.Date.AddDays(1).AddMilliseconds(-1);
                }
                else
                {
                    endDateToSearch = endDateTime.Value;
                }
            }

            var result = this.fuelReportRepository.Find(
                 fr =>
                    fr.VesselInCompany.Vessel.Code == vesselCode &&
                    fr.EventDate >= startDateToSearch &&
                    fr.EventDate <= endDateToSearch).OrderBy(fr => fr.EventDate).ToList();

            return result;
        }

        //================================================================================

        public void MoveFuelReportsToCompany(string vesselCode, long destinationCompanyId, DateTime fromDateTime, DateTime? toDateTime, long userId)
        {
            var companyDomainService = ServiceLocator.Current.GetInstance<ICompanyDomainService>();

            var foundCompany = companyDomainService.Get(destinationCompanyId);

            if (foundCompany == null)
                throw new ObjectNotFound("Destination Company", destinationCompanyId);

            var fuelReportsToMove = this.GetVesselFuelReports(vesselCode, fromDateTime, toDateTime, false);

            if (fuelReportsToMove.Any(fr => fr.State == States.Submitted && fr.VesselInCompany.CompanyId != destinationCompanyId))
            {
                throw new BusinessRuleException("", string.Format("There are some submitted FuelReports in the original company between '{0}' and '{1}'.", fuelReportsToMove.Min(fr => fr.EventDate).ToString("yyyy/MM/dd HH:mm"), fuelReportsToMove.Max(fr => fr.EventDate).ToString("yyyy/MM/dd HH:mm")));
            }

            IVesselInCompanyDomainService vesselInCompanyDomainService = ServiceLocator.Current.GetInstance<IVesselInCompanyDomainService>();
            IGoodDomainService goodDomainService = ServiceLocator.Current.GetInstance<IGoodDomainService>();

            IWorkflowStepRepository workflowRepository = ServiceLocator.Current.GetInstance<IWorkflowStepRepository>();

            var initWorkflowStep = workflowRepository.Single(c => c.Workflow.WorkflowEntity == WorkflowEntities.FuelReport &&
                 c.Workflow.CompanyId == destinationCompanyId && c.CurrentWorkflowStage == WorkflowStages.Initial &&
                c.Workflow.Name == Workflow.DEFAULT_NAME);

            if (initWorkflowStep == null)
                throw new ObjectNotFound("FuelReportInitialStep");

            var fuelReportStateFactory = ServiceLocator.Current.GetInstance<IFuelReportStateFactory>();


            var destinationVesselInCompany = vesselInCompanyDomainService.GetVesselInCompany(destinationCompanyId, vesselCode);

            if (destinationVesselInCompany == null)
                throw new ObjectNotFound("No Vessel found with Code " + vesselCode + " in Company " + foundCompany.Name);

            var vesselInCompanyVoyages = this.voyageDomainService.GetByFilter(foundCompany.Id, destinationVesselInCompany.Id);


            fuelReportsToMove.Where(fr => fr.VesselInCompany.CompanyId != destinationCompanyId).ToList().ForEach(fr =>
            {
                fr.ResetStateToInitial(fuelReportStateFactory, initWorkflowStep, userId);

                fr.ChangeCompany(foundCompany, destinationVesselInCompany, goodDomainService);

                try
                {
                    var voyage = vesselInCompanyVoyages.First(v => v.StartDate <= fr.EventDate &&
                        fr.EventDate <= (v.EndDate.HasValue ? v.EndDate : DateTime.MaxValue));

                    if (voyage != null)
                        fr.UpdateVoyageId(voyage.Id, this.voyageDomainService);
                }
                catch
                {
                }
            });

            if (fuelReportsToMove.Count > 0)
                fuelReportsToMove.First().UpdateFirstPositionStatus(this);
        }

        //================================================================================

        public void ChangeFuelReportsStateForCharteredOutVessel(VesselInCompany vesselInCompany, DateTime fromDateTime, DateTime? toDateTime, long userId)
        {
            var fuelReportsToChange = this.GetCompanyVesselFuelReports(vesselInCompany.Id, fromDateTime, toDateTime);

            if (fuelReportsToChange.Any(fr => fr.State == States.Submitted && fr.VesselInCompanyId == vesselInCompany.Id))
            {
                throw new BusinessRuleException("", "There are some submitted FuelReports in the original company.");
            }

            var fuelReportStateFactory = ServiceLocator.Current.GetInstance<IFuelReportStateFactory>();

            var vesselInCompanyVoyages = this.voyageDomainService.GetByFilter(vesselInCompany.CompanyId, vesselInCompany.Id);

            IWorkflowStepRepository workflowRepository = ServiceLocator.Current.GetInstance<IWorkflowStepRepository>();

            fuelReportsToChange.ForEach(fr =>
            {
                fr.ResetStateToInvalid(fuelReportStateFactory, workflowRepository, userId);

                try
                {
                    var voyage = vesselInCompanyVoyages.First(v => v.StartDate <= fr.EventDate &&
                        fr.EventDate <= (v.EndDate.HasValue ? v.EndDate : DateTime.MaxValue));

                    if (voyage != null)
                        fr.UpdateVoyageId(voyage.Id, this.voyageDomainService);
                }
                catch
                {
                }
            });
        }

        //================================================================================

        public void ManageFuelReportCharterContract(FuelReport fuelReport, long approverId)
        {
            if (!(fuelReport.FuelReportType == FuelReportTypes.CharterInEnd ||
                fuelReport.FuelReportType == FuelReportTypes.CharterOutStart))
                return;

            IWorkflowStepRepository _workflowStepRepository = ServiceLocator.Current.GetInstance<IWorkflowStepRepository>();
            ICharterInDomainService charterInDomainService = ServiceLocator.Current.GetInstance<ICharterInDomainService>();
            ICharterOutDomainService charterOutDomainService = ServiceLocator.Current.GetInstance<ICharterOutDomainService>();
            IEventPublisher eventPublisher = ServiceLocator.Current.GetInstance<IEventPublisher>();
            IVesselInCompanyDomainService vesselInCompanyDomainService = ServiceLocator.Current.GetInstance<IVesselInCompanyDomainService>();
            IInventoryOperationNotifier inventoryOperationNotifier = ServiceLocator.Current.GetInstance<IInventoryOperationNotifier>();
            ICurrencyDomainService currencyDomainService = ServiceLocator.Current.GetInstance<ICurrencyDomainService>();

            var charterFactory = new CharterFactory(_workflowStepRepository,
             charterInDomainService, charterOutDomainService, eventPublisher, vesselInCompanyDomainService, inventoryOperationNotifier);


            if (!fuelReport.CreatedCharterId.HasValue)
            {
                var charterItemsToAdd = fuelReport.FuelReportDetails.Select(frd => charterFactory.CraeteCharterItem(0, 0, frd.ROB, 0, 0, frd.GoodId, frd.TankId, frd.MeasuringUnitId)).ToList();

                if (fuelReport.FuelReportType == FuelReportTypes.CharterInEnd)
                {
                    fuelReport.CreatedCharter = charterFactory.CreateCharterIn(0, fuelReport.VesselInCompany.CompanyId, fuelReport.VesselInCompany.Vessel.OwnerId, fuelReport.VesselInCompanyId, currencyDomainService.GetMainCurrency().Id, fuelReport.EventDate,
                                                                               charterItemsToAdd,
                                                                               new List<InventoryOperation>(), CharterType.End, CharterEndType.None, OffHirePricingType.Undefined, approverId);
                }
                else if (fuelReport.FuelReportType == FuelReportTypes.CharterOutStart)
                {
                    fuelReport.CreatedCharter = charterFactory.CreateCharterOut(0, fuelReport.VesselInCompany.CompanyId, fuelReport.VesselInCompany.CompanyId, fuelReport.VesselInCompanyId, currencyDomainService.GetMainCurrency().Id, fuelReport.EventDate,
                                                                                charterItemsToAdd,
                                                                                new List<InventoryOperation>(), CharterType.Start, CharterEndType.None, OffHirePricingType.Undefined, approverId);
                }
            }
            else
            {
                //No Implementation for Charter In End / Charter Out start FuelReport Edit.

                //var charterItemsToUpdate = fuelReport.FuelReportDetails.Select(frd => fuelReport.CreatedCharter.CharterItems.SingleOrDefault(ci => ci.GoodId == frd.GoodId)).ToList();

                //if (fuelReport.FuelReportType == FuelReportTypes.CharterInEnd)
                //{
                //    ICharterInRepository charterInRepository = ServiceLocator.Current.GetInstance<ICharterInRepository>();

                //    var entity = charterFactory.ReCreateCharterIn(charterInRepository.GetCharterStartById(id));
                //    if (entity == null)
                //        throw new ObjectNotFound("Charter In", id);

                //    _charterInRepository.Update(entity);

                //}
                //else if (fuelReport.FuelReportType == FuelReportTypes.CharterOutStart)
                //{
                //    ICharterOutRepository charterOutRepository = ServiceLocator.Current.GetInstance<ICharterOutRepository>();

                //}
            }
        }

        //================================================================================

        #region VesselEventReport

        public VesselEventReportsView GetVesselEventReportsView(string eventReportCode)
        {
            int eventReportCodeIntValue = int.Parse(eventReportCode);
            return this.vesselEventReportsViewRep.Single(v => v.Id == eventReportCodeIntValue);
        }

        public IList<VesselEventReportsView> GetVesselEventReportsViewData(DateTime fromDate, DateTime toDate, string vesselCode)
        {
            return this.vesselEventReportsViewRep.Find(v => v.Date >= fromDate && v.Date <= toDate && v.ShipCode == vesselCode);
        }

        public List<GoodTrustReceiveData> GetGoodsTotalTrustReceiveData(CharterOut comparingCharterOutStart)
        {

            var ownerId = comparingCharterOutStart.OwnerId.Value;
            var vesselCode = comparingCharterOutStart.VesselInCompany.Code;

            var lastCharterOutEnd = this.charteringDomainService.GetLastCharterOutEnd(comparingCharterOutStart.Owner, comparingCharterOutStart.VesselInCompany, comparingCharterOutStart.ActionDate);
            var lastCharterInStart = this.charteringDomainService.GetCharterInStart(comparingCharterOutStart.Owner, comparingCharterOutStart.VesselInCompany, comparingCharterOutStart.ActionDate);

            var fromDate = DateTime.MinValue;

            if (lastCharterOutEnd == null && lastCharterInStart != null)
            {
                fromDate = lastCharterInStart.ActionDate;
            }
            else if (lastCharterOutEnd != null && lastCharterInStart == null)
            {
                fromDate = lastCharterOutEnd.ActionDate;
            }
            else if (lastCharterOutEnd != null && lastCharterInStart != null)
            {
                fromDate = lastCharterOutEnd.ActionDate > lastCharterInStart.ActionDate ? lastCharterOutEnd.ActionDate : lastCharterInStart.ActionDate;
            }

            var currentDate = comparingCharterOutStart.ActionDate;

            var result = this.findGoodsTrustReceiveData(ownerId, vesselCode, fromDate, currentDate);

            return result;
        }

        public List<GoodTrustReceiveData> GetGoodsTotalTrustReceiveData(CharterIn comparingCharterInEnd)
        {
            var ownerId = comparingCharterInEnd.OwnerId.Value;
            var vesselCode = comparingCharterInEnd.VesselInCompany.Code;

            var lastCharterInStart = this.charteringDomainService.GetCharterInStart(comparingCharterInEnd.Charterer, comparingCharterInEnd.VesselInCompany, comparingCharterInEnd.ActionDate);
            var lastCharterOutEnd = this.charteringDomainService.GetLastCharterOutEnd(comparingCharterInEnd.Charterer, comparingCharterInEnd.VesselInCompany, comparingCharterInEnd.ActionDate);

            if (lastCharterInStart == null)
                throw new ObjectNotFound("lastCharterInStart");

            var fromDate = lastCharterInStart.ActionDate;

            if (lastCharterOutEnd != null && lastCharterOutEnd.ActionDate > lastCharterInStart.ActionDate)
                fromDate = lastCharterOutEnd.ActionDate;

            var currentDate = comparingCharterInEnd.ActionDate;

            var result = this.findGoodsTrustReceiveData(ownerId, vesselCode, fromDate, currentDate);

            return result;
        }

        private List<GoodTrustReceiveData> findGoodsTrustReceiveData(long companyId, string shipCode, DateTime fromDate, DateTime currentDate)
        {
            var goodRepository = ServiceLocator.Current.GetInstance<IGoodRepository>();

            var goods = goodRepository.Find(g => g.CompanyId == companyId).ToList();

            var result = new List<GoodTrustReceiveData>();

            foreach (var good in goods)
            {
                var fuelReportDetailsWithTrustReceive = this.fuelReportDetailRepository.Find(frd =>
                    frd.GoodId == good.Id &&
                    (frd.FuelReport.State == States.Submitted || frd.FuelReport.State == States.SubmitRejected) &&
                    frd.FuelReport.EventDate >= fromDate && frd.FuelReport.EventDate <= currentDate &&
                    frd.FuelReport.VesselInCompany.CompanyId == companyId && frd.FuelReport.VesselInCompany.Vessel.Code == shipCode &&
                    frd.ReceiveType.HasValue && frd.ReceiveType.Value == ReceiveTypes.Trust
                    ).ToList();

                var totalTrustQuantity = fuelReportDetailsWithTrustReceive.Sum(frd => frd.Receive.GetValueOrDefault());

                if (totalTrustQuantity != 0)
                {
                    var trustReceiveData = new GoodTrustReceiveData()
                    {
                        Good = good,
                        GoodUnit = fuelReportDetailsWithTrustReceive.First().MeasuringUnit,
                        Quantity = totalTrustQuantity
                    };

                    result.Add(trustReceiveData);
                }
            }

            return result;
        }

        #endregion


        public bool IsFuelReportTheFirstOneInVesselActivePeriod(FuelReport fuelReport)
        {
            var vesselActivationCharterContract = this.GetVesselActivationCharterContract(fuelReport);

            return this.IsFuelReportTheFirstOneInVesselActivePeriod(fuelReport, vesselActivationCharterContract);
        }

        public bool IsFuelReportTheFirstOneInVesselActivePeriod(FuelReport fuelReport, Charter vesselActivationCharterContract)
        {
            var isTheFirstOne = false;

            if (vesselActivationCharterContract == null)
            {
                var oneDayBefore = fuelReport.EventDate.AddDays(-1).Date;
                var fuelReportsCountBeforeCurrentDay = getCompanyVesselFuelReportsCounts(fuelReport.VesselInCompanyId, null, oneDayBefore, true);
                //long fuelReportsCountBeforeCurrentDay =
                //    this
                //        .GetCompanyVesselFuelReports(fuelReport.VesselInCompanyId, null, oneDayBefore, true)
                //        .Count();

                isTheFirstOne = (fuelReportsCountBeforeCurrentDay == 0);
            }
            else
            {
                if (fuelReport.EventDate < vesselActivationCharterContract.ActionDate)
                    throw new InvalidOperation("FuelReportOperation", "The vessel is not active at the fuel report processing date.");

                var comparingEndDateTime = fuelReport.EventDate.AddSeconds(-1);

                var fuelReportsCountBeforeCurrentFuelReport = getCompanyVesselFuelReportsCounts(fuelReport.VesselInCompanyId,
                            vesselActivationCharterContract.ActionDate, comparingEndDateTime, false);

                //long fuelReportsCountBeforeCurrentFuelReport =
                //    this
                //        .GetCompanyVesselFuelReports(fuelReport.VesselInCompanyId, vesselActivationCharterContract.ActionDate, comparingEndDateTime, false)
                //        .Count();

                isTheFirstOne = (fuelReportsCountBeforeCurrentFuelReport == 0);
            }

            return isTheFirstOne;
        }



        public void SetNextFuelReportsFirstPositionStatus(FuelReport fuelReport)
        {
            //Find all FuelReports which are set as First Record in vessel active period due to malfunctioning or inserted new records before them (like the one that is given as the parameter)...
            var fuelReportsToRevise = this.fuelReportRepository.Find(fr => fr.IsTheFirstReport &&
                fr.ActivationCharterContractId == fuelReport.ActivationCharterContractId && fr.EventDate > fuelReport.EventDate && fr.VesselInCompanyId == fuelReport.VesselInCompanyId).ToList();

            //Set all found FuelReports status as "Not the First Report"...
            fuelReportsToRevise.ForEach(fr => fr.IsTheFirstReport = false);
        }

        public bool IsThereAnySubmittedFuelReportAfter(FuelReport fuelReport)
        {
            return fuelReportRepository.Find(MITD.Domain.Model.Extensions.And(this.isFuelReportSubmitted.Predicate, fr => fr.EventDate > fuelReport.EventDate && fr.VesselInCompanyId == fuelReport.VesselInCompanyId)).Count == 0;
        }

        public bool IsThereAnySubmittedFuelReportAtTheTimeOfCurrentReport(FuelReport fuelReport)
        {
            return fuelReportRepository.Find(MITD.Domain.Model.Extensions.And(this.isFuelReportSubmitted.Predicate,
                fr => fr.EventDate == fuelReport.EventDate &&
                    fr.Id != fuelReport.Id &&
                    fr.VesselInCompanyId == fuelReport.VesselInCompanyId)).Count == 0;
        }

        public List<long> FindFuelReportDetailsWithReceiveByOrder(long orderId)
        {
            Func<FuelReportDetail, bool> queryToFilterFuelReportDetailsWithOrderAsReceiveReference =
                frd => frd.ReceiveReference.ReferenceType.HasValue && frd.ReceiveReference.ReferenceType.Value == ReferenceType.Order &&
                    frd.ReceiveReference.ReferenceId.HasValue && frd.ReceiveReference.ReferenceId.Value == orderId;

            var fuelReports = this.fuelReportRepository.Find(MITD.Domain.Model.Extensions.And(this.isFuelReportClosed.Predicate, fr => fr.FuelReportDetails.Any(frd => frd.ReceiveReference.ReferenceType.HasValue && frd.ReceiveReference.ReferenceType.Value == ReferenceType.Order &&
                    frd.ReceiveReference.ReferenceId.HasValue && frd.ReceiveReference.ReferenceId.Value == orderId)));

            return fuelReports.SelectMany(fr => fr.FuelReportDetails.Where(frd => frd.ReceiveReference.ReferenceType.HasValue && frd.ReceiveReference.ReferenceType.Value == ReferenceType.Order &&
                    frd.ReceiveReference.ReferenceId.HasValue && frd.ReceiveReference.ReferenceId.Value == orderId)).Select(frd => frd.Id).ToList();
        }

        public void Delete(FuelReport fuelReport)
        {
            while (fuelReport.FuelReportDetails.Count > 0)
            {
                while (fuelReport.FuelReportDetails.First().InventoryOperations.Count > 0)
                {
                    this.inventoryOperationRepository.Delete(fuelReport.FuelReportDetails.First().InventoryOperations[0]);
                }

                this.fuelReportDetailRepository.Delete(fuelReport.FuelReportDetails.First());
            }

            while (fuelReport.ApproveWorkFlows.Count > 0)
            {
                this.workflowLogRepository.Delete(fuelReport.ApproveWorkFlows[0]);
            }

            while (fuelReport.ConsumptionInventoryOperations.Count > 0)
            {
                this.inventoryOperationRepository.Delete(fuelReport.ConsumptionInventoryOperations[0]);
            }

            this.fuelReportRepository.Delete(fuelReport);
        }

        public List<long> GetFuelReportsWithCorrectionButNotRevisedByFinance(long? companyId, string warehouseCode, DateTime toDateTime)
        {
            var isWarehouseCodeSpecified = !string.IsNullOrWhiteSpace(warehouseCode);

            var fuelReportRepository = ServiceLocator.Current.GetInstance<IFuelReportRepository>();
            var fuelReportsWithCorrectionButNotRevised = fuelReportRepository.Find(fr =>
                fr.EventDate >= new DateTime(2015, 4, 1, 0, 0, 0) &&
                fr.EventDate <= toDateTime &&
                (!companyId.HasValue || fr.VesselInCompany.CompanyId == companyId) &&
                (!isWarehouseCodeSpecified || fr.VesselInCompany.Vessel.Code == warehouseCode) &&
                (fr.State == States.Submitted) &&
                (fr.FuelReportDetails.Any(
                    frd => frd.Correction.HasValue && //Check the details for corrections without revision or not submitted yet by finance.
                           (!frd.IsCorrectionPricingTypeRevised ||
                            !(fr.ApproveWorkFlows.Any(w => w.Active && w.CurrentWorkflowStep.CurrentWorkflowStage == WorkflowStages.FinancialSubmitted)))
                            )
                )
                ).ToList().OrderBy(fr => fr.EventDate).Select(fr => fr.Id).ToList();

            return fuelReportsWithCorrectionButNotRevised;
        }

    }
}

