namespace Festival_Manager
{
    public partial class cBookingView : Form
    {
        private cViewManager viewManager = new();
        private string username;
        public cBookingView(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void bPrint_Click(object sender, EventArgs e)
        {
            Hide();
            viewManager.printOut(sender, e, false, username);
        }

        private void bExcelExport_Click(object sender, EventArgs e)
        {
            viewManager.excelExport(sender, e, username);
        }

        private void cBookingView_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hide();
            cLoginMenu cLoginMenu = new();
            cLoginMenu.ShowDialog();
        }

        private void cBookingView_Load(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
