using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MITD.Fuel.ACL.StorageSpace.InventoryServiceReference;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Inventory;
using MITD.Fuel.Domain.Model.Enums;
namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Tests
{
    [TestClass()]
    public class InventoryOperationNotifierTests
    {
        [TestMethod()]
        public void TestNotifySubmittingCharterInStart()
        {
            var charterInDto = new CharterDto();

            charterInDto.CharterType = MITD.Fuel.ACL.StorageSpace.InventoryServiceReference.CharterType.In;

            charterInDto.CharterItems = new List<CharterItemDto>() { new CharterItemDto() };

            //var inventoryOperationDtosResult = svc.NotifySubmittingCharterInStart(charterInDto).ToList();
            //var inventoryOperationDtosResult = new List<FuelReportInventoryOperationDto>();

            //var tcs = new TaskCompletionSource<List<InventoryOperation>>();
            var syncEvent = new AutoResetEvent(false);

            List<InventoryOperation> callResult = null;
            Exception callException = null;

            ClientHelper.Post<List<FuelReportInventoryOperationDto>, CharterDto>
                //(new Uri("http://localhost:65234/api/fuelevents", UriKind.Absolute),
                (new Uri("http://evaluation-srv:9090/api/fuelevents", UriKind.Absolute),
                (result, exp) =>
                {
                    callException = exp;
                    if (callResult != null)
                        callResult = result.Select(mapInventoryOperationDtoToInventoryOperation).ToList();

                    syncEvent.Set();

                    //if (exp != null) tcs.TrySetException(exp);
                    //else if (result != null)
                    //{
                    //    var mappedResult = result.Select(mapInventoryOperationDtoToInventoryOperation).ToList();
                    //    tcs.TrySetResult(mappedResult);
                    //}
                },
                charterInDto, ClientHelper.MessageFormat.Json, new Dictionary<string, string>(), "CharterDto");

            /////var inventoryOperationDtosResult.Select(mapInventoryOperationDtoToInventoryOperation).ToList();
            syncEvent.WaitOne();


            Assert.IsNull(callException);

            Assert.IsNotNull(callResult);
        }

        private InventoryOperation mapInventoryOperationDtoToInventoryOperation(FuelReportInventoryOperationDto arg)
        {
            var result = new InventoryOperation(arg.Code, arg.ActionDate, mapInventoryActionType(arg.ActionType), null, null);
            return result;
        }

        private Domain.Model.Enums.InventoryActionType mapInventoryActionType(string actionType)
        {
            return (InventoryActionType)Enum.Parse(typeof(InventoryActionType), actionType);
        }
    }
}
