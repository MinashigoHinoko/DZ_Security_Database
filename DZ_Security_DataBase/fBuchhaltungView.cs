namespace DZ_Security_DataBase
{
    public partial class cBookingView : Form
    {
        cViewManager viewManager = new cViewManager();
        public cBookingView()
        {
            InitializeComponent();
        }

        private void bPrint_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.printOut(sender, e, false);
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
    }
}
