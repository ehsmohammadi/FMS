using System;
using System.Web.Http;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public partial class OrderController : ApiController
    {
        #region props

        private IOrderFacadeService FacadeService { get; set; }

        #endregion

        #region ctor

        public OrderController()
        {
            try
            {
                this.FacadeService = ServiceLocator.Current.GetInstance<IOrderFacadeService>();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public OrderController(IOrderFacadeService facadeService)
            : base()
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;


        }
        #endregion

        #region methods

        public PageResultDto<OrderDto> Get(long? companyId, DateTime? fromDate, DateTime? toDate, long? orderId, long? orderItemId, string orderNumber, string orderTypes, int pageSize, int pageIndex, long? vesselInCompanyId, long? supplierId, long? transporterId, bool includeOrderItem = false, string orderIdList = null, bool submitedState = false)
        {
            var result = this.FacadeService.GetByFilter(companyId, fromDate, toDate, orderId, orderItemId, orderNumber, orderTypes, pageSize, pageIndex, vesselInCompanyId, supplierId, transporterId, includeOrderItem: includeOrderItem, orderIdList: orderIdList, submitedState: submitedState);
            return result;
        }

        public PageResultDto<OrderDto> Get(int pageSize, int pageIndex)
        {
            var data = this.FacadeService.GetAll(pageSize, pageIndex);
            return data;
        }

        public OrderDto Get(int id)
        {
            var result = this.FacadeService.GetById(id);
            return result;
        }

        public OrderDto Post([FromBody] OrderDto entity)
        {
            var result = this.FacadeService.Add(entity);
            return result;
        }

        public OrderDto Put(int id, [FromBody] OrderDto orderEntity)
        {
            var result = this.FacadeService.Update(orderEntity);
            return result;
        }

        public void Delete(int id)
        {
            this.FacadeService.DeleteById(id);
        }

        public void Delete([FromBody] OrderDto entity)
        {
            this.FacadeService.Delete(entity);
        }

        #endregion



    }
}
