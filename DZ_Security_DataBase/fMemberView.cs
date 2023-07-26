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

        private void bWorkerOverview_Click(object sender, EventArgs e)
        {
            Hide();
            viewManager.workerOverview(sender, e, false, username);
        }
    }
}
