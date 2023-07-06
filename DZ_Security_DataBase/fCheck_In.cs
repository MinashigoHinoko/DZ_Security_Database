using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Data.SQLite;

namespace DZ_Security_DataBase
{
    public partial class cCheckIn : Form
    {
        static string folderPath = cDataBase.DbPath;
        static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        Dictionary<string, string> mitarbeiterDict = new Dictionary<string, string>();
        public cCheckIn()
        {
            InitializeComponent();
        }
        private void buildDatabase()
        {
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT MitarbeiterID, strftime('%Y-%m-%d %H:%M:%S', CheckedIn) AS CheckedIn, strftime('%Y-%m-%d %H:%M:%S', CheckedOut) AS CheckedOut FROM Arbeitszeiten", conn))

                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Hier setzen wir die Datenquelle des DataGridView auf das DataTable.
                        this.dgvArbeitszeit.DataSource = dt;
                    }
                }
            }
        }
        private void insertDatabaseInComboBox()
        {
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT MitarbeiterID, Vorname || ' ' || Nachname AS Name FROM Mitarbeiter", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            string id = reader["MitarbeiterID"].ToString();

                            // Create a new cWorker object
                            cWorker mitarbeiter = new cWorker { ID = id, Name = name };

                            // Add it to the ComboBox
                            this.cbMitarbeiterID.Items.Add(mitarbeiter);
                        }
                    }
                }
            }

            buildDatabase();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When a Mitarbeiter is selected:
            cWorker selectedMitarbeiter = this.cbMitarbeiterID.SelectedItem as cWorker;

            string oCurrentID = selectedMitarbeiter.ID;
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand($"SELECT * FROM Mitarbeiter WHERE MitarbeiterID = {oCurrentID}", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Führen Sie hier den Code aus, den Sie für jeden Mitarbeiter mit der MitarbeiterID 3 ausführen möchten.
                            // Zum Beispiel:
                            Console.WriteLine($"Vorname: {reader["Vorname"]},Nachname: {reader["Nachname"]}, Position: {reader["Position"]}");
                            lbSurname.Text = reader["Vorname"].ToString();
                            lbName.Text = reader["Nachname"].ToString();
                            lbPosition.Text = reader["Position"].ToString();
                        }
                    }
                }
                // Prüfen, ob ZeitstempelEintragen einen Wert hat
                using (var cmd = new SQLiteCommand($"SELECT CheckedIn FROM Arbeitszeiten WHERE MitarbeiterID = {oCurrentID}", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() && !reader.IsDBNull(0))  // Prüft, ob die erste Spalte (ZeitstempelEintragen) einen Wert hat
                        {
                            start_Work_Timestamp.Enabled = false;
                            using (var cmd2 = new SQLiteCommand($"SELECT CheckedOut FROM Arbeitszeiten WHERE MitarbeiterID = {oCurrentID}", conn))
                            {
                                using (var reader2 = cmd2.ExecuteReader())
                                {
                                    if (reader2.Read() && !reader2.IsDBNull(0))  // Prüft, ob die erste Spalte (ZeitstempelEintragen) einen Wert hat
                                    {
                                        stop_Work_Timestamp.Enabled = false;

                                        DialogResult result = MessageBox.Show($"Möchten Sie änderungen an dem Bereits abgeschlossenen Arbeitstag von {selectedMitarbeiter.Name} machen?", "Fehler", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                                        if (result == DialogResult.No)
                                        {
                                            return;
                                        }
                                        if (result == DialogResult.Yes)
                                        {
                                            start_Work_Timestamp.Enabled = true;
                                            stop_Work_Timestamp.Enabled = true;
                                        }
                                    }
                                    else
                                    {
                                        stop_Work_Timestamp.Enabled = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            start_Work_Timestamp.Enabled = true;
                        }
                    }
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cWorker selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
            if (selectedWorker == null)
            {
                MessageBox.Show("Bitte wähle zuerst oben einen Mitarbeiter über seine MitarbeiterID aus", "Falsche Nutzung", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string oCurrentID = selectedWorker.ID; // now oCurrentID is the ID string
            stop_Work_Timestamp.Enabled = true;
            start_Work_Timestamp.Enabled = false;
            bool bDoesEmployeeExist = false;
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Arbeitszeiten WHERE MitarbeiterID = @EmployeeId", conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", oCurrentID);

                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());

                    bDoesEmployeeExist = rowCount > 0 ? true : false;
                }
                conn.Close();
            }

            if (!bDoesEmployeeExist)
            {
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"
                        INSERT INTO Arbeitszeiten (MitarbeiterID, CheckedIn, CheckedOut)
                        VALUES (@id, @jetzt, @ausgetragen)";
                        cmd.Parameters.AddWithValue("@id", oCurrentID);
                        cmd.Parameters.AddWithValue("@jetzt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@ausgetragen", "");

                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
            else
            {
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"
                        UPDATE Arbeitszeiten
                        SET CheckedIn = @eingetragen
                        WHERE MitarbeiterID = @id";
                        cmd.Parameters.AddWithValue("@eingetragen", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@id", oCurrentID);

                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
            buildDatabase();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool bDoesEmployeeExist = false;
            cWorker selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
            if (selectedWorker == null)
            {
                MessageBox.Show("Bitte wähle zuerst oben einen Mitarbeiter über seine MitarbeiterID aus", "Falsche Nutzung", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string oCurrentID = selectedWorker.ID; // now oCurrentID is the ID string
            if (oCurrentID == null)
            {
                MessageBox.Show("Bitte wähle zuerst oben einen Mitarbeiter über seine MitarbeiterID aus", "Falsche Nutzung", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Arbeitszeiten WHERE MitarbeiterID = @EmployeeId", conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", oCurrentID);

                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());

                    bDoesEmployeeExist = rowCount > 0 ? true : false;
                }
                conn.Close();
            }
            if (bDoesEmployeeExist)
            {
                start_Work_Timestamp.Enabled = true;
                stop_Work_Timestamp.Enabled = false;
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"
                    UPDATE Arbeitszeiten
                    SET CheckedOut = @ausgetragen
                    WHERE MitarbeiterID = @id";
                        cmd.Parameters.AddWithValue("@ausgetragen", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@id", oCurrentID);

                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                buildDatabase();
            }
            else
            {
                MessageBox.Show("Bitte Trage zuerst den Start Zeitstempel ein", "Falsche Nutzung", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
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

                            throw new InvalidOperationException("Die Datei ist nicht Speicherbar ohne ein Stop Zeitstempel");
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

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Dieser Knopf funktioniert noch nicht", "Demo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Dieser Knopf funktioniert noch nicht", "Demo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void fCheckin_FormClosed(object sender, FormClosedEventArgs e)
        {
            fMemberView menu = new fMemberView();
            menu.Show();
        }

        private void cCheckIn_Load(object sender, EventArgs e)
        {
            buildDatabase();
            insertDatabaseInComboBox();
        }
    }
}