using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class GoodController : ApiController
    {

        private IGoodFacadeService FacadeService { get; set; }

        public GoodController(IGoodFacadeService goodFacadeService)
        {

            if (goodFacadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = goodFacadeService;
        }
        public GoodController()
        {

            var scope = ServiceLocator.Current.GetInstance<IUnitOfWorkScope>();
            try
            {
                this.FacadeService = ServiceLocator.Current.GetInstance<IGoodFacadeService>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<GoodDto> Get(long id)
        {

            var data = this.FacadeService.GetAll(id);
            return data;
        }


        public void Post([FromBody]string value)
        {
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}
