namespace DZ_Security_DataBase
{
    public partial class cMemberView : Form
    {
        cViewManager viewManager = new cViewManager();
        public cMemberView()
        {
            InitializeComponent();
        }

        private void bCheckin_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.checkIn(sender, e, false);
        }
        private void bPrintReceipt_Click(object sender, EventArgs e)
        {
            viewManager.printReceipt(sender, e);
        }

        private void bToolBorrow_Click(object sender, EventArgs e)
        {
            this.Hide();
            viewManager.toolBorrow(sender, e, false);
        }

        private void cMemberView_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            cLoginMenu cLoginMenu = new cLoginMenu();
            cLoginMenu.ShowDialog();
        }
    }
}
