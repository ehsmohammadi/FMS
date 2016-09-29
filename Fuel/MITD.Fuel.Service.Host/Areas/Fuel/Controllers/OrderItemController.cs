using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Fuel.Presentation.Contracts.FacadeServices.Fuel;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public   class OrderItemController : ApiController
    {

        #region props
        private IOrderFacadeService FacadeService { get; set; }
        #endregion

        public OrderItemController()
        {
            var scope = ServiceLocator.Current.GetInstance<IUnitOfWorkScope>();
            try
            {
                this.FacadeService = ServiceLocator.Current.GetInstance<IOrderFacadeService>();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public OrderItemController(IOrderFacadeService facadeService)
            : base()
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;

        }

        public OrderItemDto Post(long id, [FromBody] OrderItemDto entity)
        {
            return this.FacadeService.AddItem(entity);
        }

        public OrderItemDto Put(long id, long orderItemId, [FromBody] OrderItemDto entity)
        {
            return this.FacadeService.UpdateItem(entity);
        }

        public void Delete(long id, long orderItemId)
        {
            this.FacadeService.DeleteItem(id, orderItemId);
        }
        public OrderItemDto Get(long id,long orderItemId )
        {
            return this.FacadeService.GetOrderItemById(id, orderItemId);
        }
        public MainUnitValueDto Get(long goodId,long goodUnitId, decimal value)
        {
            return this.FacadeService.GetGoodMainUnit(goodId,goodUnitId,value );
        }


    }
}