using System;
using System.IO;
using System.Text;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Logic.SL.ServiceWrapper
{
    public class InventoryTransactionServiceWrapper : IInventoryTransactionServiceWrapper
    {
        private readonly string transactionAddressFormatString;

        private readonly string transactionDetailAddressFormatString;
        
        private readonly string transactionDetailPriceAddressFormatString;

        public InventoryTransactionServiceWrapper()
        {
            this.transactionAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Inventory/InventoryTransaction/{0}");

            this.transactionDetailAddressFormatString = string.Concat(this.transactionAddressFormatString, "/Detail/{1}");
            
            this.transactionDetailPriceAddressFormatString = string.Concat(this.transactionDetailAddressFormatString, "/Price/{2}");
        }
        
        //================================================================================

        public void GetById(Action<Inventory_TransactionDto, Exception> action, long id)
        {
            var url = string.Format(this.transactionAddressFormatString, id);

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(url), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void GetPagedTransactionData(Action<PageResultDto<Inventory_TransactionDto>, Exception> action, int pageSize, int pageIndex)
        {
            var url = string.Format(this.transactionAddressFormatString, string.Empty);

            var sbUrl = new StringBuilder(url);
            sbUrl.Append(string.Concat("?pageSize=", pageSize));
            sbUrl.Append(string.Concat("&pageIndex=", pageIndex));

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(sbUrl), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void GetPagedTransactionDataByFilter(Action<PageResultDto<Inventory_TransactionDto>, Exception> action, long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, byte? transactionType, byte? status, decimal? inventoryCode, int pageSize, int pageIndex)
        {
            var url = string.Format(this.transactionAddressFormatString, string.Empty);

            var sbUrl = new StringBuilder(url);
            sbUrl.Append(string.Concat("?companyId=", companyId));
            sbUrl.Append(string.Concat("&warehouseId=", warehouseId));
            sbUrl.Append(string.Concat("&fromDate=", HttpUtil.DateTimeToString(fromDate)));
            sbUrl.Append(string.Concat("&toDate=", toDate.HasValue ? HttpUtil.DateTimeToString(toDate.Value.AddDays(1)) : string.Empty));
            sbUrl.Append(string.Concat("&transactionType=", transactionType));
            sbUrl.Append(string.Concat("&status=", status));
            sbUrl.Append(string.Concat("&inventoryCode=", inventoryCode));
            sbUrl.Append(string.Concat("&pageSize=", pageSize));
            sbUrl.Append(string.Concat("&pageIndex=", pageIndex));

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(sbUrl), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void GetTransactionDetailDataByFilter(Action<Inventory_TransactionDetailDto, Exception> action, long transactionId, long transactionDetailId)
        {
            var url = string.Format(this.transactionDetailAddressFormatString, transactionId, transactionDetailId);

            var sbUrl = new StringBuilder(url);
            //sbUrl.Append(string.Concat("?transactionDetailId=", transactionDetailId));

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(sbUrl), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        //================================================================================

        public void GetPagedTransactionDetailDataByFilter(Action<PageResultDto<Inventory_TransactionDetailDto>, Exception> action, long transactionId, int pageSize, int pageIndex)
        {
            var url = string.Format(this.transactionDetailAddressFormatString, transactionId, string.Empty);

            var sbUrl = new StringBuilder(url);
            sbUrl.Append(string.Concat("?pageSize=", pageSize));
            sbUrl.Append(string.Concat("&pageIndex=", pageIndex));

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(sbUrl), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //================================================================================

        public void GetTransactionDetailPriceDataByFilter(Action<Inventory_TransactionDetailPriceDto, Exception> action, long transactionId, long transactionDetailId, long transactionDetailPriceId)
        {
            var url = string.Format(this.transactionDetailPriceAddressFormatString, transactionId, transactionDetailId, transactionDetailPriceId);

            var sbUrl = new StringBuilder(url);

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(sbUrl), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        //================================================================================

        public void GetPagedTransactionDetailPriceDataByFilter(Action<PageResultDto<Inventory_TransactionDetailPriceDto>, Exception> action, 
                                                        long transactionId, long transactionDetailId, int pageSize, 
                                                        int pageIndex)
        {
            var url = string.Format(this.transactionDetailPriceAddressFormatString, transactionId, transactionDetailId, string.Empty);

            var sbUrl = new StringBuilder(url);
            //sbUrl.Append(string.Concat("?transactionDetailId=", transactionDetailId));
            sbUrl.Append(string.Concat("?pageSize=", pageSize));
            sbUrl.Append(string.Concat("&pageIndex=", pageIndex));

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(sbUrl), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }
        //================================================================================
        public void PricingTransaction(Action<string, Exception> action, long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType)
        {
            var url = string.Format(this.transactionAddressFormatString,0);
            var sbUrl = new StringBuilder(url);
            sbUrl.Append("/TransactionPricing");
            sbUrl.Append(string.Concat("?companyId=", companyId));
            sbUrl.Append(string.Concat("&warehouseId=", warehouseId));
            sbUrl.Append(string.Concat("&fromDate=", HttpUtil.DateTimeToString(fromDate)));
            sbUrl.Append(string.Concat("&toDate=", toDate.HasValue ? HttpUtil.DateTimeToString(toDate.Value.AddDays(1)) : string.Empty));
            sbUrl.Append(string.Concat("&transactionType=", transactionType));

            WebClientHelper.Put(ApiServiceAddressHelper.BuildUri(sbUrl), action, (object)null, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }
        
        //================================================================================
        public void CreateVoucherForTransactions(Action<string, Exception> action, long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType)
        {
            var url = string.Format(this.transactionAddressFormatString,0);
            var sbUrl = new StringBuilder(url);
            sbUrl.Append("/RegisterVoucher");
            sbUrl.Append(string.Concat("?companyId=", companyId));
            sbUrl.Append(string.Concat("&warehouseId=", warehouseId));
            sbUrl.Append(string.Concat("&fromDate=", HttpUtil.DateTimeToString(fromDate)));
            sbUrl.Append(string.Concat("&toDate=", toDate.HasValue ? HttpUtil.DateTimeToString(toDate.Value.AddDays(1)) : string.Empty));
            sbUrl.Append(string.Concat("&transactionType=", transactionType));

            WebClientHelper.Put(ApiServiceAddressHelper.BuildUri(sbUrl), action, (object)null, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }


    }
}