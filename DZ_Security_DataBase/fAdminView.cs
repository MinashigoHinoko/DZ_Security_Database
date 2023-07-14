using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DZ_Security_DataBase
{
    public partial class cAdminView : Form
    {
        cViewManager viewManager = new cViewManager();
        public cAdminView()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        private void bExcelExport_Click(object sender, EventArgs e)
        {
            viewManager.excelExport(sender, e);
        }

        private void bCheckin_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.checkIn(sender, e,true);
        }

        private void bToolBorrow_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.toolBorrow(sender, e,true);
        }

        private void bPrintReceipt_Click(object sender, EventArgs e)
        {
            viewManager.printReceipt(sender, e);
        }

        private void bPrint_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.printOut(sender, e);
        }

        private void bToolOverlay_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.toolOverview(sender, e);
        }

        private void bWorkerOverview_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.workerOverview(sender, e);
        }
    }
}
