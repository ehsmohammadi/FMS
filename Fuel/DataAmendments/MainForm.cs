using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Enums.Inventory;
using MITD.Fuel.Domain.Model.Extensions;

namespace DataAmendments
{
    public partial class MainForm : Form
    {
        private StorageSpaceEntities storageSpaceEntities;
        public MainForm()
        {
            InitializeComponent();

            storageSpaceEntities = new StorageSpaceEntities();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            loadDefaultData();
        }

        private void loadDefaultData()
        {
            nmudFRDetailIdCorrectionRemovalText.Maximum = storageSpaceEntities.FuelReportDetails.Max(p => p.Id);
        }

        private void btnFRDetailCorrectionRemovalProcess_Click(object sender, EventArgs e)
        {
            lstFRDetailCorrectionRemovalOutput.Items.Clear();

            using (var transactionScope = new TransactionScope())
            {
                long fuelReportDetailId = (long) nmudFRDetailIdCorrectionRemovalText.Value;

                var fuelReportDetail = storageSpaceEntities.FuelReportDetails.SingleOrDefault(frd => frd.Id == fuelReportDetailId);

                if (fuelReportDetail == null)
                {
                    MessageBox.Show("رکورد پیدا نشد");
                    return;
                }

                if (!fuelReportDetail.Correction.HasValue)
                {
                    MessageBox.Show("رکود مورد نظر مقدار اصلاحی ندارد");
                    return;
                }

                if (MessageBox.Show("آیا از حذف مقدار اصلاحی رکورد، اطمینان دارید؟", "حذف مقدار اصلاحی", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;


                var inventoryOperationReferenceType = fuelReportDetail.CorrectionType == (int) CorrectionTypes.Minus ? InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_DECREMENTAL_CORRECTION : InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_INCREMENTAL_CORRECTION;

                var inventoryOperationReferences = storageSpaceEntities.OperationReferences.Where(o => o.ReferenceType == inventoryOperationReferenceType && o.ReferenceNumber == fuelReportDetail.Id.ToString());

                foreach (var operationReference in inventoryOperationReferences)
                {
                    var transaction = storageSpaceEntities.Transactions.Single(t => t.Id == operationReference.OperationId);

                        var actionNumber = InventoryExtensions.BuildActionNumber((TransactionType) transaction.Action, transaction.WarehouseId, transaction.Code.Value, transaction.Warehouse.Code);

                    if (transaction.Status == 4)
                    {
                        //Remove vouchers
                        var vouchersToDelete = storageSpaceEntities.Vouchers.Where(v => v.ReferenceNo == actionNumber);
                        
                        vouchersToDelete.ToList().ForEach(v=> lstFRDetailCorrectionRemovalOutput.Items.Add("Removing voucher => " + v.LocalVoucherNo));

                        storageSpaceEntities.Segments.RemoveRange(vouchersToDelete.SelectMany(v => v.JournalEntries.SelectMany(j => j.Segments)));
                        storageSpaceEntities.JournalEntries.RemoveRange(vouchersToDelete.SelectMany(v => v.JournalEntries));
                        storageSpaceEntities.Vouchers.RemoveRange(vouchersToDelete);
                    }

                    var inventoryOperationsToDelete = storageSpaceEntities.InventoryOperations.Where(o => o.FuelReportDetailId == fuelReportDetail.Id && o.InventoryOperationId == transaction.Id);

                    lstFRDetailCorrectionRemovalOutput.Items.Add("Remove references in Inventory domain");
                    storageSpaceEntities.InventoryOperations.RemoveRange(inventoryOperationsToDelete);

                    lstFRDetailCorrectionRemovalOutput.Items.Add("Removing transaction => " + actionNumber);

                    storageSpaceEntities.TransactionItemPrices.RemoveRange(transaction.TransactionItems.SelectMany(ti => ti.TransactionItemPrices));
                    storageSpaceEntities.TransactionItems.RemoveRange(transaction.TransactionItems);
                    storageSpaceEntities.Transactions.Remove(transaction);
                }


                lstFRDetailCorrectionRemovalOutput.Items.Add( "Remove references in Fuel domain");
                storageSpaceEntities.OperationReferences.RemoveRange(inventoryOperationReferences);

                lstFRDetailCorrectionRemovalOutput.Items.Add( "Remove correction values");

                fuelReportDetail.Correction = null;

                fuelReportDetail.CorrectionType = null;

                fuelReportDetail.CorrectionPrice = null;

                fuelReportDetail.CorrectionPriceCurrencyISOCode = null;

                fuelReportDetail.CorrectionPriceCurrencyId = null;

                fuelReportDetail.CorrectionPricingType = null;

                fuelReportDetail.CorrectionReference_Code = null;
                fuelReportDetail.CorrectionReference_ReferenceId = null;
                fuelReportDetail.CorrectionReference_ReferenceType = null;

                lstFRDetailCorrectionRemovalOutput.Items.Add("Revert status of fuel report to inital");
                fuelReportDetail.FuelReport.State = 0;

                var nonInitialLogs = fuelReportDetail.FuelReport.WorkflowLogs.Where(l=>l.WorkflowStep.CurrentWorkflowStage != (int)WorkflowStages.Initial);

                nonInitialLogs.ToList().ForEach(l=> fuelReportDetail.FuelReport.WorkflowLogs.Remove(l));

                storageSpaceEntities.WorkflowLogs.RemoveRange(nonInitialLogs);

                storageSpaceEntities.SaveChanges();

                fuelReportDetail.FuelReport.WorkflowLogs.First().Active = true;

                storageSpaceEntities.SaveChanges();

                transactionScope.Complete();
                
                MessageBox.Show("هم اکنون می بایست مقادیر موجودی و مصرف رکورد مورد نظر در سیستم سوخت مورد بازنگری قرار گیرد.");
            }
        }
    }
}

/* method template
 long fuelReportDetailId = (long) nmudFRDetailIdCorrectionRemovalText.Value;

            var fuelReportDatail = storageSpaceEntities.FuelReportDetails.SingleOrDefault(frd => frd.Id == fuelReportDetailId);

            if (fuelReportDatail == null)
            {
                MessageBox.Show("رکورد پیدا نشد");

                return;
            }

            if(MessageBox.Show("آیا از حذف مقدار اصلاحی رکورد، اطمینان دارید؟", "Correction Removal", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            

            MessageBox.Show("هم اکنون می بایست مقادیر موجودی و مصرف رکورد مورد نظر در سیستم سوخت مورد بازنگری قرار گیرد");
 */