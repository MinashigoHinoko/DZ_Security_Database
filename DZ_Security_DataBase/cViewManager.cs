using Microsoft.VisualBasic;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing.Printing;

namespace Festival_Manager
{
    internal class cViewManager
    {
        private static string folderPath = cDataBase.DbPath;
        private static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        public void workerOverview(object sender, EventArgs e, bool isAdmin, string username)
        {
            cPersonalOverview personalOverview = new(isAdmin, username);
            personalOverview.Show();
        }
        public void toolOverview(object sender, EventArgs e, bool isAdmin, string username)
        {
            cEquipmentOverview equipment = new(isAdmin, username);
            equipment.Show();
        }
        public void printOut(object sender, EventArgs e, bool isAdmin, string username)
        {
            cPrintOutView printOut = new(isAdmin, username);
            printOut.Show();
        }
        public void excelExport(object sender, EventArgs e, string username)
        {
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                SaveFileDialog sfd = new();
                sfd.Filter = "Excel Documents (*.xlsx)|*.xlsx";
                sfd.FileName = "export.xlsx";

                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Mitarbeiter");

                // Überschriften
                IRow row = sheet.CreateRow(0);
                using (SQLiteCommand cmd = new("SELECT * FROM Mitarbeiter Where Firma = @Company", conn))
                {
                    string sChosenCompany = Interaction.InputBox("Für welche Firma benötigen Sie den Excel Export? ", "Firmen Abfrage", "", -1, -1);
                    if (sChosenCompany == "")
                    {
                        MessageBox.Show("Sie müssen eine Firma eingeben", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    cmd.Parameters.AddWithValue("@Company", sChosenCompany);
                    using (SQLiteDataAdapter adapter = new(cmd))
                    {
                        DataTable dt = new();
                        try
                        {
                            adapter.Fill(dt);

                        }
                        catch (Exception)
                        {
                            DialogResult result = MessageBox.Show("Die Datei ist nicht Speicherbar mit fehlenden Stop Zeitstempeln", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        cLogger.LogDatabaseChange($"Excel Export of: {sChosenCompany}", username);

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
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

                        }
                    }
                }
                sheet = workbook.CreateSheet("Arbeitszeiten");

                // Überschriften
                row = sheet.CreateRow(0);
                using (SQLiteCommand cmd = new("SELECT * FROM Arbeitszeiten", conn))
                {
                    using (SQLiteDataAdapter adapter = new(cmd))
                    {
                        DataTable dt = new();
                        try
                        {
                            adapter.Fill(dt);

                        }
                        catch (Exception)
                        {
                            DialogResult result = MessageBox.Show("Die Datei ist nicht Speicherbar mit fehlenden Stop Zeitstempeln", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
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


                    }
                }
                sheet = workbook.CreateSheet("Ausruestung");

                // Überschriften
                row = sheet.CreateRow(0);
                using (SQLiteCommand cmd = new("SELECT * FROM Ausruestung", conn))
                {
                    using (SQLiteDataAdapter adapter = new(cmd))
                    {
                        DataTable dt = new();
                        try
                        {
                            adapter.Fill(dt);

                        }
                        catch (Exception)
                        {
                            DialogResult result = MessageBox.Show("Die Datei ist nicht Speicherbar mit fehlenden Stop Zeitstempeln", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

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


                    }
                }
                sheet = workbook.CreateSheet("Position");

                // Überschriften
                row = sheet.CreateRow(0);
                using (SQLiteCommand cmd = new("SELECT * FROM Position", conn))
                {
                    using (SQLiteDataAdapter adapter = new(cmd))
                    {
                        DataTable dt = new();
                        try
                        {
                            adapter.Fill(dt);

                        }
                        catch (Exception)
                        {
                            DialogResult result = MessageBox.Show("Die Datei ist nicht Speicherbar mit fehlenden Stop Zeitstempeln", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

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


                    }

                }
                conn.Close();
                // Speichern
                using (FileStream stream = new(sfd.FileName, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(stream, false);
                }
            }
        }
        public void toolBorrow(object sender, EventArgs e, bool isAdmin, string username)
        {
            cEquipmentRent checkIn = new(isAdmin, username);
            checkIn.ShowDialog();
        }
        public void checkIn(object sender, EventArgs e, bool isAdmin, string username)
        {
            cCheckIn checkIn = new(isAdmin, username);
            checkIn.ShowDialog();
        }
        public void printReceipt(object sender, EventArgs e, string username)
        {
            PrintDocument printDoc = new();

            // Stellen Sie die Papiereinstellungen ein
            printDoc.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Receipt", 316, 720);

            // Legen Sie den Handler für das PrintPage-Ereignis fest.
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);

            // Zeigen Sie den Druckdialog an und starten Sie den Druckvorgang, wenn der Benutzer auf "Drucken" klickt.
            PrintDialog printDialog = new();
            printDialog.Document = printDoc;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
            cLogger.LogDatabaseChange($"Laufzettel Drucken", username);
        }

        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Form prompt = new();
            prompt.Width = 300;
            prompt.Height = 300; // adjust the height to accommodate another TextBox
            prompt.Text = "Wählen Sie ein Ausrüstungsteil aus und geben Sie die Mitarbeiter-ID ein";

            TextBox employeeBox = new() { Dock = DockStyle.Top }; // New TextBox for employee ID
            ListBox employeeListBox = new() { Dock = DockStyle.Top };

            // Erzeugt einen neuen Button zum Einreichen der ausgewählten MitarbeiterID
            Button confirmation = new() { Text = "Ok", Dock = DockStyle.Bottom };
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

            List<cWorker> allEmployee = new();
            // Füllen Sie die ComboBox mit den MitarbeiterIDs aus Ihrer Datenbank,
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new("SELECT MitarbeiterID, Vorname || ' ' || Nachname AS Name FROM Mitarbeiter\r\n", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cWorker employeeItem = new()
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
                IEnumerable<cWorker> matches = allEmployee.Where(item =>
                    searchTerms.All(term => item.ID.Contains(term.Trim())
                                        || item.Name.ToLower().Contains(term.Trim())
                                    )
                );
                employeeListBox.Items.Clear();
                foreach (cWorker? match in matches)
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

                using (SQLiteConnection conn = new(stConnectionString))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new(
                        @"SELECT m.CheckInState, m.Position, m.Vorname, m.Nachname, 
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
                                //string quadrant = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                                string vorname = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                                string nachname = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                                string ansprechpartnerId = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                                //string ansprechpartnerPosition = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                                string ansprechpartnerVorname = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                                string ansprechpartnerNachname = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);

                                // Fügt die abgerufenen Werte zum Text hinzu.
                                text += "Positions Nr: " + positionNr + "\n";
                                //text += "Quadrant: " + quadrant + "\n";
                                text += "Vorname: " + vorname + "\n";
                                text += "Nachname: " + nachname + "\n";
                                text += "Ansprechpartner Name: " + ansprechpartnerVorname + " " + ansprechpartnerNachname + "\n";
                                //text += "Ansprechpartner Positions Nr: " + ansprechpartnerPosition + "\n";

                                // Erzeugt das Font-Objekt.
                                Font printFont = new("Arial", 10);

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
