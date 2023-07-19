using ExcelDataReader;
using System.Data;
using System.Data.SQLite;
using System.Globalization;

namespace Festival_Manager
{
    public partial class fInportData : Form
    {
        bool isAdmin = false;
        string username;
        static string folderPath = cDataBase.DbPath;
        static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        public fInportData(bool isAdmin, string username)
        {
            InitializeComponent();
            this.username = username;
            this.isAdmin = isAdmin;
        }

        private void fInportData_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            if (this.isAdmin)
            {
                cAdminView cAdminView = new cAdminView(username);
                cAdminView.ShowDialog();
            }
            else
            {
                cMemberView cMemberView = new cMemberView(username);
                cMemberView.ShowDialog();
            }
        }

        private void fInportData_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string excelPath = "";

            // Öffnet eine Dialogbox und lässt den Benutzer den Pfad auswählen
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                DialogResult resu = ofd.ShowDialog();
                if (resu == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.FileName))
                {
                    excelPath = ofd.FileName;
                }
            }
            string tagOderNacht = "Tag";

            if (string.IsNullOrEmpty(excelPath))
            {
                MessageBox.Show("Bitte wählen Sie eine Excel-Datei aus", "Keine Datei ausgewählt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Hier wird die Methode verlassen, wenn kein Pfad ausgewählt wurde.
            }
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var excelStream = File.Open(excelPath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(excelStream);

            var conf = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true,
                    ReadHeaderRow = rowReader =>
                    {
                        // F skip the first two rows
                        for (int i = 0; i < 2; i++)
                        {
                            rowReader.Read();
                        }
                    }
                }
            };

            var result = reader.AsDataSet(conf);
            DataTable table = result.Tables[0];


            // Verbindung zur SQLite-Datenbank herstellen
            using var conn = new SQLiteConnection(stConnectionString);
            conn.Open();
            long workerID = 0;
            bool isNight = true;
            foreach (DataRow row in table.Rows)
            {
                using (var smallCMD = new SQLiteCommand("SELECT COUNT(MitarbeiterID) FROM Mitarbeiter", conn))
                {
                    workerID = (long)smallCMD.ExecuteScalar() + 1;
                }
                string posBezeichnung = row[8].ToString();
                string position = row[2].ToString();
                string pQuadrant = row[3].ToString();
                string pColor = row[9].ToString();
                string pZusatz = row[10].ToString();

                if (posBezeichnung.ToLower() == "tag")
                {
                    isNight = false;
                    continue;
                }
                else if (posBezeichnung.ToLower() == "nacht")
                {
                    isNight = true;
                    continue;
                }

                if (row[0].ToString().ToLower() == "gesamt")
                {
                    cLogger.LogDatabaseChange($"Importierte Mitarbeiter", username);
                    MessageBox.Show("Mitarbeiter erfolgreich hinzugefügt!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    return;
                }
                string name = row[4].ToString();
                string[] splittedName = name.Split(',');
                string nachname = splittedName[0];
                string vorname = splittedName.Length > 1 ? splittedName[1] : "";
                string firma = row[5].ToString();
                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                string dateString = row[0].ToString();
                string checkInTimeString = row[12].ToString();
                string checkOutTimeString = row[13].ToString();
                bool dateParsed = DateTime.TryParseExact(dateString, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateObj);
                if (!dateParsed)
                {
                    MessageBox.Show($"Failed to parse date: {dateString}");
                    continue;
                }

                DateTime checkInSoll = DateTime.MinValue;
                if (!string.IsNullOrWhiteSpace(checkInTimeString))
                {
                    DateTime.TryParse(checkInTimeString, out DateTime checkInDateTime);
                    TimeSpan checkInTime = checkInDateTime.TimeOfDay;
                    checkInSoll = dateObj.Date + checkInTime;

                }

                DateTime checkOutSoll = DateTime.MinValue;
                if (!string.IsNullOrWhiteSpace(checkOutTimeString))
                {
                    DateTime.TryParse(checkOutTimeString, out DateTime checkOutDateTime);
                    TimeSpan checkOutTime = checkOutDateTime.TimeOfDay;
                    checkOutSoll = dateObj.Date + checkOutTime;
                }




                // Überprüfen Sie, ob die Position bereits in der Tabelle ist
                string sql = @"SELECT COUNT(*) FROM Mitarbeiter WHERE Vorname = @vorname AND Nachname = @nachname";
                using (var checkCmd = new SQLiteCommand(sql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@vorname", vorname);
                    checkCmd.Parameters.AddWithValue("@nachname", nachname);
                    var count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count == 0)
                    {
                        sql = @"INSERT INTO Mitarbeiter (MitarbeiterID,Vorname, Nachname, Firma, Position, Nacht) 
                        VALUES (@ID,@vorname, @nachname, @firma, @fNumber,@nacht)";

                        using var cmd = new SQLiteCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@ID", workerID);
                        cmd.Parameters.AddWithValue("@vorname", vorname);
                        cmd.Parameters.AddWithValue("@nachname", nachname);
                        cmd.Parameters.AddWithValue("@firma", firma);
                        cmd.Parameters.AddWithValue("@fNumber", position);

                        cmd.Parameters.AddWithValue("@nacht", isNight ? "true" : "false");

                        cmd.ExecuteNonQuery();
                    }
                }

                if (posBezeichnung.ToLower() != "tag" && posBezeichnung.ToLower() != "nacht")
                {
                    // Überprüfen Sie, ob die Position bereits in der Tabelle ist
                    string checkSql = @"SELECT COUNT(*) FROM Position WHERE Nr = @ID";
                    using (var checkCmd = new SQLiteCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@ID", position);
                        var count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count == 0)
                        {
                            // Die Position existiert nicht, füge sie hinzu
                            sql = @"INSERT INTO Position (Nr,Quadrant, Farbe, Bezeichnung, Zusatz) 
                           VALUES (@ID,@quadrant, @farbe, @bezeichnung, @zusatz)";
                            using var cmdPos = new SQLiteCommand(sql, conn);
                            cmdPos.Parameters.AddWithValue("@ID", position);
                            cmdPos.Parameters.AddWithValue("@quadrant", pQuadrant);
                            cmdPos.Parameters.AddWithValue("@farbe", pColor);
                            cmdPos.Parameters.AddWithValue("@bezeichnung", posBezeichnung);
                            cmdPos.Parameters.AddWithValue("@zusatz", pZusatz);
                            cmdPos.ExecuteNonQuery();
                        }
                    }
                }

                sql = @"INSERT INTO ArbeitszeitenSoll (MitarbeiterID,CheckedInSoll,CheckedOutSoll) 
                           VALUES (@ID,@checkin, @checkout)";
                using var cmdPoss = new SQLiteCommand(sql, conn);

                cmdPoss.Parameters.AddWithValue("@ID", workerID);
                // Überprüfen ob die Werte NULL sind, bevor Sie sie in die Datenbank einfügen
                if (checkInSoll == DateTime.MinValue)
                    cmdPoss.Parameters.AddWithValue("@checkin", DBNull.Value);
                else
                    cmdPoss.Parameters.AddWithValue("@checkin", checkInSoll);

                if (checkOutSoll == DateTime.MinValue)
                    cmdPoss.Parameters.AddWithValue("@checkout", DBNull.Value);
                else
                    cmdPoss.Parameters.AddWithValue("@checkout", checkOutSoll);

                cmdPoss.ExecuteNonQuery();

            }
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string excelPath = "";

            // Öffnet eine Dialogbox und lässt den Benutzer den Pfad auswählen
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                DialogResult resu = ofd.ShowDialog();
                if (resu == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.FileName))
                {
                    excelPath = ofd.FileName;
                }
            }
            if (string.IsNullOrEmpty(excelPath))
            {
                MessageBox.Show("Bitte wählen Sie eine Excel-Datei aus", "Keine Datei ausgewählt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Hier wird die Methode verlassen, wenn kein Pfad ausgewählt wurde.
            }
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var excelStream = File.Open(excelPath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(excelStream);

            var conf = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true,
                    ReadHeaderRow = rowReader =>
                    {
                        // F skip the first two rows
                        for (int i = 0; i < 2; i++)
                        {
                            rowReader.Read();
                        }
                    }
                }
            };

            var result = reader.AsDataSet(conf);
            DataTable table = result.Tables[0];


            // Verbindung zur SQLite-Datenbank herstellen
            using var conn = new SQLiteConnection(stConnectionString);
            conn.Open();
            bool isNight = true;
            foreach (DataRow row in table.Rows)
            {
                string position = row[0].ToString();
                string pGender = row[1].ToString();
                string pQuadrant = row[2].ToString();
                string posBezeichnung = row[3].ToString();
                string pZusatz = row[4].ToString();
                string pComment = row[5].ToString();
                string pNeccessary = row[6].ToString();
                string psuperVisor = row[7].ToString();
                // Überprüfen Sie, ob die Position bereits in der Tabelle ist
                string checkSql = @"SELECT COUNT(*) FROM Position WHERE Nr = @ID";
                using (var checkCmd = new SQLiteCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@ID", position);
                    var count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count == 0)
                    {
                        // Die Position existiert nicht, füge sie hinzu
                        string sql = @"INSERT INTO Position (Nr,Quadrant, Geschlecht, Bezeichnung, Zusatz,Bemerkung,Benötigt,Vorgesetzter) 
                           VALUES (@ID,@quadrant, @gender, @bezeichnung, @zusatz,@bemerkung,@benötigt,@vorgesetzter)";
                        using var cmdPos = new SQLiteCommand(sql, conn);
                        cmdPos.Parameters.AddWithValue("@ID", position);
                        cmdPos.Parameters.AddWithValue("@quadrant", pQuadrant);
                        cmdPos.Parameters.AddWithValue("@gender", pGender);
                        cmdPos.Parameters.AddWithValue("@bezeichnung", posBezeichnung);
                        cmdPos.Parameters.AddWithValue("@zusatz", pZusatz);
                        cmdPos.Parameters.AddWithValue("@bemerkung", pComment);
                        cmdPos.Parameters.AddWithValue("@benötigt", pNeccessary);
                        cmdPos.Parameters.AddWithValue("@vorgesetzter", psuperVisor);
                        cmdPos.ExecuteNonQuery();
                    }
                }
            }
            cLogger.LogDatabaseChange($"Importierte Positionen", username);
            MessageBox.Show("Positionen erfolgreich hinzugefügt!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            conn.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            string excelPath = "";

            // Öffnet eine Dialogbox und lässt den Benutzer den Pfad auswählen
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                DialogResult resu = ofd.ShowDialog();
                if (resu == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.FileName))
                {
                    excelPath = ofd.FileName;
                }
            }
            if (string.IsNullOrEmpty(excelPath))
            {
                MessageBox.Show("Bitte wählen Sie eine Excel-Datei aus", "Keine Datei ausgewählt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Hier wird die Methode verlassen, wenn kein Pfad ausgewählt wurde.
            }
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var excelStream = File.Open(excelPath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(excelStream);

            var conf = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true,
                    ReadHeaderRow = rowReader =>
                    {
                        // F skip the first two rows
                        for (int i = 0; i < 2; i++)
                        {
                            rowReader.Read();
                        }
                    }
                }
            };

            var result = reader.AsDataSet(conf);
            DataTable table = result.Tables[0];


            // Verbindung zur SQLite-Datenbank herstellen
            using var conn = new SQLiteConnection(stConnectionString);
            conn.Open();
            bool isNight = true;
            foreach (DataRow row in table.Rows)
            {
                string eNumber = row[0].ToString();
                string eKind = row[1].ToString();
                string eColor = row[2].ToString();
                string ePos = row[3].ToString();
                string eCondition = row[4].ToString();
                // Überprüfen Sie, ob die Position bereits in der Tabelle ist
                string checkSql = @"SELECT COUNT(*) FROM Ausruestung WHERE ID = @ID";
                using (var checkCmd = new SQLiteCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@ID", eNumber);
                    var count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count == 0)
                    {
                        // Die Position existiert nicht, füge sie hinzu
                        string sql = @"INSERT INTO Ausruestung (ID,Art, Farbe, Position, Zustand) 
                           VALUES (@ID,@art, @farbe, @position, @zustand)";
                        using var cmdPos = new SQLiteCommand(sql, conn);
                        cmdPos.Parameters.AddWithValue("@ID", eNumber);
                        cmdPos.Parameters.AddWithValue("@art", eKind);
                        cmdPos.Parameters.AddWithValue("@farbe", eColor);
                        cmdPos.Parameters.AddWithValue("@position", ePos);
                        cmdPos.Parameters.AddWithValue("@zustand", eCondition);
                        cmdPos.ExecuteNonQuery();
                    }
                }
            }
            MessageBox.Show("Ausruestung erfolgreich hinzugefügt!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            string excelPath = "";

            // Öffnet eine Dialogbox und lässt den Benutzer den Pfad auswählen
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                DialogResult resu = ofd.ShowDialog();
                if (resu == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.FileName))
                {
                    excelPath = ofd.FileName;
                }
            }
            if (string.IsNullOrEmpty(excelPath))
            {
                MessageBox.Show("Bitte wählen Sie eine Excel-Datei aus", "Keine Datei ausgewählt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Hier wird die Methode verlassen, wenn kein Pfad ausgewählt wurde.
            }
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var excelStream = File.Open(excelPath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(excelStream);

            var conf = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true,
                    ReadHeaderRow = rowReader =>
                    {
                        // F skip the first two rows
                        for (int i = 0; i < 2; i++)
                        {
                            rowReader.Read();
                        }
                    }
                }
            };

            var result = reader.AsDataSet(conf);
            DataTable table = result.Tables[0];


            // Verbindung zur SQLite-Datenbank herstellen
            using var conn = new SQLiteConnection(stConnectionString);
            conn.Open();
            bool isNight = true;
            foreach (DataRow row in table.Rows)
            {
                string fNumber = row[0].ToString();
                string fStays = row[1].ToString();
                string fBattery = row[2].ToString();
                string fFunk = row[3].ToString();
                string fTarn = row[4].ToString();
                string fRasierer = row[5].ToString();
                string fMiki = row[6].ToString();
                string fSonstiges = row[7].ToString();
                string fVerbrauchsmaterial = row[8].ToString();
                // Überprüfen Sie, ob die Position bereits in der Tabelle ist
                string checkSql = @"SELECT COUNT(*) FROM Funkgeraete WHERE ID = @ID";
                using (var checkCmd = new SQLiteCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@ID", fNumber);
                    var count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count == 0)
                    {
                        // Die Position existiert nicht, füge sie hinzu
                        string sql = @"INSERT INTO Funkgeraete (ID,Bleibt, Akku, Funkgeraet, Tarn_Headset,Rasierer,Mikimaus,Verbrauchsmaterial,Sonstiges) 
                           VALUES (@ID,@bleibt, @akku, @funk, @tarn,@rasierer,@miki,@verbrauchsmaterial,@sonstiges)";
                        using var cmdPos = new SQLiteCommand(sql, conn);
                        cmdPos.Parameters.AddWithValue("@ID", fNumber);
                        cmdPos.Parameters.AddWithValue("@bleibt", fStays);
                        cmdPos.Parameters.AddWithValue("@akku", fBattery);
                        cmdPos.Parameters.AddWithValue("@funk", fFunk);
                        cmdPos.Parameters.AddWithValue("@tarn", fTarn);
                        cmdPos.Parameters.AddWithValue("@rasierer", fRasierer);
                        cmdPos.Parameters.AddWithValue("@miki", fMiki);
                        cmdPos.Parameters.AddWithValue("@verbrauchsmaterial", fVerbrauchsmaterial);
                        cmdPos.Parameters.AddWithValue("@sonstiges", fSonstiges);
                        cmdPos.ExecuteNonQuery();
                    }
                }
            }
            cLogger.LogDatabaseChange($"Importierte Fungeraete", username);
            MessageBox.Show("Funkgeraete erfolgreich hinzugefügt!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            conn.Close();
        }
    }
}
