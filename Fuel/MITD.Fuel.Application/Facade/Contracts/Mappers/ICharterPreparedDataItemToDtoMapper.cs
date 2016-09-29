﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface ICharterPreparedDataItemToDtoMapper : IFacadeMapper<CharterPreparedDataItem, CharterItemDto>
    {

    }
}
