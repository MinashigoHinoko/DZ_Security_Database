namespace DZ_Security_DataBase
{
    public partial class cMenu : Form
    {
        private bool firstLoad = true;
        public cMenu()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void bCheckin_Click(object sender, EventArgs e)
        {
            this.Hide();
            cCheckIn checkIn = new cCheckIn();
            checkIn.ShowDialog();
        }

        private void cMenu_Load(object sender, EventArgs e)
        {
            if (firstLoad)
            {
                cDataBase.createDatabase();
                cDataBase.editDatabase();
                firstLoad = false;
            }
        }
    }
}
