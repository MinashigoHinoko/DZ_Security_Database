using System.Data;
using System.Data.SQLite;

namespace Festival_Manager
{
    public partial class cCheckIn : Form
    {
        private static string folderPath = cDataBase.DbPath;
        private static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        private bool isAdmin = false;
        private string username;
        public cCheckIn(bool isAdmin, string username)
        {
            InitializeComponent();
            this.isAdmin = isAdmin;
            this.username = username;
        }
        private void buildDatabase()
        {
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new("SELECT COUNT(CheckInState) FROM Mitarbeiter WHERE CheckInState IS 'true'", conn))
                {
                    long countCheckIn = (long)cmd.ExecuteScalar();
                    lbCheckedIn.Text = countCheckIn.ToString();
                }

                using (SQLiteCommand cmd = new("SELECT COUNT(CheckInState) FROM Mitarbeiter WHERE CheckInState IS 'false'", conn))
                {
                    long countCheckOut = (long)cmd.ExecuteScalar();
                    lbCheckedOut.Text = countCheckOut.ToString();
                }
                using (SQLiteCommand cmd = new("Select COUNT(MitarbeiterID) FROM Mitarbeiter", conn))
                {
                    long countCheckTotal = (long)cmd.ExecuteScalar();
                    lbTotalCount.Text = countCheckTotal.ToString();
                }
                conn.Close();
            }
            cLogger.LogDatabaseChange("Load Checkin", username);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form prompt = new();
            prompt.Width = 300;
            prompt.Height = 150;
            prompt.Text = "Wählen Sie einen Mitarbeiter aus";
            prompt.StartPosition = FormStartPosition.CenterScreen;

            // Erzeugt eine TextBox und eine ListBox
            TextBox searchBox = new();
            searchBox.Dock = DockStyle.Top;

            ListBox employeeListBox = new();
            employeeListBox.Dock = DockStyle.Fill;

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
                    MessageBox.Show("Bitte wählen Sie einen Mitarbeiter aus", "Kein Mitarbeiter ausgewählt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            List<cWorker> allEmployee = new();

            // Füllen Sie die ComboBox mit den MitarbeiterIDs aus Ihrer Datenbank,
            // die noch nicht eingecheckt haben.
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new("SELECT COUNT(*) FROM Mitarbeiter WHERE CheckInState IS 'false'", conn))
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        MessageBox.Show("Es gibt derzeit kein Personal zum einchekcen.",
                                        "Kein eincheckbares Personal",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                string today = "2023-07-24"; //DateTime.Now.ToString("yyyy-MM-dd");
                using (SQLiteCommand cmd = new(@"
                SELECT m.MitarbeiterID, m.ChipNummer, m.Nachname || ' ' || m.Vorname AS FullName, m.Position, m.CheckInState
                FROM Mitarbeiter m
                INNER JOIN ArbeitszeitenSoll a ON m.MitarbeiterID = a.MitarbeiterID
                WHERE m.CheckInState IS 'false' AND date(a.CheckedInSoll) = date(@today)", conn))
                {
                    cmd.Parameters.AddWithValue("@today", today);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cWorker employeeItem = new()
                            {
                                ID = !reader.IsDBNull(0) ? reader.GetInt32(0).ToString() : "",
                                ChipNumber = !reader.IsDBNull(1) ? reader.GetInt32(1).ToString() : "",
                                Name = reader.GetString(2),
                                Position = reader.GetString(3),
                                CheckInState = reader.GetString(4).ToLower() == "eingechecked",
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

            // Fügt die TextBox, ListBox und den Bestätigungsbutton zum Formular hinzu
            // Fügt die TextBox, ListBox und den Bestätigungsbutton zum Formular hinzu
            prompt.Controls.Add(employeeListBox);
            prompt.Controls.Add(searchBox);
            prompt.Controls.Add(confirmation);
            prompt.ShowDialog();


            // ...

            // Nach dem Schließen des Dialogs ist der ausgewählte Mitarbeiter der in der ListBox ausgewählte Mitarbeiter
            if (employeeListBox.SelectedItem != null)
            {
                cWorker selectedWorker = employeeListBox.SelectedItem as cWorker;

                string oCurrentID = selectedWorker.ID; // now oCurrentID is the ID string
                if (!selectedWorker.CheckInState)
                {
                    bool bDoesEmployeeExist = false;
                    DateTime checkInTime = DateTime.Now;
                    DateTime scheduledCheckInTime;

                    using (SQLiteConnection conn = new(stConnectionString))
                    {
                        conn.Open();

                        // Retrieve the scheduled check-in time
                        using (SQLiteCommand cmd = new("SELECT CheckedInSoll FROM ArbeitszeitenSoll WHERE MitarbeiterID = @EmployeeId", conn))
                        {
                            cmd.Parameters.AddWithValue("@EmployeeId", oCurrentID);
                            object result = cmd.ExecuteScalar();
                            scheduledCheckInTime = (result != null) ? Convert.ToDateTime(result) : checkInTime;
                        }

                        using (SQLiteCommand cmd = new("SELECT COUNT(*) FROM Arbeitszeiten WHERE MitarbeiterID = @EmployeeId AND CheckedOut IS NULL", conn))
                        {
                            cmd.Parameters.AddWithValue("@EmployeeId", oCurrentID);

                            int rowCount = Convert.ToInt32(cmd.ExecuteScalar());

                            bDoesEmployeeExist = rowCount > 0;
                        }
                        conn.Close();
                    }

                    // Use scheduled check-in time if check-in is before the scheduled time
                    if (checkInTime < scheduledCheckInTime)
                    {
                        checkInTime = scheduledCheckInTime;
                    }

                    if (!bDoesEmployeeExist)
                    {
                        cLogger.LogDatabaseChange($"CheckIn, MitarbeiterID: {oCurrentID}", username);
                        using (SQLiteConnection conn = new(stConnectionString))
                        {
                            conn.Open();

                            using (SQLiteCommand cmd = new(conn))
                            {
                                cmd.CommandText = @"
            INSERT INTO Arbeitszeiten (MitarbeiterID, CheckedIn, CheckedOut)
            VALUES (@id, @jetzt, NULL)";
                                cmd.Parameters.AddWithValue("@id", oCurrentID);
                                cmd.Parameters.AddWithValue("@jetzt", checkInTime.ToString("yyyy-MM-dd HH:mm:ss"));

                                cmd.ExecuteNonQuery();
                            }
                            using (SQLiteCommand cmd = new(conn))
                            {
                                cmd.CommandText = @"
            UPDATE Mitarbeiter
            SET CheckInState = @state
            WHERE MitarbeiterID =@id ";
                                cmd.Parameters.AddWithValue("@state", "true");
                                cmd.Parameters.AddWithValue("id", oCurrentID);

                                cmd.ExecuteNonQuery();

                            }
                            conn.Close();
                        }

                        using (SQLiteConnection conn = new(stConnectionString))
                        {
                            conn.Open();
                            using (SQLiteCommand cmd = new(conn))
                            {
                                cmd.CommandText = @"SELECT ChipNummer FROM Mitarbeiter WHERE MitarbeiterID = @EmployeeId";
                                cmd.Parameters.AddWithValue("@EmployeeId", oCurrentID);

                                object result = cmd.ExecuteScalar();

                                if (result == null || result == DBNull.Value)
                                {
                                    MessageBox.Show("Der aktuelle Mitarbeiter hat keine Chip-Nummer. Bitte fügen Sie eine Chip-Nummer hinzu.",
                                                    "Fehlende Chip-Nummer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    cPersonalOverview cPersonalOverview = new(isAdmin, username, oCurrentID);
                                    cPersonalOverview.ShowDialog();
                                }
                            }
                        }
                        cEquipmentRent cEquipmentRent = new(isAdmin, username);
                        cEquipmentRent.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Bitte Gib zuerst alle Ausgeliehene Dinge zurück", "Falsche Nutzung", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            Form prompt = new();
            prompt.Width = 300;
            prompt.Height = 150;
            prompt.Text = "Wählen Sie einen Mitarbeiter aus";
            prompt.StartPosition = FormStartPosition.CenterScreen;

            // Erzeugt eine TextBox und eine ListBox
            TextBox searchBox = new();
            searchBox.Dock = DockStyle.Top;

            ListBox employeeListBox = new();
            employeeListBox.Dock = DockStyle.Fill;

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
                    MessageBox.Show("Bitte wählen Sie einen Mitarbeiter aus", "Kein Mitarbeiter ausgewählt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            List<cWorker> allEmployee = new();

            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new("SELECT COUNT(*) FROM Mitarbeiter WHERE CheckInState IS 'true'", conn))
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        MessageBox.Show("Es gibt derzeit kein Personal zum auschecken.",
                                        "Kein auscheckbares Personal",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            // Füllen Sie die ComboBox mit den MitarbeiterIDs aus Ihrer Datenbank,
            // die eingecheckt haben.
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new("SELECT MitarbeiterID,ChipNummer, Nachname || ' ' || Vorname AS FullName, Position \r\nFROM Mitarbeiter WHERE CheckInState IS 'true' AND RentState IS 'false'", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cWorker employeeItem = new()
                            {
                                ID = !reader.IsDBNull(0) ? reader.GetInt32(0).ToString() : "",
                                ChipNumber = !reader.IsDBNull(1) ? reader.GetInt32(1).ToString() : "",
                                Name = reader.GetString(2),
                                Position = reader.GetString(3),
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

                using (SQLiteConnection conn = new(stConnectionString))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = new("SELECT COUNT(*) FROM Arbeitszeiten WHERE MitarbeiterID = @EmployeeId AND CheckedOut IS NULL", conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", oCurrentID);

                        int rowCount = Convert.ToInt32(cmd.ExecuteScalar());

                        bDoesEmployeeExist = rowCount > 0;
                    }
                    conn.Close();
                }
                if (bDoesEmployeeExist)
                {
                    cLogger.LogDatabaseChange($"CheckOut, MitarbeiterID: {oCurrentID}", username);
                    using (SQLiteConnection conn = new(stConnectionString))
                    {
                        conn.Open();

                        using (SQLiteCommand cmd = new(conn))
                        {
                            cmd.CommandText = @"
                UPDATE Arbeitszeiten
                SET CheckedOut = @ausgetragen
                WHERE MitarbeiterID = @id AND CheckedOut IS NULL";
                            cmd.Parameters.AddWithValue("@ausgetragen", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@id", oCurrentID);

                            cmd.ExecuteNonQuery();
                        }
                        using (SQLiteCommand cmd = new(conn))
                        {
                            cmd.CommandText = @"
                        UPDATE Mitarbeiter
                        SET CheckInState = @state
                        WHERE MitarbeiterID =@id ";
                            cmd.Parameters.AddWithValue("@state", "false");
                            cmd.Parameters.AddWithValue("id", oCurrentID);
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
            Hide();
            if (isAdmin)
            {
                cAdminView cAdminView = new(username);
                cAdminView.ShowDialog();
            }
            else
            {
                cMemberView cMemberView = new(username);
                cMemberView.ShowDialog();
            }
        }

        private void cCheckIn_Load(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.CenterScreen;
            buildDatabase();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            cEquipmentRent cEquipmentRent = new(isAdmin, username);
            cEquipmentRent.ShowDialog();
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}