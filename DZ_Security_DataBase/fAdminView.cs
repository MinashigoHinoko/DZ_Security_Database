namespace Festival_Manager
{
    public partial class cAdminView : Form
    {
        private cViewManager viewManager = new();
        private string username;
        public cAdminView(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void bExcelExport_Click(object sender, EventArgs e)
        {
            viewManager.excelExport(sender, e, username);
        }

        private void bCheckin_Click(object sender, EventArgs e)
        {
            Hide();
            viewManager.checkIn(sender, e, true, username);
        }

        private void bToolBorrow_Click(object sender, EventArgs e)
        {
            Hide();
            viewManager.toolBorrow(sender, e, true, username);
        }

        private void bPrintReceipt_Click(object sender, EventArgs e)
        {
            viewManager.printReceipt(sender, e, username);
        }

        private void bPrint_Click(object sender, EventArgs e)
        {
            Hide();
            viewManager.printOut(sender, e, true, username);
        }

        private void bToolOverlay_Click(object sender, EventArgs e)
        {
            Hide();
            viewManager.toolOverview(sender, e, true, username);
        }

        private void bWorkerOverview_Click(object sender, EventArgs e)
        {
            Hide();
            viewManager.workerOverview(sender, e, true, username);
        }

        private void cAdminView_Load(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void cAdminView_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hide();
            cLoginMenu cLoginMenu = new();
            cLoginMenu.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            UserManagementForm userManagementForm = new(username);
            userManagementForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            fInportData inportData = new(true, username);
            inportData.ShowDialog();
        }
    }
}
