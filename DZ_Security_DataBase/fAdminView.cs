namespace Festival_Manager
{
    public partial class cAdminView : Form
    {
        cViewManager viewManager = new cViewManager();
        string username;
        public cAdminView(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void bExcelExport_Click(object sender, EventArgs e)
        {
            viewManager.excelExport(sender, e);
        }

        private void bCheckin_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.checkIn(sender, e, true, username);
        }

        private void bToolBorrow_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.toolBorrow(sender, e, true, username);
        }

        private void bPrintReceipt_Click(object sender, EventArgs e)
        {
            viewManager.printReceipt(sender, e);
        }

        private void bPrint_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.printOut(sender, e, true, username);
        }

        private void bToolOverlay_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.toolOverview(sender, e, true, username);
        }

        private void bWorkerOverview_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.workerOverview(sender, e, true, username);
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            UserManagementForm userManagementForm = new UserManagementForm(username);
            userManagementForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            fInportData inportData = new fInportData(true, username);
            inportData.ShowDialog();
        }
    }
}
