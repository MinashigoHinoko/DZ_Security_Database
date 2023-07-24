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
                }
            };

            var result = reader.AsDataSet(conf);
            DataTable table = result.Tables[0];


            // Verbindung zur SQLite-Datenbank herstellen
            using var conn = new SQLiteConnection(stConnectionString);
            conn.Open();
            foreach (DataRow row in table.Rows)
            {
                string mFirma = row[0].ToString().ToLower();
                string mName = row[1].ToString().ToLower();
                string mSurname = row[2].ToString().ToLower();
                string mBirthdayRaw = row[6].ToString().ToLower();
                string mBirthday;
                DateTime parsedDate;
                if (DateTime.TryParse(mBirthdayRaw, out parsedDate))
                {
                    // Wenn das Parsen erfolgreich war, formatieren Sie das Datum in das gewünschte Format
                    mBirthday = parsedDate.ToString("dd.MM.yyyy");
                    // Überprüfen Sie, ob die erforderlichen Felder Werte haben
                    if (string.IsNullOrEmpty(mFirma) || string.IsNullOrEmpty(mName) ||
                        string.IsNullOrEmpty(mSurname) || string.IsNullOrEmpty(mBirthday))
                    {
                        continue;  // Überspringen Sie diesen Mitarbeiter, wenn eines der erforderlichen Felder keinen Wert hat
                    }
                }
                else
                {
                    continue;
                }

                // Für andere Felder, setzen Sie sie auf NULL, wenn sie keinen Wert haben
                string mBirthname = string.IsNullOrEmpty(row[3].ToString()) ? null : row[3].ToString().ToLower();
                string mBirthplace = string.IsNullOrEmpty(row[4].ToString()) ? null : row[4].ToString().ToLower();
                string mBirthcountry = string.IsNullOrEmpty(row[5].ToString()) ? null : row[5].ToString().ToLower();
                string mCountry = string.IsNullOrEmpty(row[7].ToString()) ? null : row[7].ToString().ToLower();
                string mGender = string.IsNullOrEmpty(row[8].ToString()) ? null : row[8].ToString().ToLower();
                string mStreet = string.IsNullOrEmpty(row[9].ToString()) ? null : row[9].ToString().ToLower();
                string mStreetNumber = string.IsNullOrEmpty(row[10].ToString()) ? null : row[10].ToString().ToLower();
                string mPLZ = string.IsNullOrEmpty(row[11].ToString()) ? null : row[11].ToString().ToLower();
                string mCity = string.IsNullOrEmpty(row[12].ToString()) ? null : row[12].ToString().ToLower();
                string mBundes = string.IsNullOrEmpty(row[13].ToString()) ? null : row[13].ToString().ToLower();
                string mAusweisArt = string.IsNullOrEmpty(row[14].ToString()) ? null : row[14].ToString().ToLower();
                string mAusweisnummer = string.IsNullOrEmpty(row[15].ToString()) ? null : row[15].ToString().ToLower();
                string mBewacherregister = string.IsNullOrEmpty(row[16].ToString()) ? null : row[16].ToString().ToLower();
                string mSecurityKind = string.IsNullOrEmpty(row[17].ToString()) ? null : row[17].ToString().ToLower();
                string mMainLanguage = string.IsNullOrEmpty(row[18].ToString()) ? null : row[18].ToString().ToLower();
                string mLanguage = string.IsNullOrEmpty(row[19].ToString()) ? null : row[19].ToString().ToLower().Trim();
                string[] mLanguageArray = mLanguage == null ? new string[0] : mLanguage.Split(',');
                string mBesonderheiten = string.IsNullOrEmpty(row[20].ToString()) ? null : row[20].ToString().ToLower();


                string checkSql = @"SELECT COUNT(*) FROM Mitarbeiter WHERE Nachname = @nachname AND Vorname = @vorname AND Firma = @firma AND Geburtsdatum = @geburtsdatum";
                using (var checkCmd = new SQLiteCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@vorname", mSurname);
                    checkCmd.Parameters.AddWithValue("@nachname", mName);
                    checkCmd.Parameters.AddWithValue("@firma", mFirma);
                    checkCmd.Parameters.AddWithValue("@geburtsdatum", mBirthday);
                    var count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count == 0)
                    {
                        // Der Mitarbeiter existiert nicht, füge ihn hinzu
                        string sql = @"INSERT INTO Mitarbeiter 
                                    (Firma, Vorname, Nachname, Geburtsname, 
                                    Geburtsort, Geburtsland, Geburtsdatum, 
                                    Nationalitaet, Straße, Hausnummer, PLZ, 
                                    Wohnort, Bundesland, Ausweis_Art, Ausweis_Nr, 
                                    Bewacherregister_Nr, Security_Typ, Gender, 
                                    WeitereInformationen)
                                    VALUES 
                                    (@firma, @vorname, @nachname, @geburtsname, 
                                    @geburtsort, @geburtsland, @geburtsdatum, 
                                    @nationalitaet, @straße, @hausnummer, @plz, 
                                    @wohnort, @bundesland, @ausweis_art, @ausweis_nr, 
                                    @bewacherregister_nr, @security_typ, @gender,
                                    @weitereInformationen)";
                        using var cmd = new SQLiteCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@firma", mFirma);
                        cmd.Parameters.AddWithValue("@nachname", mName);
                        cmd.Parameters.AddWithValue("@vorname", mSurname);
                        cmd.Parameters.AddWithValue("@geburtsname", mBirthname);
                        cmd.Parameters.AddWithValue("@geburtsort", mBirthplace);
                        cmd.Parameters.AddWithValue("@geburtsland", mBirthcountry);
                        cmd.Parameters.AddWithValue("@geburtsdatum", mBirthday);
                        cmd.Parameters.AddWithValue("@nationalitaet", mCountry);
                        cmd.Parameters.AddWithValue("@straße", mStreet);
                        cmd.Parameters.AddWithValue("@hausnummer", mStreetNumber);
                        cmd.Parameters.AddWithValue("@plz", mPLZ);
                        cmd.Parameters.AddWithValue("@wohnort", mCity);
                        cmd.Parameters.AddWithValue("@bundesland", mBundes);
                        cmd.Parameters.AddWithValue("@ausweis_art", mAusweisArt);
                        cmd.Parameters.AddWithValue("@ausweis_nr", mAusweisnummer);
                        cmd.Parameters.AddWithValue("@bewacherregister_nr", mBewacherregister);
                        cmd.Parameters.AddWithValue("@security_typ", mSecurityKind);
                        cmd.Parameters.AddWithValue("@gender", mGender);
                        cmd.Parameters.AddWithValue("@weitereInformationen", mBesonderheiten);
                        cmd.ExecuteNonQuery();


                        // Erstellen Sie eine neue Abfrage, um die zuletzt eingefügte ID zu erhalten
                        string idSql = "SELECT last_insert_rowid()";
                        using var idCmd = new SQLiteCommand(idSql, conn);
                        long mitarbeiterID = (long)idCmd.ExecuteScalar();

                        // Fügen Sie die Muttersprache des Mitarbeiters hinzu
                        string langSQL = @"INSERT INTO MitarbeiterSprachen (MitarbeiterID,Sprache,Muttersprache)
                   VALUES (@employeeID,@lang,@mother)";
                        using var langCMD = new SQLiteCommand(langSQL, conn);
                        langCMD.Parameters.AddWithValue("@employeeID", mitarbeiterID);
                        langCMD.Parameters.AddWithValue("@lang", mMainLanguage);
                        langCMD.Parameters.AddWithValue("@mother", true);
                        langCMD.ExecuteNonQuery();
                        cLogger.LogDatabaseChange($"MutterSprache {mMainLanguage} Hinzugefügt zu Mitarbeiter {mitarbeiterID} in Liste", username);

                        foreach (string sprache in mLanguageArray)  // Durchlaufen Sie jede Sprache im Array
                        {
                            // Überprüfen Sie zuerst, ob die Sprache bereits existiert
                            string checkLanguageSql = @"SELECT COUNT(*) FROM MitarbeiterSprachen WHERE MitarbeiterID = @mitarbeiterID AND Sprache = @sprache";
                            using (var checkLanguageCmd = new SQLiteCommand(checkLanguageSql, conn))
                            {
                                checkLanguageCmd.Parameters.AddWithValue("@mitarbeiterID", mitarbeiterID);
                                checkLanguageCmd.Parameters.AddWithValue("@sprache", sprache.Trim());
                                var count2 = Convert.ToInt32(checkLanguageCmd.ExecuteScalar());
                                if (count2 > 0)
                                {
                                    // Die Sprache existiert bereits, fahren Sie mit der nächsten Sprache fort
                                    continue;
                                }
                            }

                            // Wenn die Sprache nicht existiert, fügen Sie sie hinzu
                            string foreachSQL = @"INSERT INTO MitarbeiterSprachen (MitarbeiterID, Sprache) VALUES (@mitarbeiterID, @sprache)";
                            using (var foreachCMD = new SQLiteCommand(foreachSQL, conn))
                            {
                                // Fügen Sie die Parameter zur SQL-Abfrage hinzu
                                foreachCMD.Parameters.AddWithValue("@mitarbeiterID", mitarbeiterID);
                                foreachCMD.Parameters.AddWithValue("@sprache", sprache.Trim());  // Verwenden Sie Trim(), um eventuelle Leerzeichen zu entfernen

                                // Führen Sie die SQL-Abfrage aus
                                foreachCMD.ExecuteNonQuery();
                                cLogger.LogDatabaseChange($"Sprache {sprache.Trim()} Hinzugefügt zu Mitarbeiter {mitarbeiterID} in Liste", username);
                            }
                        }



                        // Jetzt enthält mitarbeiterID die ID des neu eingefügten Mitarbeiters

                        cLogger.LogDatabaseChange($"Importiert Mitarbeiter {mitarbeiterID} in Liste", username);
                    }
                    else
                    {
                        cLogger.LogDatabaseChange($"Mitarbeiter {mSurname}, {mName} aus der Firma {mFirma} existiert bereits", username);
                        continue;
                    }
                }

            }
            cLogger.LogDatabaseChange($"Importiert MitarbeiterStammdaten Liste", username);
            MessageBox.Show("Mitarbeiter Stammdaten erfolgreich hinzugefügt!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                string position = row[0].ToString().ToLower();
                string pGender = row[1].ToString().ToLower();
                string pQuadrant = row[2].ToString().ToLower();
                string posBezeichnung = row[3].ToString().ToLower();
                string pZusatz = row[4].ToString().ToLower();
                string pComment = row[5].ToString().ToLower();
                string pNeccessary = row[6].ToString().ToLower();
                string psuperVisor = row[7].ToString().ToLower();
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
            string tagOderNacht = "tag";

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
                string posBezeichnung = row[8].ToString().ToLower();
                string position = row[2].ToString().ToLower();
                string pQuadrant = row[3].ToString().ToLower();
                string pColor = row[9].ToString().ToLower();
                string pZusatz = row[10].ToString().ToLower();

                if (posBezeichnung == "tag")
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
                    cLogger.LogDatabaseChange($"Importiert Mitarbeiter", username);
                    MessageBox.Show("Mitarbeiter erfolgreich hinzugefügt!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    return;
                }
                string name = row[4].ToString().ToLower().Trim();
                string[] splittedName = name.Split(',');
                string nachname = splittedName[0].Trim();
                string vorname = splittedName.Length > 1 ? splittedName[1].Trim() : "";
                string firma = row[5].ToString().ToLower();
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



                string selectSQL = @"SELECT MitarbeiterID FROM Mitarbeiter WHERE Vorname = @vorname AND Nachname = @nachname AND Firma = @firma";
                using (var selectCmd = new SQLiteCommand(selectSQL, conn))
                {
                    selectCmd.Parameters.AddWithValue("@vorname", vorname);
                    selectCmd.Parameters.AddWithValue("@nachname", nachname);
                    selectCmd.Parameters.AddWithValue("@firma", firma);
                    var employeeID = selectCmd.ExecuteScalar();

                    if (employeeID == null)
                    {
                        MessageBox.Show($"Personal: {nachname} {vorname}, aus der firma {firma} kann nicht in den Mitarbeiter Stammdaten gefunden werden");
                        continue;
                    }
                    string sql = @"INSERT INTO ArbeitszeitenSoll (MitarbeiterID,CheckedInSoll,CheckedOutSoll, Nacht, Position) 
                           VALUES (@ID,@checkin, @checkout,@nacht,@position)";
                    using var cmdPoss = new SQLiteCommand(sql, conn);

                    cmdPoss.Parameters.AddWithValue("@ID", employeeID);
                    // Überprüfen ob die Werte NULL sind, bevor Sie sie in die Datenbank einfügen
                    if (checkInSoll == DateTime.MinValue)
                        cmdPoss.Parameters.AddWithValue("@checkin", DBNull.Value);
                    else
                        cmdPoss.Parameters.AddWithValue("@checkin", checkInSoll);

                    if (checkOutSoll == DateTime.MinValue)
                        cmdPoss.Parameters.AddWithValue("@checkout", DBNull.Value);
                    else
                        cmdPoss.Parameters.AddWithValue("@checkout", checkOutSoll);

                    cmdPoss.Parameters.AddWithValue("@nacht", isNight ? "true" : "false");
                    cmdPoss.Parameters.AddWithValue("@position", position);
                    cmdPoss.ExecuteNonQuery();

                }
            }
            conn.Close();
        }
    }
}
