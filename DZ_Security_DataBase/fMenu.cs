using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DZ_Security_DataBase
{
    public partial class cMenu : Form
    {
        public cMenu()
        {
            InitializeComponent();
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
            cDataBase.createDatabase();
            cDataBase.editDatabase();
        }
    }
}
