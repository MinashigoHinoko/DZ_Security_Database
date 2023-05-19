using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Data.SQLite;

namespace DZ_Security_DataBase
{
    public partial class Form1 : Form
    {
        static string folderName = "datenBank";
        static string folderPath = Path.Combine(Application.StartupPath, folderName);
        static string stConnectionString = $"Data Source={folderPath}\\MeineDatenbank.sqlite;Version=3;";
        public Form1()
        {
            InitializeComponent();
            buildDatabase();
            insertDatabaseInComboBox();
        }
        private void buildDatabase()
        {
            Directory.CreateDirectory(folderPath);
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                using (var cmd = new SQLiteCommand("SELECT MitarbeiterID, strftime('%Y-%m-%d %H:%M:%S', ZeitstempelEingetragen) AS ZeitstempelEingetragen, strftime('%Y-%m-%d %H:%M:%S', ZeitstempelAusgetragen) AS ZeitstempelAusgetragen FROM Arbeitszeiten", conn))

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

                using (var cmd = new SQLiteCommand("SELECT MitarbeiterID FROM Mitarbeiter", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            this.cbMitarbeiterID.Items.Add(reader["MitarbeiterID"].ToString());
                        }
                    }
                }
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            object oCurrentID = cbMitarbeiterID.SelectedItem;
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
                            Console.WriteLine($"Name: {reader["Name"]}, Position: {reader["Position"]}");
                            lbName.Text = reader["Name"].ToString();
                            lbPosition.Text= reader["Position"].ToString();
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            object oCurrentID = cbMitarbeiterID.SelectedItem;
            if (oCurrentID == null)
            {
                MessageBox.Show("Bitte wähle zuerst oben einen Mitarbeiter über seine MitarbeiterID aus", "Falsche Nutzung", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            bool bDoesEmployeeExist = false;
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Mitarbeiter WHERE MitarbeiterID = @EmployeeId", conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", oCurrentID);

                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());

                    bDoesEmployeeExist = rowCount > 0 ? true : false;
                }
                conn.Close();
            }
            if (bDoesEmployeeExist)
            {
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"
                        INSERT INTO Arbeitszeiten (MitarbeiterID, ZeitstempelEingetragen, ZeitstempelAusgetragen)
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
                        SET ZeitstempelEingetragen = @eingetragen
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
            object oCurrentID = cbMitarbeiterID.SelectedItem;
            if(oCurrentID == null)
            {
                MessageBox.Show("Bitte wähle zuerst oben einen Mitarbeiter über seine MitarbeiterID aus", "Falsche Nutzung", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Mitarbeiter WHERE MitarbeiterID = @EmployeeId", conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", oCurrentID);

                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());

                    bDoesEmployeeExist = rowCount > 0 ? true : false;
                }
                conn.Close();
            }
            if (bDoesEmployeeExist)
            {
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"
                    UPDATE Arbeitszeiten
                    SET ZeitstempelAusgetragen = @ausgetragen
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
                MessageBox.Show("Bitte Trage zuerst den Start Zeitstempel ein","Falsche Nutzung",MessageBoxButtons.OK,MessageBoxIcon.Information);
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
                        adapter.Fill(dt);

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
                                workbook.Write(stream,false);
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
    }
}