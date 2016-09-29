using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace WorkflowDataConvertor
{
    public class WFDataConvertor
    {
        public static void ConcvertOldData(string connectionString)
        {
            //Console.Clear();

            //Console.WriteLine("Converting WF Data...");

            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadCommitted;
            transactionOptions.Timeout = TransactionManager.MaximumTimeout;

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                List<Fuel_WorkflowLogOld> oldFuelLogsList = null;

                InfoForm info = new InfoForm();

                using (MyDbContext context = new MyDbContext(connectionString))
                {
                    //Console.WriteLine("Number of logs to be converted is : " + context.Fuel_WorkflowLogOld.Count());
                    //Console.WriteLine("Begin of data mapping...");

                    //if (context.Fuel_WorkflowLogOld.Count() > 0)
                    //{
                    //    Console.Write("Mapped log count : ");
                    //}
                    oldFuelLogsList = context.Fuel_WorkflowLogOld.ToList();
                

                var totalCount = oldFuelLogsList.Count;

                info.SetTotalCount(totalCount);
                info.Show();

                var pageSize = 20;

                var processedCount = 0;

                    for (int pageIndex = 0; pageIndex < totalCount / pageSize; pageIndex++)
                    {
                        using (var contextToSave = new MyDbContext(connectionString))
                        {
                            foreach (var oldLog in oldFuelLogsList.GetRange(pageIndex * pageSize, pageSize))
                            {
                                var user = contextToSave.BasicInfo_UserView.Single(u => u.Id == oldLog.ActorUserId);
                                var companyId = user.CompanyId;
                                var oldStep = contextToSave.Fuel_ApproveFlowConfig.Single(a => a.Id == oldLog.CurrentWorkflowStepId);
                                var workflowId = contextToSave.Fuel_Workflow.Single(w => w.CompanyId == companyId && w.WorkflowEntity == oldStep.WorkflowEntity && w.Name == "Default").Id;

                                var newStep = contextToSave.Fuel_WorkflowStep.Single(s => s.WorkflowId == workflowId && s.State == oldStep.State && s.CurrentWorkflowStage == oldStep.CurrentWorkflowStage);

                                var newLog = new Fuel_WorkflowLog();
                                newLog.ActionDate = oldLog.ActionDate;
                                newLog.Active = oldLog.Active;
                                newLog.ActorUserId = oldLog.ActorUserId;
                                newLog.CharterId = oldLog.CharterId;
                                newLog.CurrentWorkflowStepId = newStep.Id;
                                newLog.Discriminator = oldLog.Discriminator;
                                newLog.FuelReportId = oldLog.FuelReportId;
                                newLog.InvoiceId = oldLog.InvoiceId;
                                newLog.OffhireId = oldLog.OffhireId;
                                newLog.OrderId = oldLog.OrderId;
                                newLog.ScrapId = oldLog.ScrapId;
                                newLog.WorkflowAction = oldLog.WorkflowAction;
                                newLog.WorkflowEntity = oldLog.WorkflowEntity;
                                newLog.Remark = oldLog.Remark;

                                contextToSave.Fuel_WorkflowLog.Add(newLog);
                                ++processedCount;
                                info.UpdateCurrentCount(processedCount);
                            }
                            //Console.SetCursorPosition(20, 4);
                            //Console.Write(count);
                            contextToSave.SaveChanges();
                        }
                        //Console.WriteLine("End of data mapping.");
                    }

                    info.Close();

                    //Console.WriteLine("Saving changes to Database...");

                    //Console.WriteLine("Changes saved to Database successfully.");
                }

                transactionScope.Complete();
            }
            MessageBox.Show("Data mapping completed successfully.");
        }
    }
}
