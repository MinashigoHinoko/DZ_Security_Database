namespace DZ_Security_DataBase
{
    public partial class fInportData : Form
    {
        bool isAdmin = false;
        public fInportData(bool isAdmin)
        {
            InitializeComponent();
            this.isAdmin = isAdmin;
        }

        private void fInportData_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            if (this.isAdmin)
            {
                cAdminView cAdminView = new cAdminView();
                cAdminView.ShowDialog();
            }
            else
            {
                cMemberView cMemberView = new cMemberView();
                cMemberView.ShowDialog();
            }
        }
    }
}
