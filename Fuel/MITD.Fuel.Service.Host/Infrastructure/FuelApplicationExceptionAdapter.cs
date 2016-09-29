using System;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Infrastructure.Service;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.FuelSecurity.Domain.Model.ErrorException;

namespace MITD.Fuel.Service.Host.Infrastructure
{
    public class FuelApplicationExceptionAdapter : IFuelApplicationExceptionAdapter
    {
        private readonly IApplicationLogger _loger;

        public FuelApplicationExceptionAdapter(IApplicationLogger loger)
        {
            _loger = loger;

        }

        public ExceptionMessageDto Get(Exception exception)
        {
            var exceptionDto = new ExceptionMessageDto();

            switch (exception.GetType().Name)
            {
                case "BusinessRuleException":
                    {
                        var exp = exception as BusinessRuleException;
                        exceptionDto.Message = exp.Message;
                        exceptionDto.Parameter = exp.BusinessRuleCode;
                    }

                    break;
                case "ConcurencyException":
                    {
                        var exp = exception as ConcurencyException;
                        exceptionDto.Message = exp.Message;
                    }

                    break;

                case "InvalidArgument":
                    {
                        var exp = exception as InvalidArgument;

                        exceptionDto.Message = exp.Message;
                        exceptionDto.Parameter = exp.ParameterName;
                    }
                    break;
                case "InvalidOperation":
                    {
                        var exp = exception as InvalidOperation;
                        exceptionDto.Message = exp.Message;
                        exceptionDto.Parameter = exp.OperationName;
                    }
                    break;
                case "ObjectNotFound":
                    {
                        var exp = exception as ObjectNotFound;
                        exceptionDto.Message = exp.Message;
                        exceptionDto.Parameter = exp.EntityName;
                    }
                    break;
                case "WorkFlowException":
                    {
                        var exp = exception as WorkFlowException;
                        exceptionDto.Message = exp.Message;
                    }
                    break;
                case "FuelSecurityAccessException":
                {
                    var exp = exception as FuelSecurityAccessException;
                    exceptionDto.Message = exp.Message;
                    
                   
                }
                    break;

                default:
                    var message = HandleUnHandleException(exception);
                    _loger.Log(message);

                    exceptionDto.ExceptionMessageType = ExceptionMessageTypeDto.Error;
                    exceptionDto.Message = message;

                    break;
            }
            return exceptionDto;
        }

        private string HandleUnHandleException(Exception exception)
        {
            var logs = exception.Message;
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                logs += exception.Message;
            }
            return logs;
        }
    }
}