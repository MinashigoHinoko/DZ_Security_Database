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
            viewManager.printOut(sender, e);
        }

        private void bExcelExport_Click(object sender, EventArgs e)
        {
            viewManager.excelExport(sender, e);
        }
    }
}
