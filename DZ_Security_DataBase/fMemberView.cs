using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data.SQLite;
using System.Data;
using System.Drawing.Printing;

namespace DZ_Security_DataBase
{
    public partial class cMemberView : Form
    {
        cViewManager viewManager = new cViewManager();
        public cMemberView()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        private void bCheckin_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.checkIn(sender,e);
        }
        private void bPrintReceipt_Click(object sender, EventArgs e)
        {
            viewManager.printReceipt(sender,e);
        }

        private void bToolBorrow_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.toolBorrow(sender,e);
        }
    }
}
