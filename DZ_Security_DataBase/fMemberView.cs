namespace Festival_Manager
{
    public partial class cMemberView : Form
    {
        private cViewManager viewManager = new();
        private string username;
        public cMemberView(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void bCheckin_Click(object sender, EventArgs e)
        {
            Hide();
            viewManager.checkIn(sender, e, false, username);
        }
        private void bPrintReceipt_Click(object sender, EventArgs e)
        {
            viewManager.printReceipt(sender, e, username);
        }

        private void bToolBorrow_Click(object sender, EventArgs e)
        {
            Hide();
            viewManager.toolBorrow(sender, e, false, username);
        }

        private void cMemberView_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hide();
            cLoginMenu cLoginMenu = new();
            cLoginMenu.ShowDialog();
        }

        private void cMemberView_Load(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
