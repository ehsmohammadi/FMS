using System;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Service.Host
{
    public interface IFuelApplicationExceptionAdapter
    {
        ExceptionMessageDto Get(Exception exception);
    }
}