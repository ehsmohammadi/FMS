using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class UsersController : ApiController
    {
        #region props
        private IUserFacadeService FacadeService { get; set; }
        #endregion

        #region ctor

        public UsersController()
        {
            try
            {
                this.FacadeService = ServiceLocator.Current.GetInstance<IUserFacadeService>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public UsersController(IUserFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }
        #endregion

        #region methods


        public PageResultDto<UserDTOWithActions> GetAllUsers(int pageSize, int pageIndex, string filter = "")
        {
            return FacadeService.GetAllUsers(pageSize, pageIndex, filter);
        }
        public UserDto GetUser(string partyName)
        {
            return FacadeService.GetUserByUserName(partyName);
        }

        public UserDto Get(int id)
        {
            var result = this.FacadeService.GetUserWithCompany(id);
            return result;
        }

        public void Post([FromBody] UserDto entity)
        {
            this.FacadeService.Add(entity);
        }

        public void Put([FromBody] UserDto entity)
        {
            this.FacadeService.Update(entity);
        }

        public void PutChangePassWord(string newPassWord,string oldPassWord)
        {
            this.FacadeService.ChangePassWord(newPassWord,oldPassWord);
        }

        public void Delete(int id)
        {
            this.FacadeService.DeleteById(id);
        }


      

       

  
    
     


        #endregion
    }
}
