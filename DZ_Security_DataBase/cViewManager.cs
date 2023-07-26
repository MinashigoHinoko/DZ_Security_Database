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
        private string _employeeId;

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
        public void printReceipt(object sender, EventArgs e, string username, string employeeID)
        {
            PrintDocument printDoc = new();
            _employeeId = employeeID;

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
            cLogger.LogDatabaseChange($"Laufzettel Drucken für {employeeID}", username);
        }

        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Nach dem Schließen des Dialogs ist das ausgewählte Ausrüstungsteil das in der ListBox ausgewählte Ausrüstungsteil
            if (_employeeId != null)
            {
                using (SQLiteConnection conn = new(stConnectionString))
                {
                    string text = "";
                    string positionNr = "";
                    string vorname = "";
                    string nachname = "";
                    string ansprechpartnerPosition = "";
                    string ansprechpartnerVorname = "";
                    string ansprechpartnerNachname = "";
                    string checkedIn = "";
                    string checkedOut = "";
                    conn.Open();
                    using (SQLiteCommand cmd = new(
                        @"
                        SELECT 
                        CheckInState, Position, Vorname, Nachname
                        FROM Mitarbeiter 
                        WHERE MitarbeiterID = @mitarbeiterID
                        ", conn))
                    {
                        cmd.Parameters.AddWithValue("@mitarbeiterID", _employeeId);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                text = reader.GetString(0) == "true" ? "Laufzettel: CheckIn\n\n" : "Laufzettel: CheckOut\n\n";
                                positionNr = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                                vorname = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                                nachname = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                            }
                        }
                    }
                    using (SQLiteCommand cmd = new(
                         @"
                        SELECT 
                        CheckedIn, CheckedOut
                        FROM Arbeitszeiten
                        Where MitarbeiterID = @mitarbeiterID
                        ", conn))
                    {
                        cmd.Parameters.AddWithValue("@mitarbeiterID", _employeeId);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                checkedIn = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                                checkedOut = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            }
                        }
                    }
                    using (SQLiteCommand cmd = new(
                        @"
                        SELECT 
                        Vorgesetzter
                        FROM Position
                        Where Nr = @posID
                        ", conn))
                    {
                        cmd.Parameters.AddWithValue("@posID", positionNr);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.IsDBNull(0))
                                {
                                    ansprechpartnerPosition = string.Empty;
                                }
                                else
                                {
                                    object value = reader.GetValue(0);
                                    ansprechpartnerPosition = value.ToString();
                                }
                            }
                        }
                    }
                    using (SQLiteCommand cmd = new(
    @"
                        SELECT 
                        MitarbeiterID,Vorname,Nachname
                        FROM Mitarbeiter
                        Where Position = @posID
                        ", conn))
                    {
                        cmd.Parameters.AddWithValue("@posID", positionNr);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ansprechpartnerVorname = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                                ansprechpartnerNachname = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                            }
                        }
                        // Fügt die abgerufenen Werte zum Text hinzu.
                        text += "Positions Nr: " + positionNr + "\n";
                        text += "Nachname: " + nachname + "\n";
                        text += "Vorname: " + vorname + "\n";
                        if (ansprechpartnerPosition != "")
                        {
                            text += "Ansprechpartner Name: " + ansprechpartnerNachname + " " + ansprechpartnerVorname + "\n";
                            text += "Ansprechpartner Position: " + ansprechpartnerPosition + "\n";
                        }
                        if (checkedIn != "")
                        {
                            text += "Eingechecked: " + checkedIn + "\n";
                        }
                        if (checkedOut != "")
                        {
                            text += "Ausgechecked: " + checkedOut + "\n";
                        }

                        // Erzeugt das Font-Objekt.
                        Font printFont = new("Arial", 10);

                        // Zeichnet den Text.
                        e.Graphics.DrawString(text, printFont, Brushes.Black, 10, 10);
                        conn.Close();
                        MessageBox.Show("Drucken Erfolgreich!");
                    }
                }
            }



        }
    }
}
