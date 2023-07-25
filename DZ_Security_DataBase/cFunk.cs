using System.Data;
using System.Data.SQLite;

namespace Festival_Manager
{
    public partial class cFunk : Form
    {
        private static string folderPath = cDataBase.DbPath;
        private static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        private string username;
        public cFunk(string username)
        {
            this.username = username;
            InitializeComponent();
        }
        private void buildDatabase()
        {
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new("SELECT COUNT(DISTINCT ID) FROM Funkgeraete WHERE Funkgeraet IS 'true'", conn))
                {
                    long countRadio = (long)cmd.ExecuteScalar();
                    lbRadio.Text = countRadio.ToString();
                }
                using (SQLiteCommand cmd = new("SELECT COUNT(DISTINCT ID) FROM Funkgeraete WHERE Mikimaus IS 'true'", conn))
                {
                    long countMicky = (long)cmd.ExecuteScalar();
                    lbMicky.Text = countMicky.ToString();
                }
                using (SQLiteCommand cmd = new("SELECT COUNT(DISTINCT ID) FROM Funkgeraete WHERE Rasierer IS 'true'", conn))
                {
                    long countShaver = (long)cmd.ExecuteScalar();
                    lbShaver.Text = countShaver.ToString();
                }
                using (SQLiteCommand cmd = new("SELECT COUNT(DISTINCT ID) FROM Funkgeraete WHERE Tarn_Headset IS 'true'", conn))
                {
                    long countHidden = (long)cmd.ExecuteScalar();
                    lbHidden.Text = countHidden.ToString();
                }

                conn.Close();
            }

        }

        private void cFunk_Load(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.CenterScreen;
            buildDatabase();
        }

        private void bRent_Click(object sender, EventArgs e)
        {

            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new("SELECT COUNT(*) FROM Funkgeraete WHERE Status IS 'Ausleihbar'", conn))
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        MessageBox.Show("Es gibt derzeit keine ausleihbare Ausrüstung.",
                                        "Keine ausleihbare Ausrüstung",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            Form prompt = new();
            prompt.Width = 300;
            prompt.Height = 300; // adjust the height to accommodate another TextBox
            prompt.Text = "Wählen Sie ein Ausrüstungsteil aus und geben Sie die Mitarbeiter-ID ein";

            // Erzeugt eine TextBox und eine ListBox
            TextBox equipmentBox = new() { Dock = DockStyle.Top };

            ListBox equipmentListBox = new() { Dock = DockStyle.Top };

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
                if (equipmentListBox.SelectedItem != null && employeeListBox.SelectedItem != null)
                {
                    prompt.Close();
                }
                else
                {
                    MessageBox.Show("Bitte wählen Sie ein Ausrüstungsteil aus und geben Sie eine Mitarbeiter-ID ein",
                                    "Erforderliche Informationen fehlen",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                }
            };

            List<cEquipment> allEquipment = new();
            List<cWorker> allEmployee = new();

            // Füllen Sie die ComboBox mit den EquipmentIDs aus Ihrer Datenbank,
            // die noch nicht eingecheckt haben.
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new("SELECT ID,Art,Bleibt \r\nFROM Funkgeraete WHERE Status IS 'Ausleihbar' \r\n", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cEquipment equipmentItem = new()
                            {
                                ID = reader.GetInt32(0).ToString(),
                                Name = reader.GetString(1),
                                Position = reader.GetString(2)
                            };
                            allEquipment.Add(equipmentItem);
                            equipmentListBox.Items.Add(equipmentItem);
                        }
                    }
                }
            }
            // Füllen Sie die ComboBox mit den MitarbeiterIDs aus Ihrer Datenbank,
            // die noch nicht eingecheckt haben.
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new("SELECT MitarbeiterID, Vorname || ' ' || Nachname AS Name FROM Mitarbeiter\r\n WHERE CheckInState IS 'true'", conn))
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
            equipmentBox.TextChanged += (sender, e) =>
            {
                // Konvertiere Suchbegriffe zu Kleinbuchstaben und teile sie auf Basis von Kommas
                string[] searchTerms = equipmentBox.Text.ToLower().Split(',');

                // Nur die Einträge anzeigen, die alle Suchbegriffe enthalten
                IEnumerable<cEquipment> matches = allEquipment.Where(item =>
                    searchTerms.All(term => item.ID.Contains(term.Trim())
                                        || item.Name.ToLower().Contains(term.Trim())
                                        || item.Color.ToLower().Contains(term.Trim())
                                        || item.Position.ToLower().Contains(term.Trim())
                                    )
                );
                equipmentListBox.Items.Clear();
                foreach (cEquipment? match in matches)
                {
                    equipmentListBox.Items.Add(match);
                }
            };

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
            prompt.Controls.Add(equipmentBox);
            prompt.Controls.Add(equipmentListBox);
            prompt.Controls.Add(confirmation);
            prompt.ShowDialog();


            // Nach dem Schließen des Dialogs ist das ausgewählte Ausrüstungsteil das in der ListBox ausgewählte Ausrüstungsteil
            if (equipmentListBox.SelectedItem != null)
            {
                cEquipment selectedEquipment = equipmentListBox.SelectedItem as cEquipment;

                string oCurrentID = selectedEquipment.ID; // now oCurrentID is the ID string
                cWorker selectedWorker = employeeListBox.SelectedItem as cWorker;
                string oCurrentMemberID = selectedWorker.ID;
                string oCurrentPosition = selectedEquipment.Position;
                using (SQLiteConnection conn = new(stConnectionString))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = new("SELECT COUNT(*) FROM Funkgeraete WHERE ID = @FunkID AND Status IS 'Ausleihbar'", conn))
                    {
                        cmd.Parameters.AddWithValue("@FunkID", oCurrentID);
                    }
                    conn.Close();
                }
                cLogger.LogDatabaseChange($"Ausleihen Funkgeraet: {oCurrentID}, MitarbeiterID: {oCurrentMemberID}", username);
                using (SQLiteConnection conn = new(stConnectionString))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = new(conn))
                    {
                        cmd.CommandText = @"
                        UPDATE Funkgeraete
                        SET Status = @status,
                        MitarbeiterID = @memberID
                        WHERE ID = @id AND Status IS 'Ausleihbar'";
                        cmd.Parameters.AddWithValue("@id", oCurrentID);
                        cmd.Parameters.AddWithValue("@status", "Ausgeliehen");
                        cmd.Parameters.AddWithValue("@memberID", oCurrentMemberID);

                        cmd.ExecuteNonQuery();
                    }
                }
                buildDatabase();
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void bReturn_Click(object sender, EventArgs e)
        {

            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new("SELECT COUNT(*) FROM Funkgeraete WHERE Status IS 'Ausgeliehen'", conn))
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        MessageBox.Show("Es gibt derzeit keine ausgeliehene Ausrüstung.",
                                        "Keine ausgeliehene Ausrüstung",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            Form prompt = new();
            prompt.Width = 300;
            prompt.Height = 300;
            prompt.Text = "Wählen Sie ein Ausrüstungsteil aus und geben Sie die Mitarbeiter-ID ein";

            TextBox equipmentBox = new() { Dock = DockStyle.Top };
            ListBox equipmentListBox = new() { Dock = DockStyle.Top };

            TextBox employeeBox = new() { Dock = DockStyle.Top };
            ListBox employeeListBox = new() { Dock = DockStyle.Top };

            Button confirmation = new() { Text = "Ok", Dock = DockStyle.Bottom };
            confirmation.Width = 100; // Set the width
            confirmation.Height = 30; // Set the height
            prompt.AcceptButton = confirmation;
            confirmation.Click += (sender, e) =>
            {
                if (equipmentListBox.SelectedItem != null && employeeListBox.SelectedItem != null)
                {
                    prompt.Close();
                }
                else
                {
                    MessageBox.Show("Bitte wählen Sie ein Ausrüstungsteil und einen Mitarbeiter aus",
                                    "Erforderliche Informationen fehlen",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                }
            };

            List<cEquipment> allEquipment = new();
            List<cWorker> allEmployee = new();

            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new("SELECT ID,Art,Bleibt FROM Funkgeraete WHERE Status IS 'Ausgeliehen'", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cEquipment equipmentItem = new()
                            {
                                ID = reader.GetInt32(0).ToString(),
                                Name = reader.GetString(1),
                                Position = reader.GetString(2)
                            };
                            allEquipment.Add(equipmentItem);
                            equipmentListBox.Items.Add(equipmentItem);
                        }
                    }
                }
            }

            equipmentListBox.SelectedIndexChanged += (sender, e) =>
            {
                if (equipmentListBox.SelectedItem is cEquipment selectedEquipment)
                {
                    allEmployee.Clear();
                    employeeListBox.Items.Clear();

                    using (SQLiteConnection conn = new(stConnectionString))
                    {
                        conn.Open();
                        using (SQLiteCommand cmd = new(
                            "SELECT Mitarbeiter.MitarbeiterID, Mitarbeiter.Vorname || ' ' || Mitarbeiter.Nachname AS Name, Mitarbeiter.Position " +
                            "FROM Mitarbeiter " +
                            "JOIN Funkgeraete ON Mitarbeiter.MitarbeiterID = Funkgeraete.MitarbeiterID " +
                            "WHERE Funkgeraete.ID = @funkID", conn))
                        {
                            cmd.Parameters.AddWithValue("@funkID", selectedEquipment.ID);
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    cWorker employeeItem = new()
                                    {
                                        ID = reader.GetInt32(0).ToString(),
                                        Name = reader.GetString(1),
                                        Position = reader.GetString(2),
                                    };
                                    allEmployee.Add(employeeItem);
                                    employeeListBox.Items.Add(employeeItem);
                                }
                            }
                        }
                    }
                }
            };

            equipmentBox.TextChanged += (sender, e) =>
            {
                string[] searchTerms = equipmentBox.Text.ToLower().Split(',');

                IEnumerable<cEquipment> matches = allEquipment.Where(item =>
                    searchTerms.All(term => item.ID.Contains(term.Trim())
                                        || item.Name.ToLower().Contains(term.Trim())
                                        || item.Color.ToLower().Contains(term.Trim())
                                        || item.Position.ToLower().Contains(term.Trim())
                                    )
                );
                equipmentListBox.Items.Clear();
                foreach (cEquipment? match in matches)
                {
                    equipmentListBox.Items.Add(match);
                }
            };

            employeeBox.TextChanged += (sender, e) =>
            {
                string[] searchTerms = employeeBox.Text.ToLower().Split(',');

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
            prompt.Controls.Add(equipmentBox);
            prompt.Controls.Add(equipmentListBox);
            prompt.Controls.Add(confirmation);
            prompt.ShowDialog();

            if (equipmentListBox.SelectedItem != null && employeeListBox.SelectedItem != null)
            {
                cEquipment selectedEquipment = equipmentListBox.SelectedItem as cEquipment;
                string oCurrentID = selectedEquipment.ID;
                cWorker selectedWorker = employeeListBox.SelectedItem as cWorker;
                string oCurrentMemberID = selectedWorker.ID;
                cLogger.LogDatabaseChange($"Rückgabe Funkgeraet: {oCurrentID}, MitarbeiterID: {oCurrentMemberID}", username);
                using (SQLiteConnection conn = new(stConnectionString))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = new(conn))
                    {
                        cmd.CommandText = @"
                        UPDATE Funkgeraete
                        SET Status = @status,
                        MitarbeiterID = @mitarbeiterID
                        WHERE ID = @id AND Status IS 'Ausgeliehen'";
                        cmd.Parameters.AddWithValue("@id", oCurrentID);
                        cmd.Parameters.AddWithValue("@status", "Ausleihbar");
                        cmd.Parameters.AddWithValue("@mitarbeiterID", null);

                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }
                buildDatabase();
            }
        }
    }
}
