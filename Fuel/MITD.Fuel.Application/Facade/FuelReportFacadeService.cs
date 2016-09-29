using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Castle.Core;
using Castle.Core.Internal;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.DTOs.Report;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.FuelSecurity.Domain.Model;
using MITD.Presentation.Contracts;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.DomainObjects.Factories;
using MITD.Fuel.Domain.Model.Specifications;

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class FuelReportFacadeService : IFuelReportFacadeService
    {
        private readonly IFuelReportApplicationService appService;
        private readonly IFuelReportFuelReportDtoMapper fuelReportMapper;
        private readonly IVesselEventReportViewToVesselEventReportViewDtoMapper vesselEventReportViewToVesselEventReportViewtoMapper;
        private readonly IFuelReportDetailToFuelReportDetailDtoMapper fuelReportDetailToFuelReportDetailDtoMapper;
        private readonly IFuelReportDomainService fuelReportDomainService;
        private readonly IInventoryOperationDomainService inventoryOperationDomainService;
        private readonly IInventoryOperationToInventoryOperationDtoMapper inventoryOperationMapper;
        private readonly ICharterPreparedDataToDtoMapper charterPreparedDataToDtoMapper;
        private readonly IInventoryManagementDomainService inventoryManagementDomainService;
        private readonly IInventoryResultItemToInventoryResultItemDtoMapper inventoryResultItemDtoMapper;

        //================================================================================

        public FuelReportFacadeService(
            IFuelReportApplicationService appService,
            IFuelReportFuelReportDtoMapper fuelReportMapper,
            IVesselEventReportViewToVesselEventReportViewDtoMapper vesselEventReportViewToVesselEventReportViewtoMapper,
            IFuelReportDetailToFuelReportDetailDtoMapper fuelReportDetailToFuelReportDetailDtoMapper,
            IFuelReportDomainService fuelReportDomainService,
            IInventoryOperationDomainService inventoryOperationDomainService,
            IInventoryOperationToInventoryOperationDtoMapper inventoryOperationMapper, ICharterPreparedDataToDtoMapper charterPreparedDataToDtoMapper, IInventoryResultItemToInventoryResultItemDtoMapper inventoryResultItemDtoMapper)
        {
            this.appService = appService;
            this.fuelReportMapper = fuelReportMapper;
            this.vesselEventReportViewToVesselEventReportViewtoMapper = vesselEventReportViewToVesselEventReportViewtoMapper;
            this.fuelReportDetailToFuelReportDetailDtoMapper = fuelReportDetailToFuelReportDetailDtoMapper;
            this.fuelReportDomainService = fuelReportDomainService;
            this.inventoryOperationDomainService = inventoryOperationDomainService;
            this.inventoryOperationMapper = inventoryOperationMapper;
            this.charterPreparedDataToDtoMapper = charterPreparedDataToDtoMapper;
            this.inventoryResultItemDtoMapper = inventoryResultItemDtoMapper;

            this.inventoryManagementDomainService = ServiceLocator.Current.GetInstance<IInventoryManagementDomainService>();
        }

        //================================================================================

        public ResultFuelReportDto Add(FuelReportCommandDto data)
        {
            var result = new ResultFuelReportDto();

            try
            {
                var entity = appService.ManageCommand(data);


                if (entity == null)
                {
                    result.Message = "The FuelReport is not operational.";

                    result.Type = ResultType.Reject;
                }
                else
                    result.Type = ResultType.Accept;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message + "\nStackTrace:\n" + ex.StackTrace;

                result.Type = ResultType.Exception;
            }

            return result;
        }

        //================================================================================

        public FuelReportDto GetById(long id, bool includeReferencesLookup)
        {
            var entity = fuelReportDomainService.Get(id);

            FuelReportDto dto = null;
            if (includeReferencesLookup)
                dto = fuelReportMapper.MapToModel(entity, manipulateFuelReportDetailExtraData);
            else
                dto = fuelReportMapper.MapToModel(entity);

            setFuelReportDetailsPreviousROB(entity, dto);

            dto.VesselEventReportViewDto = GetVesselEventData(entity.Code);

            return dto;
        }

        //================================================================================

        private void setFuelReportDetailsPreviousROB(FuelReport fuelReport, FuelReportDto fuelReportDto)
        {
            if (!fuelReport.IsTheFirstReport)
            {
                var previousFuelReport = this.fuelReportDomainService.GetPreviousFuelReport(fuelReportDto.Id);

                if (previousFuelReport == null) return;

                fuelReportDto.FuelReportDetail.ForEach(frd =>
                {
                    frd.PreviousROB = null;

                    var previousFuelReportDetail = previousFuelReport.FuelReportDetails.SingleOrDefault(pfrd => pfrd.GoodId == frd.GoodId);

                    if (previousFuelReportDetail == null) return;

                    frd.PreviousROB = previousFuelReportDetail.ROB;
                });
            }
            else if (fuelReport.ActivationCharterContract != null)
            {
                fuelReportDto.FuelReportDetail.ForEach(frdDto =>
                {
                    var fuelReportDetail = fuelReport.FuelReportDetails.Single(frd => frd.Id == frdDto.Id);
                    CharterItem charterItem = fuelReportDetail.GetRelevantActivationCharterItem();
                    frdDto.PreviousROB = charterItem == null ? null : (decimal?)charterItem.Rob;
                });
            }
        }

        //================================================================================

        private void setFuelReportDetailPreviousROB(FuelReportDetail entity, FuelReportDetailDto fuelReportDetailDto, IList<FuelReport> result = null)
        {
            if (!entity.FuelReport.IsTheFirstReport)
            {
                FuelReportDetail previousFuelReportDetail = null;

                bool isPreviousDetailFound = false;
                if (result != null)
                {
                    IsFuelReportNotCancelled isFuelReportNotCancelled = new IsFuelReportNotCancelled();

                    var previousFuelReport = result.FirstOrDefault(MITD.Domain.Model.Extensions.And(isFuelReportNotCancelled.Predicate, fr => fr.EventDate < entity.FuelReport.EventDate).Compile());
                    if (previousFuelReport != null)
                    {
                        previousFuelReportDetail = previousFuelReport.FuelReportDetails.SingleOrDefault(frd => frd.GoodId == entity.GoodId);

                        isPreviousDetailFound = previousFuelReportDetail != null;
                    }
                }

                if (result == null || !isPreviousDetailFound)
                {
                    previousFuelReportDetail = this.fuelReportDomainService.GetPreviousFuelReportDetailByGood(fuelReportDetailDto.FuelReportId, fuelReportDetailDto.GoodId.Value);
                }

                if (previousFuelReportDetail == null)
                {
                    fuelReportDetailDto.PreviousROB = null;
                }
                else
                {
                    fuelReportDetailDto.PreviousROB = previousFuelReportDetail.ROB;
                }
            }
            else if (entity.FuelReport.ActivationCharterContract != null)
            {
                CharterItem charterItem = entity.GetRelevantActivationCharterItem();
                fuelReportDetailDto.PreviousROB = charterItem == null ? null : (decimal?)charterItem.Rob;
            }
        }

        //================================================================================

        public PageResultDto<FuelReportDto> GetAll(int pageSize, int pageIndex, bool includeReferencesLookup)
        {
            List<FuelReport> data = fuelReportDomainService.GetAll();

            List<FuelReportDto> mapped = null;

            if (includeReferencesLookup)
                mapped = fuelReportMapper.MapToModel(data, manipulateFuelReportDetailExtraData).ToList();
            else
                mapped = fuelReportMapper.MapToModel(data).ToList();

            var result = new PageResultDto<FuelReportDto>
            {
                CurrentPage = 1,
                PageSize = 1,
                Result = mapped,
                TotalCount = 1,
                TotalPages = 1
            };

            return result;
        }

        //================================================================================

        public PageResultDto<FuelReportDto> GetByFilter(long? companyId, long? vesselInCompanyId, string vesselReportCode, DateTime? fromDate, DateTime? toDate, string fuelReportIds, string fuelReportDetailIds, int pageSize, int pageIndex, bool includeReferencesLookup)
        {
            //DateTime dt1 = DateTime.Now;

            fromDate = fromDate.HasValue ? (DateTime?) fromDate.Value.Date : null;
            toDate = toDate.HasValue ? (DateTime?)toDate.Value.Date.AddDays(1).AddMilliseconds(-1) : null;

            PageResult<FuelReport> data = fuelReportDomainService.GetByFilter(companyId, vesselInCompanyId, vesselReportCode, fromDate, toDate, fuelReportIds, fuelReportDetailIds, pageSize, pageIndex);

            //var dif = DateTime.Now.Subtract(dt1);
            //dt1 = DateTime.Now;

            List<FuelReportDto> mapped = fuelReportMapper.MapToModel(data.Result, (detail, dto) =>{
                if (includeReferencesLookup)
                    this.manipulateFuelReportDetailExtraData(detail, dto);

                this.setFuelReportDetailPreviousROB(detail, dto, data.Result);
            }).ToList();

            //dif = DateTime.Now.Subtract(dt1);
            //dt1 = DateTime.Now;

            var firstReport = data.Result.LastOrDefault();
            var lastReport = data.Result.FirstOrDefault();

            var fromFuelReportDate = firstReport != null ? firstReport.EventDate.Date : DateTime.MinValue;
            var toFuelReportDate = lastReport != null ? lastReport.EventDate.Date : DateTime.MaxValue;
            //dif = DateTime.Now.Subtract(dt1);
            //dt1 = DateTime.Now;

            if (data.Result.Count > 0)
            {
                var eventsData = GetVesselEventsData(fromFuelReportDate, toFuelReportDate, data.Result.First().VesselInCompany.Vessel.Code);

                //dif = DateTime.Now.Subtract(dt1);
                //dt1 = DateTime.Now;

                // Add VesselEventReportView DTOs
                mapped.ForEach(fuelReportDto =>{
                    fuelReportDto.VesselEventReportViewDto = eventsData.SingleOrDefault(ev => ev.Id.ToString() == fuelReportDto.Code);
                });
                //dif = DateTime.Now.Subtract(dt1);
                //dt1 = DateTime.Now;
            }

            var result = new PageResultDto<FuelReportDto>
                             {
                                 CurrentPage = data.CurrentPage,
                                 PageSize = data.PageSize,
                                 Result = mapped,
                                 TotalCount = data.TotalCount,
                                 TotalPages = data.TotalPages
                             };

            //dif = DateTime.Now.Subtract(dt1);

            return result;
        }

        //================================================================================

        public FuelReportDetailDto UpdateFuelReportDetail(long fuelReportId, FuelReportDetailDto fuelReportDetailDto)
        {
            var transferReference = Reference.Empty;
            var receiveReference = Reference.Empty;
            var correctionReference = Reference.Empty;

            if (fuelReportDetailDto.FuelReportTransferReferenceNoDto != null)
            {
                transferReference = new Reference()
                                    {
                                        Code = fuelReportDetailDto.FuelReportTransferReferenceNoDto.Code,
                                        ReferenceType = (ReferenceType)fuelReportDetailDto.FuelReportTransferReferenceNoDto.ReferenceType,
                                        ReferenceId = fuelReportDetailDto.FuelReportTransferReferenceNoDto.Id
                                    };
            }

            if (fuelReportDetailDto.FuelReportReceiveReferenceNoDto != null)
            {
                receiveReference = new Reference()
                {
                    Code = fuelReportDetailDto.FuelReportReceiveReferenceNoDto.Code,
                    ReferenceType = (ReferenceType)fuelReportDetailDto.FuelReportReceiveReferenceNoDto.ReferenceType,
                    ReferenceId = fuelReportDetailDto.FuelReportReceiveReferenceNoDto.Id
                };
            }


            if (fuelReportDetailDto.FuelReportCorrectionReferenceNoDto != null)
            {
                correctionReference = new Reference()
                {
                    Code = fuelReportDetailDto.FuelReportCorrectionReferenceNoDto.Code,
                    ReferenceType = (ReferenceType)fuelReportDetailDto.FuelReportCorrectionReferenceNoDto.ReferenceType,
                    ReferenceId = fuelReportDetailDto.FuelReportCorrectionReferenceNoDto.Id
                };
            }



            var updatedEntity = this.appService.UpdateFuelReportDetail(
                fuelReportDetailDto.FuelReportId,
                fuelReportDetailDto.Id,
                fuelReportDetailDto.ROB,
                fuelReportDetailDto.Consumption ?? 0,
                fuelReportDetailDto.Recieve,
                this.fuelReportDetailToFuelReportDetailDtoMapper.MapDtoReceiveTypeTypeToEntityReceiveTypeType(fuelReportDetailDto.ReceiveType),
                fuelReportDetailDto.Transfer,
                this.fuelReportDetailToFuelReportDetailDtoMapper.MapDtoTransferTypeTypeToEntityTransferTypeType(fuelReportDetailDto.TransferType),
                fuelReportDetailDto.Correction,
                this.fuelReportDetailToFuelReportDetailDtoMapper.MapDtoCorrectionTypeToEntityCorrectionType(fuelReportDetailDto.CorrectionType),
                this.fuelReportDetailToFuelReportDetailDtoMapper.MapDtoCorrectionPricingTypeToEntityCorrectionPricingType(fuelReportDetailDto.CorrectionPricingType),
                fuelReportDetailDto.CorrectionPrice,
                fuelReportDetailDto.CurrencyDto == null ? null : (long?)fuelReportDetailDto.CurrencyDto.Id,
                transferReference,
                receiveReference,
                correctionReference,
                fuelReportDetailDto.TrustIssueInventoryTransactionItemId);

            var dto = fuelReportDetailToFuelReportDetailDtoMapper.MapToModel(updatedEntity);

            manipulateFuelReportDetailExtraData(updatedEntity, dto);

            setFuelReportDetailPreviousROB(updatedEntity, dto);

            return dto;
        }

        public FuelReportDetailDto UpdateFuelReportDetailByFinance(long fuelReportId, FuelReportDetailDto fuelReportDetailDto)
        {
            return UpdateFuelReportDetail(fuelReportId, fuelReportDetailDto);
        }

        //================================================================================

        public FuelReportDto Update(FuelReportDto fuelReportDto)
        {
            if (fuelReportDto.Voyage == null)
                throw new InvalidArgument("fuelReportDto.Voyage");

            var updatedFuelReport = this.appService.UpdateVoyageId(
                fuelReportDto.Id,
                fuelReportDto.Voyage.Id);

            var updatedDto = this.GetById(fuelReportDto.Id, false);

            return updatedDto;
        }

        //================================================================================

        private void populateFuelReportDetailDtoReferencesLookup(FuelReportDetail fuelReportDetail, FuelReportDetailDto fuelReportDetailDto)
        {
            fuelReportDetailDto.CorrectionReferenceNoDtos = new List<FuelReportCorrectionReferenceNoDto>();

            if (fuelReportDetail.Correction.HasValue)
            {
                var correctionReferences = fuelReportDomainService.GetFuelReportDetailCorrectionReferences(fuelReportDetail);

                if (correctionReferences != null)
                {
                    fuelReportDetailDto.CorrectionReferenceNoDtos.AddRange(
                                                                           correctionReferences.Select(r => new FuelReportCorrectionReferenceNoDto()
                                                                                                            {
                                                                                                                Id = r.ReferenceId.Value,
                                                                                                                Code = r.Code,
                                                                                                                ReferenceType = (ReferenceTypeEnum)(int)r.ReferenceType.Value
                                                                                                            }));
            }
            }


            fuelReportDetailDto.TransferReferenceNoDtos = new Dictionary<TransferTypeEnum, List<FuelReportTransferReferenceNoDto>>();

            if (fuelReportDetail.Transfer.HasValue)
            {
                var internalTransferTransferReferences = fuelReportDomainService.GetFuelReportDetailInternalTransferTransferReferences(fuelReportDetail);
                var saleTransferTransferReferences = fuelReportDomainService.GetFuelReportDetailSaleTransferTransferReferences(fuelReportDetail);
                var rejectedTransferReferences = fuelReportDomainService.GetFuelReportDetailRejectedTransferReferences(fuelReportDetail);

                fuelReportDetailDto.TransferReferenceNoDtos.Add(TransferTypeEnum.InternalTransfer, new List<FuelReportTransferReferenceNoDto>());
                fuelReportDetailDto.TransferReferenceNoDtos.Add(TransferTypeEnum.Rejected, new List<FuelReportTransferReferenceNoDto>());
                fuelReportDetailDto.TransferReferenceNoDtos.Add(TransferTypeEnum.TransferSale, new List<FuelReportTransferReferenceNoDto>());

                if (internalTransferTransferReferences != null)
                {
                    fuelReportDetailDto.TransferReferenceNoDtos[TransferTypeEnum.InternalTransfer].AddRange(internalTransferTransferReferences.Select(r => new FuelReportTransferReferenceNoDto()
                                                                                                                                                           {
                                                                                                                                                               Id = r.ReferenceId.Value,
                                                                                                                                                               Code = r.Code,
                                                                                                                                                               ReferenceType = (ReferenceTypeEnum)(int)r.ReferenceType.Value
                                                                                                                                                           }));
                }


                if (saleTransferTransferReferences != null)
                {
                    fuelReportDetailDto.TransferReferenceNoDtos[TransferTypeEnum.TransferSale].AddRange(saleTransferTransferReferences.Select(r => new FuelReportTransferReferenceNoDto()
                                                                                                                                                   {
                                                                                                                                                       Id = r.ReferenceId.Value,
                                                                                                                                                       Code = r.Code,
                                                                                                                                                       ReferenceType = (ReferenceTypeEnum)(int)r.ReferenceType.Value
                                                                                                                                                   }));
                }

                if (rejectedTransferReferences != null)
                {
                    fuelReportDetailDto.TransferReferenceNoDtos[TransferTypeEnum.Rejected].AddRange(rejectedTransferReferences.Select(r => new FuelReportTransferReferenceNoDto()
                                                                                                                                           {
                                                                                                                                               Id = r.ReferenceId.Value,
                                                                                                                                               Code = r.Code,
                                                                                                                                               ReferenceType = (ReferenceTypeEnum)(int)r.ReferenceType.Value
                                                                                                                                           }));
                }
            }

            fuelReportDetailDto.ReceiveReferenceNoDtos = new Dictionary<ReceiveTypeEnum, List<FuelReportReceiveReferenceNoDto>>();

            if (fuelReportDetail.Receive.HasValue || (fuelReportDetailDto.IsTrustIssueQuantityAssignmentPossible && fuelReportDetailDto.TrustIssueInventoryResultItem != null))
            {
                var internalTransferReceiveReferences = fuelReportDomainService.GetFuelReportDetailInternalTransferReceiveReferences(fuelReportDetail, fuelReportDetailDto.IsTrustIssueQuantityAssignmentPossible, fuelReportDetailDto.TrustIssueInventoryResultItem == null ? null : (decimal?)fuelReportDetailDto.TrustIssueInventoryResultItem.Quantity);
                var purchaseReceiveReferences = fuelReportDomainService.GetFuelReportDetailPurchaseReceiveReferences(fuelReportDetail, fuelReportDetailDto.IsTrustIssueQuantityAssignmentPossible, fuelReportDetailDto.TrustIssueInventoryResultItem == null ? null : (decimal?)fuelReportDetailDto.TrustIssueInventoryResultItem.Quantity);
                var transferPurchaseReceiveReferences = fuelReportDomainService.GetFuelReportDetailTransferPurchaseReceiveReferences(fuelReportDetail, fuelReportDetailDto.IsTrustIssueQuantityAssignmentPossible, fuelReportDetailDto.TrustIssueInventoryResultItem == null ? null : (decimal?)fuelReportDetailDto.TrustIssueInventoryResultItem.Quantity);

                fuelReportDetailDto.ReceiveReferenceNoDtos.Add(ReceiveTypeEnum.InternalTransfer, new List<FuelReportReceiveReferenceNoDto>());
                fuelReportDetailDto.ReceiveReferenceNoDtos.Add(ReceiveTypeEnum.Purchase, new List<FuelReportReceiveReferenceNoDto>());
                fuelReportDetailDto.ReceiveReferenceNoDtos.Add(ReceiveTypeEnum.TransferPurchase, new List<FuelReportReceiveReferenceNoDto>());

                if (internalTransferReceiveReferences != null)
                {
                    fuelReportDetailDto.ReceiveReferenceNoDtos[ReceiveTypeEnum.InternalTransfer].AddRange(internalTransferReceiveReferences.Select(r => new FuelReportReceiveReferenceNoDto()
                                                                                                                                                        {
                                                                                                                                                            Id = r.ReferenceId.Value,
                                                                                                                                                            Code = r.Code,
                                                                                                                                                            ReferenceType = (ReferenceTypeEnum)(int)r.ReferenceType.Value
                                                                                                                                                        }));
                }

                if (purchaseReceiveReferences != null)
                {
                    fuelReportDetailDto.ReceiveReferenceNoDtos[ReceiveTypeEnum.Purchase].AddRange(purchaseReceiveReferences.Select(r => new FuelReportReceiveReferenceNoDto()
                                                                                                                                        {
                                                                                                                                            Id = r.ReferenceId.Value,
                                                                                                                                            Code = r.Code,
                                                                                                                                            ReferenceType = (ReferenceTypeEnum)(int)r.ReferenceType.Value
                                                                                                                                        }));
                }

                if (transferPurchaseReceiveReferences != null)
                {
                    fuelReportDetailDto.ReceiveReferenceNoDtos[ReceiveTypeEnum.TransferPurchase].AddRange(transferPurchaseReceiveReferences.Select(r => new FuelReportReceiveReferenceNoDto()
                                                                                                                                                        {
                                                                                                                                                            Id = r.ReferenceId.Value,
                                                                                                                                                            Code = r.Code,
                                                                                                                                                            ReferenceType = (ReferenceTypeEnum)(int)r.ReferenceType.Value
                                                                                                                                                        }));
                }
            }
        }

        //================================================================================

        public List<FuelReportInventoryOperationDto> GetDetailInventoryOperations(long id, long detailId)
        {
            var result = inventoryOperationMapper.MapToModel(
                inventoryOperationDomainService.GetFuelReportDetailInventoryOperations(id, detailId)).ToList();

            var detailDto = GetById(id, false).FuelReportDetail.First(frd => frd.Id == detailId);

            result.ForEach(invDto => invDto.Good = detailDto.Good);

            return result;
        }

        //================================================================================

        public CharterDto PrepareCharterData(long fuelReportId)
        {
            var preparedCharterData = fuelReportDomainService.PrepareCharterData(fuelReportId);

            return charterPreparedDataToDtoMapper.MapToModel(preparedCharterData);
        }

        //================================================================================

        public FuelReportDetailDto GetFuelReportDetailById(long id, long detailId, bool includeReferencesLookup)
        {
            var entity = fuelReportDomainService.GetFuelReportDetailById(id, detailId);

            var dto = fuelReportDetailToFuelReportDetailDtoMapper.MapToModel(entity);

            if (includeReferencesLookup)
            {
                manipulateFuelReportDetailExtraData(entity, dto);
            }

            setFuelReportDetailPreviousROB(entity, dto);

            return dto;
        }

        private void manipulateFuelReportDetailExtraData(FuelReportDetail entity, FuelReportDetailDto dto)
        {
            setFuelReportDetailEditingMode(entity, dto);

            var isTheFirstFuelReport = fuelReportDomainService.IsFuelReportTheFirstOneInVesselActivePeriod(entity.FuelReport);

            if (isTheFirstFuelReport)
            {
                var lastIssuedTrustReceiveTransaction = inventoryManagementDomainService.GetLastNotOperatedIssuedTrustReceive(entity.FuelReport.VesselInCompany.CompanyId, entity.FuelReport.VesselInCompany.Code, entity.FuelReport.EventDate);

                if (lastIssuedTrustReceiveTransaction != null)
                {
                    InventoryResultItem lastIssuedTrusyReceiveTransactionItem = null;
                    lastIssuedTrusyReceiveTransactionItem = lastIssuedTrustReceiveTransaction.InventoryResultItems.SingleOrDefault(ri => ri.Good.Id == entity.GoodId);

                    if (lastIssuedTrusyReceiveTransactionItem != null)
                    {
                        dto.TrustIssueInventoryResultItem = inventoryResultItemDtoMapper.MapToModel(lastIssuedTrusyReceiveTransactionItem);
                        dto.IsTrustIssueQuantityAssignmentPossible = true;
                    }
                    else
                    {
                        dto.TrustIssueInventoryResultItem = null;
                        dto.IsTrustIssueQuantityAssignmentPossible = false;
                    }
                }
                else
                {
                    dto.TrustIssueInventoryResultItem = null;
                    dto.IsTrustIssueQuantityAssignmentPossible = false;
                }
            }

            dto.TrustIssueInventoryTransactionItemId = entity.TrustIssueInventoryTransactionItemId;
            dto.IsTheTrustIssueQuantityAssignedTo = entity.TrustIssueInventoryTransactionItemId.HasValue;

            populateFuelReportDetailDtoReferencesLookup(entity, dto);
        }

        private void setFuelReportDetailEditingMode(FuelReportDetail entity, FuelReportDetailDto dto)
        {
            var securityFacade = ServiceLocator.Current.GetInstance<ISecurityFacadeService>();

            var actions = securityFacade.GetUserAuthorizedActions(ClaimsPrincipal.Current);
            //var actions = securityFacade.GetUserAuthorizedActionsById(SecurityApplicationService.GetCurrentUserId());

            dto.EnableCommercialEditing = actions.Exists(a => a.Id == ActionType.EditFuelReport.Id) && (entity.FuelReport.State == States.Open || entity.FuelReport.State == States.SubmitRejected);

            var lastFuelReportStage = entity.FuelReport.ApproveWorkFlows.Last(wf => wf.Active).CurrentWorkflowStep.CurrentWorkflowStage;

            dto.EnableFinancialEditing = actions.Exists(a => a.Id == ActionType.EditFinancialFuelReport.Id) && (entity.Correction.HasValue) && (entity.FuelReport.State == States.Submitted) && lastFuelReportStage == WorkflowStages.Submited;
        }

        public void RefreshFuelReportsVoyage(long companyId, long? vesselInCompanyId)
        {
            this.appService.RefreshFuelReportsVoyage(companyId, vesselInCompanyId);
        }

        public VesselEventReportViewDto GetVesselEventData(string eventReportCode)
        {
            var data = fuelReportDomainService.GetVesselEventReportsView(eventReportCode);
            return data != null ? vesselEventReportViewToVesselEventReportViewtoMapper.MapToModel(data) : null;
        }

        public IList<VesselEventReportViewDto> GetVesselEventsData(DateTime fromDate, DateTime toDate, string vesselCode)
        {
            var data = fuelReportDomainService.GetVesselEventReportsViewData(fromDate.Date, toDate.Date, vesselCode);

            return vesselEventReportViewToVesselEventReportViewtoMapper.MapToModel(data).ToList();
        }

        public void Delete(long id)
        {
            this.appService.Delete(id);
        }

        public void RevertFuelReportConsumptionInventoryOperations(long fuelReportId)
        {
           this.appService.RevertFuelReportConsumptionInventoryOperations(fuelReportId); 
        }

        public void RevertFuelReportDetailCorrectionInventoryOperations(long fuelReportId, long fuelReportDetailId)
        {
            throw new NotImplementedException();
        }

        public void RevertFuelReportDetailReceiveInventoryOperations(long fuelReportId, long fuelReportDetailId)
        {
            throw new NotImplementedException();
        }

        public void RevertFuelReportDetailTransferInventoryOperations(long fuelReportId, long fuelReportDetailId)
        {
            throw new NotImplementedException();
        }

        //================================================================================

        public List<FuelReportInventoryOperationDto> GetInventoryOperations(long id)
        {
            var result = inventoryOperationMapper.MapToModel(
                inventoryOperationDomainService.GetFuelReportHeaderInventoryOperations(id)).ToList();

            result.ForEach(invDto =>
            {
                invDto.Good = new GoodDto();
                invDto.Good.Name = "All";
            });

            return result;
        }

        //================================================================================
    }
}
