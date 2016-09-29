using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using MITD.AutomaticVoucher.Data;
using MITD.AutomaticVoucher.SAPIDFinancialService;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.AutomaticVoucher.FinancialService
{
    public class SAPIDFinancialVoucherService
    {
        private IVoucherRepository _voucherRepository;
        private IUnitOfWorkScope _unitOfWorkScope;

        public SAPIDFinancialVoucherService(IVoucherRepository voucherRepository, IUnitOfWorkScope unitOfWorkScope)
        {
            _voucherRepository = voucherRepository;
            _unitOfWorkScope = unitOfWorkScope;
        }

        public void Send(List<Voucher> vouchers, string date, string departmenCode, long userId)
        {

            var acc = new AccServiceClient();
            var svcHeadersData = new svcHeadersData();
            var header = new svcHeader();


            try
            {
                using (var trans = new TransactionScope())
                {
                    var duration = acc.getDuration("FMS", "1234", date);
                    #region Header

                    header.department = departmenCode;
                    header.doSortDbCr = true;
                    //header.domain = "";
                    //var des = "";
                    //vouchers.ForEach(c => des += c.Description + " , ");
                    //header.explain = des;
                    header.explain = DesCreator(vouchers);
                    header.reference = "";
                    header.userSequence = 0;
                    header.voucherDate = date;
                    header.voucherNumber = 0;
                    header.voucherType = -5;

                    #endregion

                    #region Articls

                    var Articls = new List<svcArticle>();
                    vouchers.ForEach(v =>
                    {
                        int i = 0;
                        int cntr = 0;
                        v.JournalEntrieses.ForEach(j =>
                        {

                            var article = new svcArticle();
                            article.accountCode = j.AccountNo;
                            var voy = "";
                            var port = "";
                            var vessel = "";
                            foreach (var seg in j.Segments)
                            {
                                if (seg.SegmentType.Id == SegmentType.Company.Id)
                                {
                                    article.accountFreeCode = seg.Code;
                                    article.addonString = null;
                                    break;
                                }
                                else
                                {

                                    if (seg.SegmentType.Id == SegmentType.Voayage.Id)
                                        voy = seg.Code + ";";
                                    if (seg.SegmentType.Id == SegmentType.Port.Id)
                                        port = seg.Code;
                                    if (seg.SegmentType.Id == SegmentType.Vessel.Id)

                                        vessel = seg.Code;


                                    article.accountFreeCode = null;

                                }
                                if (!string.IsNullOrEmpty(vessel))
                                    article.addonString = vessel;
                                else
                                {
                                    article.addonString = voy + port;
                                }

                            }




                            //article.accountFreeGroupCode

                            article.amount = (j.IsCredit) ? (Decimal.Round(j.IrrAmount) * -1) : Decimal.Round(j.IrrAmount);
                            article.amountSpecified = true;
                            //article.articleParam
                            article.collect = null;
                            article.currencyAmount = (j.IsCredit) ? (Decimal.Round(j.ForeignAmount, 2) * -1) : Decimal.Round(j.ForeignAmount, 2);
                            article.currencyAmountSpecified = true;
                            article.currencyCode = j.Currency.Abbreviation;
                            article.reference = (++cntr).ToString() + j.VoucherRef;
                            //فقط برای اسناد رسید خرید
                            if (v.VoucherTypeId == 1 || v.VoucherTypeId == 4)
                            {
                                if (j.IsCredit && j.Segments[0].SegmentType.Id == SegmentType.Company.Id)
                                    article.dbman = new svcDbman()
                                    {
                                        dbmanDate = date.ToString(),
                                        dueDate = date.ToString(),
                                        planDate = null,
                                        deadlineDate = date.ToString(),
                                        benefCode = j.Segments[0].Code,
                                        priorityID = 1,
                                        amount = j.IrrAmount,
                                        //amountSpecified = 
                                        //currencyCode = 

                                        reference = (cntr).ToString() + j.VoucherRef,
                                        //remark = 
                                        //ref2 = 
                                        //ref3 = 
                                        //ref4 = 
                                        //ref5 = 
                                        dbmanTypeCode = "00012"

                                    };
                            }
                            //article.effectiveDate
                            article.explain = j.Description;
                            //article.explainPriority
                            article.purposeID = ++i;
                          
                            //article.tag
                            Articls.Add(article);
                        });
                    });


                    header.articles = Articls.ToArray();

                    #endregion

                    svcHeadersData.headers = new svcHeader[1];
                    svcHeadersData.headers[0] = header;

                    var sessionKey = acc.login("FMS", "1234", duration.dataBaseStartDate);
                    var result = acc.saveHeader(svcHeadersData, sessionKey);
                    acc.logout(sessionKey);

                    if (result[0].errorCode > 0)
                    {
                        vouchers.ForEach(c =>
                        {
                            var res = _voucherRepository.FindByKey(c.Id);
                            res.FinancialVoucherState = 3;

                        });
                        var service = new VoucherTransferLogService();
                        service.Add(vouchers, date, departmenCode, result[0].errorCode + " : " + result[0].errorCode, userId);
                    }
                    else
                    {
                        vouchers.ForEach(c =>
                        {
                            var res = _voucherRepository.FindByKey(c.Id);
                            res.FinancialVoucherNo = result[0].reference;
                            res.FinancialVoucherState = 1;
                            res.FinancialVoucherSendingUserId = userId;
                            res.FinancialVoucherDate = date;
                        });

                    }
                    _unitOfWorkScope.Commit();
                    trans.Complete();
                }
            }
            catch (Exception ex)
            {
                vouchers.ForEach(c =>
                {
                    var res = _voucherRepository.FindByKey(c.Id);
                    res.FinancialVoucherState = 3;
                });
                _unitOfWorkScope.Commit();

                var service = new VoucherTransferLogService();
                var message = "-->Message : " + ex.Message + "\n -->InnerException : " + ((ex.InnerException != null)
                    ? ex.InnerException.Message
                    : "" + "\n" + GetValue(header));
                service.Add(vouchers, date, departmenCode, message, userId);

                //  throw ex;
            }


        }

        public string GetValue(svcHeader header)
        {
            var res = "\n Header \n";
            foreach (var prp in header.GetType().GetProperties())
            {
                res += prp.Name + " : " + prp.GetValue(header) + " ; ";
            }

            header.articles.ToList().ForEach(c =>
            {
                res += "\n Article --> ";
                foreach (var re in c.GetType().GetProperties())
                {
                    res += re.Name + " : " + re.GetValue(c) + " ; ";
                }
            });
            return res;
        }

        string DesCreator(List<Voucher> vouchers)
        {

            string res = "";
            vouchers.ForEach(c =>
            {
                res += DesSelctor(c.VoucherDetailTypeId) + " , ";
            });

            return res;

        }

        string DesSelctor(int typeid)
        {
            var res = "";
            switch (typeid)
            {
                case 1:
                    {
                        res = "رسید Charter In Start";
                        break;
                    }
                case 2:
                    {
                        res = "حواله Charter Out Start";
                        break;
                    }
                case 3:
                    {
                        res = "حواله مصرف پایان سفر";
                        break;
                    }
                case 4:
                    {
                        res = "حواله مصرف پایان سال";
                        break;
                    }
                case 5:
                    {
                        res = "حواله مصرف پایان ماه";
                        break;
                    }
                case 6:
                    {
                        res = "رسید خرید";
                        break;
                    }
                case 7:
                    {
                        res = "حواله Charter In End";
                        break;
                    }
                case 8:
                    {
                        res = "رسید Charter Out End";
                        break;
                    }
                case 9:
                    {
                        res = "Offhire Charter Out";
                        break;
                    }
                case 10:
                    {
                        res = "Offhire Charter In";
                        break;
                    }
                case 11:
                    {
                        res = "Plus Correction";
                        break;
                    }
                case 12:
                    {
                        res = "Mints Correction";
                        break;
                    }
                case 13:
                    {
                        res = "حواله مصرف CharterOutStart Variance ";
                        break;
                    }
                case 14:
                    {
                        res = "حواله مصرف CharterInEnd Variance";
                        break;
                    }
                case 15:
                    {
                        res = "انتقال (فروش انتقالی)";
                        break;
                    }




            }
            return res;

        }
    }
}
