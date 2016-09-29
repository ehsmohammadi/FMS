//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAmendments
{
    using System;
    
    public partial class TransactionItemsGetAll_Result
    {
        public int tiId { get; set; }
        public short tiRowVersion { get; set; }
        public long tiGoodId { get; set; }
        public long tiQuantityUnitId { get; set; }
        public Nullable<decimal> tiQuantityAmount { get; set; }
        public string tiDescription { get; set; }
        public Nullable<int> tiUserCreatorId { get; set; }
        public Nullable<System.DateTime> tiCreateDate { get; set; }
        public int tId { get; set; }
        public byte tAction { get; set; }
        public Nullable<decimal> tCode { get; set; }
        public string tDescription { get; set; }
        public Nullable<int> tPricingReferenceId { get; set; }
        public long tWarehouseId { get; set; }
        public int tStoreTypesId { get; set; }
        public int tTimeBucketId { get; set; }
        public Nullable<byte> tStatus { get; set; }
        public Nullable<System.DateTime> tRegistrationDate { get; set; }
        public Nullable<int> tSenderReciver { get; set; }
        public string tHardCopyNo { get; set; }
        public string tReferenceType { get; set; }
        public string tReferenceNo { get; set; }
        public Nullable<System.DateTime> tReferenceDate { get; set; }
        public Nullable<int> tUserCreatorId { get; set; }
        public Nullable<System.DateTime> tCreateDate { get; set; }
        public short StoreTypeCode { get; set; }
        public string StoreTypeInputName { get; set; }
        public string StoreTypeOutputName { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public long CompanyId { get; set; }
        public Nullable<bool> WarehouseStatus { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public Nullable<bool> CompanyStatus { get; set; }
        public string GoodCode { get; set; }
        public string GoodName { get; set; }
        public bool GoodStatus { get; set; }
        public string UnitAbbreviation { get; set; }
        public string UnitName { get; set; }
        public Nullable<bool> UnitIsCurrency { get; set; }
        public Nullable<bool> UnitIsBaseCurrency { get; set; }
        public bool UnitStatus { get; set; }
    }
}