using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkflowDataConvertor
{
    public partial class InfoForm : Form
    {
        public InfoForm()
        {
            InitializeComponent();
        }

        public void SetTotalCount(int totalCount)
        {
            this.progressBar1.Minimum = 0;
            this.progressBar1.Maximum = totalCount;
            this.lblRecordsCount.Text = totalCount.ToString();
        }

        public void UpdateCurrentCount(int currentCount)
        {
            this.progressBar1.Value = currentCount;
            this.Refresh();
            Application.DoEvents();
        }
    }
}
