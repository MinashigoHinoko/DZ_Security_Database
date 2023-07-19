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


            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var excelStream = File.Open(excelPath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(excelStream);

            var conf = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true,
                    ReadHeaderRow = rowReader => {
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





                string sql = @"INSERT INTO Mitarbeiter (MitarbeiterID,Vorname, Nachname, Firma, Position, CheckInSoll, CheckOutSoll, Nacht) 
   VALUES (@ID,@vorname, @nachname, @firma, @position, @checkInSoll, @checkOutSoll, @nacht)";

                using var cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", workerID);
                cmd.Parameters.AddWithValue("@vorname", vorname);
                cmd.Parameters.AddWithValue("@nachname", nachname);
                cmd.Parameters.AddWithValue("@firma", firma);
                cmd.Parameters.AddWithValue("@position", position);

                // Überprüfen ob die Werte NULL sind, bevor Sie sie in die Datenbank einfügen
                if (checkInSoll == DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@checkInSoll", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@checkInSoll", checkInSoll);

                if (checkOutSoll == DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@checkOutSoll", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@checkOutSoll", checkOutSoll);

                cmd.Parameters.AddWithValue("@nacht", isNight ? "true" : "false");

                cmd.ExecuteNonQuery();

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

            }
            MessageBox.Show("Mitarbeiter erfolgreich hinzugefügt!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            conn.Close();
        }
    }
}
