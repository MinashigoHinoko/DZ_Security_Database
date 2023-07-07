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

                using (var cmd = new SQLiteCommand("SELECT COUNT(DISTINCT MitarbeiterID) FROM Arbeitszeiten WHERE CheckedIn IS NOT NULL AND CheckedOut IS NULL", conn))
                {
                    long countCheckIn = (long)cmd.ExecuteScalar();
                    lbCheckedIn.Text = countCheckIn.ToString();
                }

                using (var cmd = new SQLiteCommand("SELECT COUNT(DISTINCT MitarbeiterID) FROM Arbeitszeiten WHERE CheckedOut IS NOT NULL", conn))
                {
                    long countCheckOut = (long)cmd.ExecuteScalar();
                    lbCheckedOut.Text = countCheckOut.ToString();
                }
                using (var cmd = new SQLiteCommand("Select COUNT(MitarbeiterID) FROM Mitarbeiter", conn))
                {
                    long countCheckTotal = (long)cmd.ExecuteScalar();
                    lbTotalCount.Text = countCheckTotal.ToString();
                }
                conn.Close();
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
                        }
                    }
                }
            }

            buildDatabase();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Erzeugt ein neues Formular
            Form prompt = new Form();
            prompt.Width = 200;
            prompt.Height = 150;
            prompt.Text = "Wählen Sie einen Mitarbeiter aus";

            // Erzeugt eine neue ComboBox und füllt sie mit MitarbeiterIDs
            ComboBox employeeComboBox = new ComboBox();
            employeeComboBox.Dock = DockStyle.Fill;

            // Füllen Sie die ComboBox mit den MitarbeiterIDs aus Ihrer Datenbank,
            // die noch nicht eingecheckt haben.
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT Mitarbeiter.MitarbeiterID, Mitarbeiter.Vorname || ' ' || Mitarbeiter.Nachname AS FullName \r\nFROM Mitarbeiter \r\nLEFT JOIN Arbeitszeiten \r\nON Mitarbeiter.MitarbeiterID = Arbeitszeiten.MitarbeiterID \r\nWHERE (Arbeitszeiten.CheckedIn IS NULL )AND (Arbeitszeiten.CheckedOut IS NULL)\r\n", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var employeeItem = new cWorker
                            {
                                ID = reader.GetInt32(0).ToString(), // oder GetInt64(0).ToString(), wenn MitarbeiterID ein long ist
                                Name = reader.GetString(1)
                            };
                            employeeComboBox.Items.Add(employeeItem);
                        }
                    }
                }
            }

            // Erzeugt einen neuen Button zum Einreichen der ausgewählten MitarbeiterID
            Button confirmation = new Button() { Text = "Ok", Dock = DockStyle.Fill };
            confirmation.Click += (sender, e) =>
            {
                if (employeeComboBox.SelectedItem != null)
                {
                    prompt.Close();
                }
                else
                {
                    MessageBox.Show("Bitte wählen Sie einen Mitarbeiter aus", "Kein Mitarbeiter ausgewählt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            // Fügt die ComboBox und den Bestätigungsbutton zum Formular hinzu
            prompt.Controls.Add(employeeComboBox);
            prompt.Controls.Add(confirmation);
            prompt.ShowDialog();

            // Nach dem Schließen des Dialogs ist der ausgewählte Mitarbeiter der in der ComboBox ausgewählte Mitarbeiter
            if (employeeComboBox.SelectedItem != null)
            {
                string selectedEmployeeID = employeeComboBox.SelectedItem.ToString();

                string oCurrentID = selectedEmployeeID; // now oCurrentID is the ID string

                bool bDoesEmployeeExist = false;
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Arbeitszeiten WHERE MitarbeiterID = @EmployeeId AND CheckedOut IS NULL", conn))
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
                        VALUES (@id, @jetzt, NULL)";
                            cmd.Parameters.AddWithValue("@id", oCurrentID);
                            cmd.Parameters.AddWithValue("@jetzt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Bitte trage zuerst den Aus-Zeitstempel ein", "Falsche Nutzung", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                buildDatabase();
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // Erzeugt ein neues Formular
            bool bDoesEmployeeExist = false;
            Form prompt = new Form();
            prompt.Width = 200;
            prompt.Height = 150;
            prompt.Text = "Wählen Sie einen Mitarbeiter aus";

            // Erzeugt eine neue ComboBox und füllt sie mit MitarbeiterIDs
            ComboBox employeeComboBox = new ComboBox();
            employeeComboBox.Dock = DockStyle.Fill;

            // Füllen Sie die ComboBox mit den MitarbeiterIDs aus Ihrer Datenbank,
            // die noch nicht eingecheckt haben.
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT Mitarbeiter.MitarbeiterID, Mitarbeiter.Vorname || ' ' || Mitarbeiter.Nachname AS FullName \r\nFROM Mitarbeiter \r\nLEFT JOIN Arbeitszeiten \r\nON Mitarbeiter.MitarbeiterID = Arbeitszeiten.MitarbeiterID \r\nWHERE Arbeitszeiten.CheckedOut IS NULL\r\n", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var employeeItem = new cWorker
                            {
                                ID = reader.GetInt32(0).ToString(), // oder GetInt64(0).ToString(), wenn MitarbeiterID ein long ist
                                Name = reader.GetString(1)
                            };
                            employeeComboBox.Items.Add(employeeItem);
                        }
                    }
                }
            }

            // Erzeugt einen neuen Button zum Einreichen der ausgewählten MitarbeiterID
            Button confirmation = new Button() { Text = "Ok", Dock = DockStyle.Fill };
            confirmation.Click += (sender, e) =>
            {
                if (employeeComboBox.SelectedItem != null)
                {
                    prompt.Close();
                }
                else
                {
                    MessageBox.Show("Bitte wählen Sie einen Mitarbeiter aus", "Kein Mitarbeiter ausgewählt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            // Fügt die ComboBox und den Bestätigungsbutton zum Formular hinzu
            prompt.Controls.Add(employeeComboBox);
            prompt.Controls.Add(confirmation);
            prompt.ShowDialog();

            // Nach dem Schließen des Dialogs ist der ausgewählte Mitarbeiter der in der ComboBox ausgewählte Mitarbeiter
            if (employeeComboBox.SelectedItem != null)
            {
                string selectedEmployeeID = employeeComboBox.SelectedItem.ToString();

                string oCurrentID = selectedEmployeeID; // now oCurrentID is the ID string
                if (oCurrentID == null)
                {
                    MessageBox.Show("Bitte wähle zuerst oben einen Mitarbeiter aus", "Falsche Nutzung", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Arbeitszeiten WHERE MitarbeiterID = @EmployeeId AND CheckedOut IS NULL", conn))
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
                SET CheckedOut = @ausgetragen
                WHERE MitarbeiterID = @id AND CheckedOut IS NULL";
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