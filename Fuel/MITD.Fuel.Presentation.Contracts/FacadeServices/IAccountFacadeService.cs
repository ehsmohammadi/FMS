﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
  public interface IAccountFacadeService:IFacadeService
  {

      PageResultDto<AccountDto> GetAllByFilter(string name, string code, int pageIndex, int pageSize);
      void Add(AccountDto data);
  }
}