using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ_Security_DataBase
{
    internal class cViewManager
    {
        public void workerOverview(object sender, EventArgs e)
        {
            cPersonalOverview personalOverview = new cPersonalOverview();
            personalOverview.Show();
        }
        public void toolOverview(object sender, EventArgs e)
        {
            cEquipmentOverview equipment = new cEquipmentOverview();
            equipment.Show();
        }
        public void printOut(object sender, EventArgs e)
        {
            cPrintOutView printOut = new cPrintOutView();
            printOut.Show();
        }
        public void excelExport(object sender, EventArgs e)
        {

            string folderPath = cDataBase.DbPath;
            string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT * FROM Arbeitszeiten", conn))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        try
                        {
                            adapter.Fill(dt);

                        }
                        catch (Exception)
                        {
                            DialogResult result = MessageBox.Show("Die Datei ist nicht Speicherbar mit fehlenden Stop Zeitstempeln", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Excel Documents (*.xlsx)|*.xlsx";
                        sfd.FileName = "export.xlsx";

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            IWorkbook workbook = new XSSFWorkbook();
                            ISheet sheet = workbook.CreateSheet("Arbeitszeiten");

                            // Überschriften
                            IRow row = sheet.CreateRow(0);
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                            }

                            // Daten
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                row = sheet.CreateRow(i + 1);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    row.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                                }
                            }

                            // Speichern
                            using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write))
                            {
                                workbook.Write(stream, false);
                            }

                        }
                    }
                }
                conn.Close();
            }
        }
        public void toolBorrow(object sender, EventArgs e,bool isAdmin)
        {
            cEquipmentRent checkIn = new cEquipmentRent(isAdmin);
            checkIn.ShowDialog();
        }
        public void checkIn(object sender, EventArgs e, bool isAdmin)
        {
            cCheckIn checkIn = new cCheckIn(isAdmin);
            checkIn.ShowDialog();
        }
        public void printReceipt(object sender, EventArgs e)
        {
            PrintDocument printDoc = new PrintDocument();

            // Stellen Sie die Papiereinstellungen ein
            printDoc.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Receipt", 316, 720);

            // Legen Sie den Handler für das PrintPage-Ereignis fest.
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);

            // Zeigen Sie den Druckdialog an und starten Sie den Druckvorgang, wenn der Benutzer auf "Drucken" klickt.
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDoc;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
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
