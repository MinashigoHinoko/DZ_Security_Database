using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data.SQLite;
using System.Data;
using System.Drawing.Printing;

namespace DZ_Security_DataBase
{
    public partial class fMemberView : Form
    {
        private bool firstLoad = true;
        public fMemberView()
        {
            InitializeComponent();
            this.TopMost = true;
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

        private void bPrintReceipt_Click(object sender, EventArgs e)
        {
            PrintDocument printDoc = new PrintDocument();

            // Stellen Sie die Papiereinstellungen ein
            printDoc.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Receipt", 316, 720); // Diese Größe ist typisch für einen 80mm Bon-Drucker. Sie müssen es möglicherweise an Ihre spezifische Situation anpassen.
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
            printDoc.Print();
        }


        void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Erzeugt den Zeichenstring.
            string text = "Laufzettel\n\n";

            // Add some values. Ersetzen Sie "someValue1" und "someValue2" durch die tatsächlichen Werte, die Sie drucken möchten.
            //text += "Wert 1: " + someValue1.ToString() + "\n";
            //text += "Wert 2: " + someValue2.ToString() + "\n";
            text += "Wert1: \n";
            text += "Wert2: \n";
            // ... add as many values as you need

            // Erzeugt das Font-Objekt.
            Font printFont = new Font("Arial", 10);

            // Zeichnet den Text.
            e.Graphics.DrawString(text, printFont, Brushes.Black, 10, 10);
        }

    }
}
