using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.Enums;
using NHibernate.Linq;
using Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.PredefinedTransformations;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
    public class VoucherDetailType
    {

        #region Prop
        
        public int Id { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }

        public VoucherType VoucherType { get; set; }


        public static VoucherDetailType CharterInStart {
            get {return new VoucherDetailType(1,"CharterInStart","01" ); }
        }

        public static VoucherDetailType CharterOutStart
        {
            get { return new VoucherDetailType(2, "CharterOutStart", "02"); }
        }
        public static VoucherDetailType EndOfVoyageFuelReport
        {
            get { return new VoucherDetailType(3, "EndOfVoyageFuelReport", "03"); }
        }
        public static VoucherDetailType EndOfYearFuelReport
        {
            get { return new VoucherDetailType(4, "EndOfYearFuelReport", "04"); }
        }
        public static VoucherDetailType EndOfMonthFuelReport
        {
            get { return new VoucherDetailType(5, "EndOfMonthFuelReport", "05"); }
        }

        public static VoucherDetailType PurchesInvoice
        {
            get { return new VoucherDetailType(6, "PurchesInvoice", "06"); }
        }


        public static VoucherDetailType CharterInEnd
        {
            get { return new VoucherDetailType(7, "CharterInEnd", "07"); }
        }

        public static VoucherDetailType CharterOutEnd
        {
            get { return new VoucherDetailType(8, "CharterOutEnd", "08"); }
        }

        public static VoucherDetailType OffhireCharterIn
        {
            get { return new VoucherDetailType(9, "OffhireCharterIn", "09"); }
        }

        public static VoucherDetailType OffhireCharterOut
        {
            get { return new VoucherDetailType(10, "OffhireCharterOut", "10"); }
        }

        public static VoucherDetailType PlusCorrection
        {
            get { return new VoucherDetailType(11, "PlusCorrection", "11"); }
        }

        public static VoucherDetailType MintsCorrection
        {
            get { return new VoucherDetailType(12, "MintsCorrection", "12"); }
        }
        public static VoucherDetailType CharterOutVariance
        {
            get { return new VoucherDetailType(13, "CharterOutStartVariance", "13"); }
        }
        public static VoucherDetailType CharterInVariance
        {
            get { return new VoucherDetailType(14, "CharterInEndVariance", "14"); }
        }

        public static VoucherDetailType Transfer
        {
            get { return new VoucherDetailType(15, "Transfer", "15"); }
        }
        public static VoucherDetailType InvoiceTransfer
        {
            get { return new VoucherDetailType(16, "InvoiceTransfer", "16"); }
        }
        public static VoucherDetailType CharterInStartVariance
        {
            get { return new VoucherDetailType(17, "CharterInStartVariance", "17"); }
        }
        #endregion


        #region Ctor

        public VoucherDetailType(int id,string name,string code)
        {
            this.Id = id;
            this.Name = name;
            this.Code = code;
            
        
        }

        #endregion

        #region Method

        public  void SetVoucherType(VoucherType voucherType)
        {
            VoucherType = voucherType;
        }

        #endregion


    }
}
