namespace DZ_Security_DataBase
{
    public partial class cAdminView : Form
    {
        cViewManager viewManager = new cViewManager();
        public cAdminView()
        {
            InitializeComponent();
        }

        private void bExcelExport_Click(object sender, EventArgs e)
        {
            viewManager.excelExport(sender, e);
        }

        private void bCheckin_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.checkIn(sender, e, true);
        }

        private void bToolBorrow_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.toolBorrow(sender, e, true);
        }

        private void bPrintReceipt_Click(object sender, EventArgs e)
        {
            viewManager.printReceipt(sender, e);
        }

        private void bPrint_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.printOut(sender, e, true);
        }

        private void bToolOverlay_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.toolOverview(sender, e, true);
        }

        private void bWorkerOverview_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.workerOverview(sender, e, true);
        }

        private void cAdminView_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void cAdminView_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            cLoginMenu cLoginMenu = new cLoginMenu();
            cLoginMenu.ShowDialog();
        }
    }
}
