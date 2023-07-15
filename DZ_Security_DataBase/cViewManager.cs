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
        static string folderPath = cDataBase.DbPath;
        static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
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
            Form prompt = new Form();
            prompt.Width = 300;
            prompt.Height = 300; // adjust the height to accommodate another TextBox
            prompt.Text = "Wählen Sie ein Ausrüstungsteil aus und geben Sie die Mitarbeiter-ID ein";

            TextBox employeeBox = new TextBox() { Dock = DockStyle.Top }; // New TextBox for employee ID
            ListBox employeeListBox = new ListBox() { Dock = DockStyle.Top };

            // Erzeugt einen neuen Button zum Einreichen der ausgewählten MitarbeiterID
            Button confirmation = new Button() { Text = "Ok", Dock = DockStyle.Bottom };
            confirmation.Width = 100; // Set the width
            confirmation.Height = 30; // Set the height
                                      // Set the AcceptButton property of the Form
            prompt.AcceptButton = confirmation;
            confirmation.Click += (sender, e) =>
            {
                if (employeeListBox.SelectedItem != null)
                {
                    prompt.Close();
                }
                else
                {
                    MessageBox.Show("Bitte wählen Sie eine Mitarbeiter-ID ein",
                                    "Erforderliche Informationen fehlen",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                }
            };

            List<cWorker> allEmployee = new List<cWorker>();
            // Füllen Sie die ComboBox mit den MitarbeiterIDs aus Ihrer Datenbank,
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT MitarbeiterID, Vorname || ' ' || Nachname AS Name FROM Mitarbeiter\r\n", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var employeeItem = new cWorker
                            {
                                ID = reader.GetInt32(0).ToString(),
                                Name = reader.GetString(1),
                            };
                            allEmployee.Add(employeeItem);
                            employeeListBox.Items.Add(employeeItem);
                        }
                    }
                }
            }
            // Füge das Event TextChanged hinzu, um die Liste zu filtern, wenn der Benutzer in die TextBox schreibt
            employeeBox.TextChanged += (sender, e) =>
            {
                // Konvertiere Suchbegriffe zu Kleinbuchstaben und teile sie auf Basis von Kommas
                string[] searchTerms = employeeBox.Text.ToLower().Split(',');

                // Nur die Einträge anzeigen, die alle Suchbegriffe enthalten
                var matches = allEmployee.Where(item =>
                    searchTerms.All(term => item.ID.Contains(term.Trim())
                                        || item.Name.ToLower().Contains(term.Trim())
                                    )
                );
                employeeListBox.Items.Clear();
                foreach (var match in matches)
                {
                    employeeListBox.Items.Add(match);
                }
            };

            prompt.Controls.Add(employeeBox);
            prompt.Controls.Add(employeeListBox);
            prompt.Controls.Add(confirmation);
            prompt.ShowDialog();


            // Nach dem Schließen des Dialogs ist das ausgewählte Ausrüstungsteil das in der ListBox ausgewählte Ausrüstungsteil
            if (employeeListBox.SelectedItem != null)
            {
                cWorker selectedWorker = employeeListBox.SelectedItem as cWorker;
                string oCurrentID = selectedWorker.ID;

                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(
                        @"SELECT m.CheckInState, m.Position, p.Quadrat, m.Vorname, m.Nachname, 
                      m.Ansprechpartner, a.Position, a.Vorname, a.Nachname
                      FROM Mitarbeiter m 
                      LEFT JOIN Mitarbeiter a ON m.Ansprechpartner = a.MitarbeiterID
                      LEFT JOIN Position p ON m.Position = p.Nr
                      WHERE m.MitarbeiterID = @MitarbeiterID", conn))
                    {
                        cmd.Parameters.AddWithValue("@MitarbeiterID", oCurrentID);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string text = reader.GetString(0) == "true" ? "Laufzettel: CheckIn\n\n" : "Laufzettel: CheckOut\n\n";
                                string positionNr = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                                string quadrant = reader.GetString(2);
                                string vorname = reader.GetString(3);
                                string nachname = reader.GetString(4);
                                string ansprechpartnerId = reader.GetString(5);
                                string ansprechpartnerPosition = reader.GetString(6);
                                string ansprechpartnerVorname = reader.GetString(7);
                                string ansprechpartnerNachname = reader.GetString(8);

                                // Fügt die abgerufenen Werte zum Text hinzu.
                                text += "Positions Nr: " + positionNr + "\n";
                                text += "Quadrant: " + quadrant + "\n";
                                text += "Vorname: " + vorname + "\n";
                                text += "Nachname: " + nachname + "\n";
                                text += "Ansprechpartner Name: " + ansprechpartnerVorname + " " + ansprechpartnerNachname + "\n";
                                text += "Ansprechpartner Positions Nr: " + ansprechpartnerPosition + "\n";

                                // Erzeugt das Font-Objekt.
                                Font printFont = new Font("Arial", 10);

                                // Zeichnet den Text.
                                e.Graphics.DrawString(text, printFont, Brushes.Black, 10, 10);
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }


    }
}
