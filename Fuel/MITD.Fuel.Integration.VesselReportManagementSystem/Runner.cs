
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MITD.Fuel.Integration.VesselReportManagementSystem.Data;
using MITD.Fuel.Integration.VesselReportManagementSystem.ServiceWrapper;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;

namespace MITD.Fuel.Integration.VesselReportManagementSystem
{
    public class Runner
    {
        private VoyageCostEntities db;
        private ServiceWrapper.VesselReportServiceWrapper _reportServiceWrapper;
        public Runner()
        {
            db = new VoyageCostEntities();
        }

        public void Start()
        {
            try
            {
                var resultReport = new ResultFuelReportDto();

                _reportServiceWrapper = new VesselReportServiceWrapper();

                var lst = GetReports();
                WriteMessage(true, lst.Count);

                foreach (var report in lst)
                {
                    try
                    {
                        var syncEvent = new AutoResetEvent(false);

                        Exception resultException = null;
                        
                        _reportServiceWrapper.Add((res, exception) =>{
                            resultException = exception;

                            resultReport = res;

                            syncEvent.Set();


                        }, Mapper.ReportMapper.Map(report));

                        syncEvent.WaitOne();

                        if (resultReport != null)
                        {
                            if (resultReport.Type == ResultType.Exception)
                            {
                                report.State = DateTime.Now.Date.Subtract(report.DateIn.GetValueOrDefault(DateTime.Now)) <= TimeSpan.FromDays(10) ? (byte)resultReport.Type : (byte)ResultType.ExceptionLimitExceeded;

                                report.ErrorMessage = resultReport.Message;
                                
                                db.SaveChanges();

                                //For debug mode, uncomment following records.
                                //var serverException = new Exception(resultReport.Message);
                                //WriteMessage(serverException);
                            }
                            else
                            {
                                report.State = (byte) resultReport.Type;

                                db.SaveChanges();

                                //var previousState = report.State;
                                //WriteMessage(report.ID.ToString(),
                                //             previousState.HasValue ? ((ResultType) previousState.Value).ToString() : string.Empty,
                                //             ((ResultType) resultReport.Type).ToString(),
                                //             resultReport.Message);
                            }
                        }
                        else if (resultException != null)
                        {
                            var exception = ExtractException(resultException);

                            throw exception;
                        }
                    }
                    catch (DbEntityValidationException dbException)
                    {
                        var errorMessage =
                            dbException.EntityValidationErrors.Aggregate<DbEntityValidationResult, string>("", (result, element) => result + element.ValidationErrors.Aggregate<DbValidationError, string>("", (p1, p2) => p1 + "=>" + p2.ErrorMessage)) + "  \n " + dbException.Message;

                        var ex = new Exception(errorMessage, dbException);
                        LogService(ex, report.ID);
                        WriteMessage(ex);
                    }
                    catch (Exception ex)
                    {
                        report.State = (int) ResultType.Failure;

                        report.ErrorMessage = ex.Message;
                        db.SaveChanges();

                        //WriteMessage(ex);
                    }

                    Console.WriteLine("=========================================================================");
                }

                //WriteMessage(false, 0);
            }
            catch (DbEntityValidationException dbException)
            {
                var errorMessage =
                    dbException.EntityValidationErrors.Aggregate<DbEntityValidationResult, string>("", (result, element) => result + element.ValidationErrors.Aggregate<DbValidationError, string>("", (p1, p2) => p1 + "=>" + p2.ErrorMessage)) + "  \n " + dbException.Message;

                var ex = new Exception(errorMessage, dbException);
                LogService(ex, -1000);
                //WriteMessage(ex);
            }
            catch (Exception ex)
            {
                LogService(ex, -1000);
                //WriteMessage(ex);
            }
        }

        private List<Data.EventReport> GetReports()
        {
            var reportResult = db.EventReports.Where(c =>
                (!c.State.HasValue ||
                c.State == (byte)ResultType.Exception ||
                c.State == (byte)ResultType.New ||
                c.State == (byte)ResultType.Failure) && ((c.Year == 2014 && c.Month >= 6) || c.Year >= 2015) ).OrderBy(c => new { c.Year, c.Month, c.Day, c.Time }).ToList();

            return reportResult.ToList();
        }

        private void WriteMessage(string recordId, string previousState, string currentState, string message)
        {
            var stringBuilder = new StringBuilder();


            var msg = string.Format("Record Id : {0} ,Previous State: {1} ,Current State: {2} ", recordId, previousState,
           currentState);

            stringBuilder.AppendLine(msg);
            if (!String.IsNullOrEmpty(message))
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                stringBuilder.AppendLine("Message : " + message);
                System.Console.ResetColor();
            }
            //stringBuilder.AppendLine("=========================================================================");
            System.Console.WriteLine(stringBuilder);
        }

        private void WriteMessage(bool startFlag, int count)
        {
            if (startFlag)
                System.Console.WriteLine("Count Of Record : {0}   Start Proccess ........", count.ToString());
            else
            {
                System.Console.WriteLine("====================== End of Proccess ======================");
            }
        }

        private void WriteMessage(Exception ex)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("System Failure ==> ");
            System.Console.ResetColor();
            System.Console.WriteLine(ex.Message);

            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("Inner Exception ==> ");
            System.Console.ResetColor();
            System.Console.WriteLine(ex.InnerException);
            
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("Stack Trace ==> ");
            System.Console.ResetColor();
            System.Console.WriteLine(ex.StackTrace);
        }

        public Exception ExtractException(Exception webException)
        {
            if (webException.Data.Contains("error"))
            {
                try
                {
                    var exceptionMessageDto =
                         Newtonsoft.Json.JsonConvert.DeserializeObject<ExceptionMessageDto>(webException.Data["error"] as string);

                    var exception = new Exception(exceptionMessageDto.Message);
                    return exception;
                }
                catch
                {
                    return webException;
                }
            }
            else
            {
                return webException;
            }
        }

        private void LogService(Exception ex, long recordId)
        {
            try
            {
                FuelReportLog fuelReportLog = new FuelReportLog()
                {
                    Date = DateTime.Now,
                    FailureMessage = ex.Message.Substring(0, Math.Min(ex.Message.Length, 4000)),
                    RecordId = recordId,
                    StackTrace = ex.StackTrace ?? String.Empty

                };

                db.FuelReportLogs.Add(fuelReportLog);
                db.SaveChanges();
            }
            catch (Exception exp)
            {
                //WriteMessage(exp);
            }


        }

        int? GetMaxLength(DbContext context, string tableName, string propertyName)
        {
            var oc = ((IObjectContextAdapter)context).ObjectContext;

            return oc.MetadataWorkspace.GetItems(DataSpace.CSpace).OfType<EntityType>()
                     .Where(et => et.Name == tableName)
                     .SelectMany(et => et.Properties.Where(p => p.Name == propertyName))
                     .Select(p => p.MaxLength)
                     .FirstOrDefault();
        }

        int? GetMaxLength<T>(DbContext context, Expression<Func<T, object>> property)
        {
            var memberExpression = (MemberExpression)property.Body;
            string propertyName = memberExpression.Member.Name;
            return GetMaxLength(context, typeof(T).Name, propertyName);
        }

    }
}
