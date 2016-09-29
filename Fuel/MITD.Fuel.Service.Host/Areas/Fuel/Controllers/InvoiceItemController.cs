using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public   class InvoiceItemController : ApiController
    {

        #region props
        private IInvoiceFacadeService FacadeService { get; set; }
        #endregion

        public InvoiceItemController()
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
        public InvoiceItemController(IInvoiceFacadeService facadeService)
            : base()
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;

         
        }
        public IEnumerable<InvoiceItemDto> Get(string orderIdList)
        {
            var result = this.FacadeService.GenerateInvoiceItemForOrders(orderIdList);
            return result;
        }

//        public InvoiceItemDto Put(long id, long invoiceItemId, [FromBody] InvoiceItemDto entity)
//        {
//            return this.FacadeService.UpdateItem(entity);
//        }
//
//        public void Delete(long id, long invoiceItemId, [FromBody] InvoiceItemDto entity)
//        {
//            this.FacadeService.DeleteItem(entity);
//        }
//        public InvoiceItemDto Get(long id,long InvoiceItemId )
//        {
//            return this.FacadeService.GetInvoiceItemById(id, InvoiceItemId);
//        }
//        public MainUnitValueDto Get(long goodId,long goodUnitId, decimal value)
//        {
//            return this.FacadeService.GetGoodMainUnit(goodId,goodUnitId,value );
//        }


    }
}