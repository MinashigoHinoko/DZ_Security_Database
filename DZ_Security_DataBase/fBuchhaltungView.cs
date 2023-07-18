namespace Festival_Manager
{
    public partial class cBookingView : Form
    {
        cViewManager viewManager = new cViewManager();
        string username;
        public cBookingView(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void bPrint_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.printOut(sender, e, false, username);
        }

        private void bExcelExport_Click(object sender, EventArgs e)
        {
            viewManager.excelExport(sender, e);
        }

        private void cBookingView_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            cLoginMenu cLoginMenu = new cLoginMenu();
            cLoginMenu.ShowDialog();
        }

        private void cBookingView_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
