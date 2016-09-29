namespace DataAmendments
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.FuelReportDetailCorrectionRemovalTabPage = new System.Windows.Forms.TabPage();
            this.nmudFRDetailIdCorrectionRemovalText = new System.Windows.Forms.NumericUpDown();
            this.btnFRDetailCorrectionRemovalProcess = new System.Windows.Forms.Button();
            this.lblFuelReportDetailIdCorrectionRemovalLabelTitle = new System.Windows.Forms.Label();
            this.lstFRDetailCorrectionRemovalOutput = new System.Windows.Forms.ListBox();
            this.mainTabControl.SuspendLayout();
            this.FuelReportDetailCorrectionRemovalTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmudFRDetailIdCorrectionRemovalText)).BeginInit();
            this.SuspendLayout();
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.FuelReportDetailCorrectionRemovalTabPage);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(514, 398);
            this.mainTabControl.TabIndex = 0;
            // 
            // FuelReportDetailCorrectionRemovalTabPage
            // 
            this.FuelReportDetailCorrectionRemovalTabPage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.FuelReportDetailCorrectionRemovalTabPage.Controls.Add(this.lstFRDetailCorrectionRemovalOutput);
            this.FuelReportDetailCorrectionRemovalTabPage.Controls.Add(this.nmudFRDetailIdCorrectionRemovalText);
            this.FuelReportDetailCorrectionRemovalTabPage.Controls.Add(this.btnFRDetailCorrectionRemovalProcess);
            this.FuelReportDetailCorrectionRemovalTabPage.Controls.Add(this.lblFuelReportDetailIdCorrectionRemovalLabelTitle);
            this.FuelReportDetailCorrectionRemovalTabPage.Location = new System.Drawing.Point(4, 22);
            this.FuelReportDetailCorrectionRemovalTabPage.Name = "FuelReportDetailCorrectionRemovalTabPage";
            this.FuelReportDetailCorrectionRemovalTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.FuelReportDetailCorrectionRemovalTabPage.Size = new System.Drawing.Size(506, 372);
            this.FuelReportDetailCorrectionRemovalTabPage.TabIndex = 0;
            this.FuelReportDetailCorrectionRemovalTabPage.Text = "FR Detail Correction Removal";
            this.FuelReportDetailCorrectionRemovalTabPage.UseVisualStyleBackColor = true;
            // 
            // nmudFRDetailIdCorrectionRemovalText
            // 
            this.nmudFRDetailIdCorrectionRemovalText.Location = new System.Drawing.Point(75, 10);
            this.nmudFRDetailIdCorrectionRemovalText.Name = "nmudFRDetailIdCorrectionRemovalText";
            this.nmudFRDetailIdCorrectionRemovalText.Size = new System.Drawing.Size(120, 20);
            this.nmudFRDetailIdCorrectionRemovalText.TabIndex = 4;
            // 
            // btnFRDetailCorrectionRemovalProcess
            // 
            this.btnFRDetailCorrectionRemovalProcess.Location = new System.Drawing.Point(201, 9);
            this.btnFRDetailCorrectionRemovalProcess.Name = "btnFRDetailCorrectionRemovalProcess";
            this.btnFRDetailCorrectionRemovalProcess.Size = new System.Drawing.Size(126, 23);
            this.btnFRDetailCorrectionRemovalProcess.TabIndex = 3;
            this.btnFRDetailCorrectionRemovalProcess.Text = "Remove Correction";
            this.btnFRDetailCorrectionRemovalProcess.UseVisualStyleBackColor = true;
            this.btnFRDetailCorrectionRemovalProcess.Click += new System.EventHandler(this.btnFRDetailCorrectionRemovalProcess_Click);
            // 
            // lblFuelReportDetailIdCorrectionRemovalLabelTitle
            // 
            this.lblFuelReportDetailIdCorrectionRemovalLabelTitle.AutoSize = true;
            this.lblFuelReportDetailIdCorrectionRemovalLabelTitle.Location = new System.Drawing.Point(6, 12);
            this.lblFuelReportDetailIdCorrectionRemovalLabelTitle.Name = "lblFuelReportDetailIdCorrectionRemovalLabelTitle";
            this.lblFuelReportDetailIdCorrectionRemovalLabelTitle.Size = new System.Drawing.Size(63, 13);
            this.lblFuelReportDetailIdCorrectionRemovalLabelTitle.TabIndex = 1;
            this.lblFuelReportDetailIdCorrectionRemovalLabelTitle.Text = "FR Detail Id";
            // 
            // lstFRDetailCorrectionRemovalOutput
            // 
            this.lstFRDetailCorrectionRemovalOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFRDetailCorrectionRemovalOutput.FormattingEnabled = true;
            this.lstFRDetailCorrectionRemovalOutput.Location = new System.Drawing.Point(9, 36);
            this.lstFRDetailCorrectionRemovalOutput.Name = "lstFRDetailCorrectionRemovalOutput";
            this.lstFRDetailCorrectionRemovalOutput.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstFRDetailCorrectionRemovalOutput.Size = new System.Drawing.Size(487, 316);
            this.lstFRDetailCorrectionRemovalOutput.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 398);
            this.Controls.Add(this.mainTabControl);
            this.Name = "MainForm";
            this.Text = "FMS Amendments";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainTabControl.ResumeLayout(false);
            this.FuelReportDetailCorrectionRemovalTabPage.ResumeLayout(false);
            this.FuelReportDetailCorrectionRemovalTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmudFRDetailIdCorrectionRemovalText)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage FuelReportDetailCorrectionRemovalTabPage;
        private System.Windows.Forms.NumericUpDown nmudFRDetailIdCorrectionRemovalText;
        private System.Windows.Forms.Button btnFRDetailCorrectionRemovalProcess;
        private System.Windows.Forms.Label lblFuelReportDetailIdCorrectionRemovalLabelTitle;
        private System.Windows.Forms.ListBox lstFRDetailCorrectionRemovalOutput;
    }
}

