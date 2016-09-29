using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices.Fuel;
using MITD.Presentation.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public partial class InvoiceController : ApiController
    {
        #region props

        private IInvoiceFacadeService FacadeService { get; set; }

        #endregion

        #region ctor

        public InvoiceController()
        {
            var scope = ServiceLocator.Current.GetInstance<IUnitOfWorkScope>();
            try
            {
                this.FacadeService = ServiceLocator.Current.GetInstance<IInvoiceFacadeService>();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public InvoiceController(IInvoiceFacadeService facadeService)
            : base()
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;

            try
            {
                this.FacadeService = ServiceLocator.Current.GetInstance<IInvoiceFacadeService>();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region methods


        public PageResultDto<InvoiceDto> Get(long? companyId, DateTime? fromDate, DateTime? toDate, string invoiceIds, string invoiceItemIds, string invoiceNumber, long? vesselInCompanyId, string orderNumbers, InvoiceTypeEnum? invoiceType,
                                             int pageSize,
                                             int pageIndex,
                                             bool submitedState)
            
        {
            var result = this.FacadeService.GetByFilter(companyId, fromDate, toDate, invoiceIds, invoiceItemIds, invoiceNumber, vesselInCompanyId, orderNumbers, invoiceType, pageSize, pageIndex, submitedState);
            return result;
        }

        public PageResultDto<InvoiceDto> Get(int pageSize, int pageIndex)
        {
            var data = this.FacadeService.GetAll(pageSize, pageIndex);
            return data;
        }

        public InvoiceDto Get(int id)
        {
            var result = this.FacadeService.GetById(id);
            return result;
        }

        public InvoiceDto Post([FromBody] InvoiceDto entity)
        {
            var result = this.FacadeService.Add(entity);
            return result;
        }

        public InvoiceDto Put(int id, [FromBody] InvoiceDto invoiceEntity)
        {
            var result = this.FacadeService.Update(invoiceEntity);
            return result;
        }

        public void Delete(int id)
        {
            this.FacadeService.DeleteById(id);
        }

        public void Delete([FromBody] InvoiceDto entity)
        {
            this.FacadeService.Delete(entity);
        }

        #endregion
    }
}