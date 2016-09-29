using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Enums.Inventory;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices.Inventory;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Domain.Model.Specifications;

namespace MITD.Fuel.Integration.Inventory.Infrastructure
{
    public static class ExtensionMethods
    {
        public static List<Issue> CreateIssueDataForFinanceArticles(this Inventory_Transaction operationTransaction, long companyId, IGoodRepository goodRepository)
        {
            var articleIssueParameter = new List<Issue>();

            if (operationTransaction.Action != (byte)TransactionType.Issue)
                throw new InvalidArgument("Type of inventory operation to create Finance articles is invalid", "operationTransaction");


            foreach (var transactionItem in operationTransaction.Inventory_TransactionItem)
            {
                var good = goodRepository.Single(g => g.SharedGoodId == transactionItem.GoodId && g.CompanyId == companyId);

                //issueArticleToFinance.AddRange(
                //    transactionItem.Inventory_TransactionItemPrice.Select(
                //                tip =>
                //                    new Issue(0, good.Id, (int)tip.QuantityAmount.Value, tip.Fee.Value, tip.FeeInMainCurrency.Value / tip.Fee.Value, tip.Inventory_Unit_QuantityUnit.Name, transactionItem.Inventory_Good.Name, issueTransaction.RegistrationDate.Value, tip.PriceUnitId, tip.Inventory_Unit_PriceUnit.Name)).ToList());

                articleIssueParameter.AddRange(transactionItem.Inventory_TransactionItemPrice.GroupBy(
                    tip => new
                    {
                        Code = tip.Inventory_TransactionItem.Inventory_Transaction.Code,
                        RegistrationDate = tip.Inventory_TransactionItem.Inventory_Transaction.RegistrationDate,
                        Action = tip.Inventory_TransactionItem.Inventory_Transaction.Action,
                        QuantityUnitId = tip.QuantityUnitId,
                        QuantityUnitAbbreviation = tip.Inventory_Unit_QuantityUnit.Abbreviation,
                        QuantityUnitName = tip.Inventory_Unit_QuantityUnit.Name,
                        PriceUnitId = tip.PriceUnitId,
                        PriceUnitAbbreviation = tip.Inventory_Unit_PriceUnit.Abbreviation,
                        PriceUnitName = tip.Inventory_Unit_PriceUnit.Name,
                        MainCurrencyUnitId = tip.MainCurrencyUnitId,
                        MainCurrencyAbbreviation = tip.Inventory_Unit_MainCurrencyUnit.Abbreviation,
                        MainCurrencyName = tip.Inventory_Unit_MainCurrencyUnit.Name,
                        //SUM(c.QuantityAmountPriced) AS QuantityAmountPriced, 
                        Fee = tip.Fee,
                        FeeInMainCurrency = tip.FeeInMainCurrency
                        //SUM(c.TotalPrice) AS TotalPrice

                    },
                    (key, groupedItemPrices) =>
                        new Issue(0, good.Id, groupedItemPrices.Sum(ti => ti.QuantityAmount.Value), key.Fee.Value, key.Fee.Value == 0 ? 0 : key.FeeInMainCurrency.Value / key.Fee.Value,
                            key.QuantityUnitName,
                            transactionItem.Inventory_Good.Name,
                            operationTransaction.RegistrationDate.Value,
                            key.PriceUnitId, key.PriceUnitName, transactionItem.Id)
                    ).ToList());

                //issueArticleToFinance.AddRange(transactionItem.Inventory_TransactionItemPrice.GroupBy(
                //   groupingKeySelector,
                //   (key, groupedItemPrices) =>
                //       new Issue(0, good.Id, groupedItemPrices.Sum(ti => ti.QuantityAmount.Value), key.Fee.Value, key.FeeInMainCurrency.Value / key.Fee.Value,
                //           key.QuantityUnitName,
                //           transactionItem.Inventory_Good.Name,
                //           issueTransaction.RegistrationDate.Value,
                //           key.PriceUnitId, key.PriceUnitName)
                //       ).ToList());
            }

            return articleIssueParameter;
        }

        public static List<Receipt> CreateReceiptDataForFinanceArticles(this Inventory_Transaction operationTransaction, long companyId, IGoodRepository goodRepository)
        {
            var articleIssueParameter = new List<Receipt>();

            if (operationTransaction.Action != (byte)TransactionType.Receipt)
                throw new InvalidArgument("Type of inventory operation to create Finance articles is invalid", "operationTransaction");


            foreach (var transactionItem in operationTransaction.Inventory_TransactionItem)
            {
                var good = goodRepository.Single(g => g.SharedGoodId == transactionItem.GoodId && g.CompanyId == companyId);


                articleIssueParameter.AddRange(transactionItem.Inventory_TransactionItemPrice.GroupBy(
                    tip => new
                    {

                        Code = tip.Inventory_TransactionItem.Inventory_Transaction.Code,
                        RegistrationDate = tip.Inventory_TransactionItem.Inventory_Transaction.RegistrationDate,
                        Action = tip.Inventory_TransactionItem.Inventory_Transaction.Action,
                        QuantityUnitId = tip.QuantityUnitId,
                        QuantityUnitAbbreviation = tip.Inventory_Unit_QuantityUnit.Abbreviation,
                        QuantityUnitName = tip.Inventory_Unit_QuantityUnit.Name,
                        PriceUnitId = tip.PriceUnitId,
                        PriceUnitAbbreviation = tip.Inventory_Unit_PriceUnit.Abbreviation,
                        PriceUnitName = tip.Inventory_Unit_PriceUnit.Name,
                        MainCurrencyUnitId = tip.MainCurrencyUnitId,
                        MainCurrencyAbbreviation = tip.Inventory_Unit_MainCurrencyUnit.Abbreviation,
                        MainCurrencyName = tip.Inventory_Unit_MainCurrencyUnit.Name,
                        //SUM(c.QuantityAmountPriced) AS QuantityAmountPriced, 
                        Fee = tip.Fee,
                        FeeInMainCurrency = tip.FeeInMainCurrency
                        //SUM(c.TotalPrice) AS TotalPrice

                    },
                    (key, groupedItemPrices) =>
                        new Receipt(0, good.Id, groupedItemPrices.Sum(ti => ti.QuantityAmount.Value), key.Fee.Value, key.Fee.Value == 0 ? 0 : key.FeeInMainCurrency.Value / key.Fee.Value,
                            key.QuantityUnitName,
                            transactionItem.Inventory_Good.Name,
                            operationTransaction.RegistrationDate.Value,
                            key.PriceUnitId, key.PriceUnitName, transactionItem.Id)
                    ).ToList());


            }

            return articleIssueParameter;
        }

        public static bool IsFullyPriced(this Inventory_Transaction inventoryTransaction)
        {
            var isTransactionFullyPriced = new IsTransactionFullyPriced();
            return isTransactionFullyPriced.IsSatisfiedBy(inventoryTransaction);
        }

        public static Issue CreateDifferentialIssueDataForFinanceArticles(this Inventory_Transaction updatedTransaction, Inventory_Transaction originalTransaction, long companyId, long sharedGoodId, IGoodRepository goodRepository)
        {
            var updatedTransactionItem = updatedTransaction.Inventory_TransactionItem.SingleOrDefault(ti => ti.GoodId == sharedGoodId);
            var originalTransactionItem = originalTransaction.Inventory_TransactionItem.SingleOrDefault(ti => ti.GoodId == sharedGoodId);

            if (updatedTransactionItem == null || originalTransactionItem == null)
            {
                throw new ObjectNotFound("No Transaction Item found for good.", sharedGoodId);
            }

            var good = goodRepository.Single(g => g.SharedGoodId == updatedTransactionItem.GoodId && g.CompanyId == companyId);

            var sourceQuantity = originalTransactionItem.Inventory_TransactionItemPrice.Sum(tip => tip.QuantityAmount.Value);
            var updatedQuantity = updatedTransactionItem.Inventory_TransactionItemPrice.Sum(tip => tip.QuantityAmount.Value);

            if (updatedQuantity > sourceQuantity)
                throw new BusinessRuleException("", "The updated quantity to calculate differential issue voucher quantity is greater than original value.");

            return new Issue(0, good.Id, sourceQuantity - updatedQuantity,
                        updatedTransactionItem.Inventory_TransactionItemPrice.First().FeeInMainCurrency.Value,
                        1, //Because the Fee in Main currency is used, the exchange rate is set to 1.
                        updatedTransactionItem.Inventory_TransactionItemPrice.First().Inventory_Unit_QuantityUnit.Name,
                        updatedTransactionItem.Inventory_Good.Name,
                        updatedTransaction.RegistrationDate.Value,
                        updatedTransactionItem.Inventory_TransactionItemPrice.First().MainCurrencyUnitId,
                        updatedTransactionItem.Inventory_TransactionItemPrice.First().Inventory_Unit_MainCurrencyUnit.Name, updatedTransactionItem.Id);

        }

        public static Receipt CreateDifferentialReceiptDataForFinanceArticles(this Inventory_Transaction updatedTransaction, Inventory_Transaction originalTransaction, long companyId, long sharedGoodId, IGoodRepository goodRepository)
        {
            var updatedTransactionItem = updatedTransaction.Inventory_TransactionItem.SingleOrDefault(ti => ti.GoodId == sharedGoodId);
            var originalTransactionItem = originalTransaction.Inventory_TransactionItem.SingleOrDefault(ti => ti.GoodId == sharedGoodId);

            if (updatedTransactionItem == null || originalTransactionItem == null)
            {
                throw new ObjectNotFound("No Transaction Item found for good.", sharedGoodId);
            }

            var good = goodRepository.Single(g => g.SharedGoodId == updatedTransactionItem.GoodId && g.CompanyId == companyId);

            var sourceQuantity = originalTransactionItem.Inventory_TransactionItemPrice.Sum(tip => tip.QuantityAmount.Value);
            var updatedQuantity = updatedTransactionItem.Inventory_TransactionItemPrice.Sum(tip => tip.QuantityAmount.Value);

            if (updatedQuantity < sourceQuantity)
                throw new BusinessRuleException("", "The updated quantity to calculate differential receipt voucher quantity is less than original value.");

            return new Receipt(0, good.Id, updatedQuantity - sourceQuantity,
                        updatedTransactionItem.Inventory_TransactionItemPrice.First().FeeInMainCurrency.Value,
                        1, //Because the Fee in Main currency is used, the exchange rate is set to 1.
                        updatedTransactionItem.Inventory_TransactionItemPrice.First().Inventory_Unit_QuantityUnit.Name,
                        updatedTransactionItem.Inventory_Good.Name,
                        updatedTransaction.RegistrationDate.Value,
                        updatedTransactionItem.Inventory_TransactionItemPrice.First().MainCurrencyUnitId,
                        updatedTransactionItem.Inventory_TransactionItemPrice.First().Inventory_Unit_MainCurrencyUnit.Name, updatedTransactionItem.Id);


        }

        public static T Clone<T>(this T source)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static decimal CalculateSingedTotalPriceInMainCurrency(this List<GoodQuantityPricing> source)
        {
            return source.Sum(i => i.FeeInMainCurrency * i.SignedQuantity);
        }

        //public static decimal CalculateUnsingedTotalPriceInMainCurrency(this List<GoodQuantityPricing> source)
        //{
        //    return Math.Abs(CalculateSingedTotalPriceInMainCurrency(source));
        //}

        public static bool IsIssueWithFIFOPricing(this Inventory_Transaction source)
        {
            return ((InventoryOperationType)source.Action == InventoryOperationType.Issue) && !source.PricingReferenceId.HasValue;
        }

        public static string GetInventoryTransactionReferenceNumber(this FuelReport source)
        {
            return source.Id.ToString();
        }

        public static string GetInventoryTransactionReferenceNumber(this FuelReportDetail source)
        {
            return source.Id.ToString();
        }

        public static string GetInventoryTransactionReferenceNumber(this CharterIn source)
        {
            return source.Id.ToString();
        }

        public static string GetInventoryTransactionReferenceNumber(this CharterOut source)
        {
            return source.Id.ToString();
        }

        public static string GetInventoryTransactionReferenceNumber(this Scrap source)
        {
            return source.Id.ToString();
        }

        public static string GetOrderItemBalancePricingReferenceNumber(this OrderItemBalance source)
        {
            return String.Format("{0},{1}", source.FuelReportDetailId, source.InvoiceItemId);
        }
    }
}
