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
            Form prompt = new Form();
            prompt.Width = 300;
            prompt.Height = 150;
            prompt.Text = "Wählen Sie einen Mitarbeiter aus";

            // Erzeugt eine TextBox und eine ListBox
            TextBox searchBox = new TextBox();
            searchBox.Dock = DockStyle.Top;

            ListBox employeeListBox = new ListBox();
            employeeListBox.Dock = DockStyle.Fill;

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
                    MessageBox.Show("Bitte wählen Sie einen Mitarbeiter aus", "Kein Mitarbeiter ausgewählt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            List<cWorker> allEmployee = new List<cWorker>();

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
                                ID = reader.GetInt32(0).ToString(),
                                Name = reader.GetString(1)
                            };
                            allEmployee.Add(employeeItem);
                            employeeListBox.Items.Add(employeeItem);
                        }
                    }
                }
            }

            // Füge das Event TextChanged hinzu, um die Liste zu filtern, wenn der Benutzer in die TextBox schreibt
            searchBox.TextChanged += (sender, e) =>
            {
                // Konvertiere Suchbegriffe zu Kleinbuchstaben und teile sie auf Basis von Kommas
                string[] searchTerms = searchBox.Text.ToLower().Split(',');

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

            // Fügt die TextBox, ListBox und den Bestätigungsbutton zum Formular hinzu
            // Fügt die TextBox, ListBox und den Bestätigungsbutton zum Formular hinzu
            prompt.Controls.Add(employeeListBox);
            prompt.Controls.Add(searchBox);
            prompt.Controls.Add(confirmation);
            prompt.ShowDialog();


            // Nach dem Schließen des Dialogs ist der ausgewählte Mitarbeiter der in der ListBox ausgewählte Mitarbeiter
            if (employeeListBox.SelectedItem != null)
            {
                cWorker selectedWorker = employeeListBox.SelectedItem as cWorker;

                string oCurrentID = selectedWorker.ID; // now oCurrentID is the ID string

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
            prompt.Width = 300;
            prompt.Height = 150;
            prompt.Text = "Wählen Sie einen Mitarbeiter aus";

            // Erzeugt eine TextBox und eine ListBox
            TextBox searchBox = new TextBox();
            searchBox.Dock = DockStyle.Top;

            ListBox employeeListBox = new ListBox();
            employeeListBox.Dock = DockStyle.Fill;

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
                    MessageBox.Show("Bitte wählen Sie einen Mitarbeiter aus", "Kein Mitarbeiter ausgewählt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            List<cWorker> allEmployee = new List<cWorker>();

            // Füllen Sie die ComboBox mit den MitarbeiterIDs aus Ihrer Datenbank,
            // die noch nicht eingecheckt haben.
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT Mitarbeiter.MitarbeiterID, Mitarbeiter.Vorname || ' ' || Mitarbeiter.Nachname AS FullName \r\nFROM Mitarbeiter \r\nLEFT JOIN Arbeitszeiten \r\nON Mitarbeiter.MitarbeiterID = Arbeitszeiten.MitarbeiterID \r\nWHERE Arbeitszeiten.CheckedIn IS NOT NULL AND Arbeitszeiten.CheckedOut IS NULL\r\n", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var employeeItem = new cWorker
                            {
                                ID = reader.GetInt32(0).ToString(),
                                Name = reader.GetString(1)
                            };
                            allEmployee.Add(employeeItem);
                            employeeListBox.Items.Add(employeeItem);
                        }
                    }
                }
            }



            // Füge das Event TextChanged hinzu, um die Liste zu filtern, wenn der Benutzer in die TextBox schreibt
            searchBox.TextChanged += (sender, e) =>
            {
                // Konvertiere Suchbegriffe zu Kleinbuchstaben und teile sie auf Basis von Kommas
                string[] searchTerms = searchBox.Text.ToLower().Split(',');

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


            // Fügt die TextBox, ListBox und den Bestätigungsbutton zum Formular hinzu
            // Fügt die TextBox, ListBox und den Bestätigungsbutton zum Formular hinzu
            prompt.Controls.Add(employeeListBox);
            prompt.Controls.Add(searchBox);
            prompt.Controls.Add(confirmation);
            prompt.ShowDialog();


            // Nach dem Schließen des Dialogs ist der ausgewählte Mitarbeiter der in der ListBox ausgewählte Mitarbeiter
            if (employeeListBox.SelectedItem != null)
            {
                cWorker selectedWorker = employeeListBox.SelectedItem as cWorker;

                string oCurrentID = selectedWorker.ID; // now oCurrentID is the ID string
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

        private void bCheckInAgain_Click(object sender, EventArgs e)
        {
            // Erzeugt ein neues Formular
            Form prompt = new Form();
            prompt.Width = 300;
            prompt.Height = 150;
            prompt.Text = "Wählen Sie einen Mitarbeiter aus";

            // Erzeugt eine TextBox und eine ListBox
            TextBox searchBox = new TextBox();
            searchBox.Dock = DockStyle.Top;

            ListBox employeeListBox = new ListBox();
            employeeListBox.Dock = DockStyle.Fill;

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
                    MessageBox.Show("Bitte wählen Sie einen Mitarbeiter aus", "Kein Mitarbeiter ausgewählt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            List<cWorker> allEmployee = new List<cWorker>();

            // Lade die Mitarbeiter aus der Datenbank in die ListBox
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(@"SELECT Mitarbeiter.MitarbeiterID, Mitarbeiter.Vorname || ' ' || Mitarbeiter.Nachname AS FullName
                            FROM Mitarbeiter 
                            WHERE NOT EXISTS (
                                SELECT 1 FROM Arbeitszeiten 
                                WHERE Mitarbeiter.MitarbeiterID = Arbeitszeiten.MitarbeiterID 
                                AND Arbeitszeiten.CheckedOut IS NULL)", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var employeeItem = new cWorker
                            {
                                ID = reader.GetInt32(0).ToString(),
                                Name = reader.GetString(1)
                            };
                            allEmployee.Add(employeeItem);
                            employeeListBox.Items.Add(employeeItem);
                        }
                    }
                }
            }

            // Füge das Event TextChanged hinzu, um die Liste zu filtern, wenn der Benutzer in die TextBox schreibt
            searchBox.TextChanged += (sender, e) =>
            {
                // Konvertiere Suchbegriffe zu Kleinbuchstaben und teile sie auf Basis von Kommas
                string[] searchTerms = searchBox.Text.ToLower().Split(',');

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

            // Fügt die TextBox, ListBox und den Bestätigungsbutton zum Formular hinzu
            // Fügt die TextBox, ListBox und den Bestätigungsbutton zum Formular hinzu
            prompt.Controls.Add(employeeListBox);
            prompt.Controls.Add(searchBox);
            prompt.Controls.Add(confirmation);
            prompt.ShowDialog();


            // Nach dem Schließen des Dialogs ist der ausgewählte Mitarbeiter der in der ListBox ausgewählte Mitarbeiter
            if (employeeListBox.SelectedItem != null)
            {
                cWorker selectedWorker = employeeListBox.SelectedItem as cWorker;

                string oCurrentID = selectedWorker.ID; // now oCurrentID is the ID string

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
        private void fCheckin_FormClosed(object sender, FormClosedEventArgs e)
        {
            cMemberView menu = new cMemberView();
            menu.Show();
        }

        private void cCheckIn_Load(object sender, EventArgs e)
        {
            insertDatabaseInComboBox();
        }
    }
}