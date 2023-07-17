using System.Data;
using System.Data.SQLite;

namespace DZ_Security_DataBase
{
    public partial class cEquipmentRent : Form
    {
        static string folderPath = cDataBase.DbPath;
        static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        bool isAdmin = false;
        public cEquipmentRent(bool isAdmin)
        {
            this.isAdmin = isAdmin;
            InitializeComponent();
        }

        private void buildDatabase()
        {
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT COUNT(DISTINCT ID) FROM Ausruestung WHERE Status IS 'Ausleihbar'", conn))
                {
                    long countRentable = (long)cmd.ExecuteScalar();
                    lbRentable.Text = countRentable.ToString();
                }

                using (var cmd = new SQLiteCommand("SELECT COUNT(DISTINCT ID) FROM Ausruestung WHERE Status IS 'Ausgeliehen'", conn))
                {
                    long countRented = (long)cmd.ExecuteScalar();
                    lbRented.Text = countRented.ToString();
                }
                using (var cmd = new SQLiteCommand("Select COUNT(ID) FROM Ausruestung", conn))
                {
                    long countCheckTotal = (long)cmd.ExecuteScalar();
                    lbTotalCount.Text = countCheckTotal.ToString();
                }
                conn.Close();
            }

        }

        private void return_Click(object sender, EventArgs e)
        {
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Ausruestung WHERE Status IS 'Ausgeliehen'", conn))
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
            Form prompt = new Form();
            prompt.Width = 300;
            prompt.Height = 300;
            prompt.Text = "Wählen Sie ein Ausrüstungsteil aus und geben Sie die Mitarbeiter-ID ein";

            TextBox equipmentBox = new TextBox() { Dock = DockStyle.Top };
            ListBox equipmentListBox = new ListBox() { Dock = DockStyle.Top };

            TextBox employeeBox = new TextBox() { Dock = DockStyle.Top };
            ListBox employeeListBox = new ListBox() { Dock = DockStyle.Top };

            Button confirmation = new Button() { Text = "Ok", Dock = DockStyle.Bottom };
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

            List<cEquipment> allEquipment = new List<cEquipment>();
            List<cWorker> allEmployee = new List<cWorker>();

            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT ID,Art,Farbe, Position FROM Ausruestung WHERE Status IS 'Ausgeliehen'", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var equipmentItem = new cEquipment
                            {
                                ID = reader.GetInt32(0).ToString(),
                                Name = reader.GetString(1),
                                Color = reader.GetString(2),
                                Position = reader.GetString(3)
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

                    using (var conn = new SQLiteConnection(stConnectionString))
                    {
                        conn.Open();
                        using (var cmd = new SQLiteCommand(
                            "SELECT Mitarbeiter.MitarbeiterID, Mitarbeiter.Vorname || ' ' || Mitarbeiter.Nachname AS Name, Mitarbeiter.Position " +
                            "FROM Mitarbeiter " +
                            "JOIN Ausruestung ON Mitarbeiter.MitarbeiterID = Ausruestung.MitarbeiterID " +
                            "WHERE Ausruestung.ID = @equipmentID", conn))
                        {
                            cmd.Parameters.AddWithValue("@equipmentID", selectedEquipment.ID);
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var employeeItem = new cWorker
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

                var matches = allEquipment.Where(item =>
                    searchTerms.All(term => item.ID.Contains(term.Trim())
                                        || item.Name.ToLower().Contains(term.Trim())
                                        || item.Color.ToLower().Contains(term.Trim())
                                        || item.Position.ToLower().Contains(term.Trim())
                                    )
                );
                equipmentListBox.Items.Clear();
                foreach (var match in matches)
                {
                    equipmentListBox.Items.Add(match);
                }
            };

            employeeBox.TextChanged += (sender, e) =>
            {
                string[] searchTerms = employeeBox.Text.ToLower().Split(',');

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

                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"
                        UPDATE Ausruestung
                        SET Status = @status,
                        MitarbeiterID = @mitarbeiterID
                        WHERE ID = @id AND Status IS 'Ausgeliehen'";
                        cmd.Parameters.AddWithValue("@id", oCurrentID);
                        cmd.Parameters.AddWithValue("@status", "Ausleihbar");
                        cmd.Parameters.AddWithValue("@mitarbeiterID", null);

                        cmd.ExecuteNonQuery();
                    }
                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"
                        UPDATE Mitarbeiter
                        SET Position = @position
                        WHERE MitarbeiterID = @id";
                        cmd.Parameters.AddWithValue("@id", oCurrentMemberID);
                        cmd.Parameters.AddWithValue("@position", null);

                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }
                buildDatabase();
            }

        }

        private void cEquipmentRent_Load(object sender, EventArgs e)
        {
            buildDatabase();
        }

        private void rent_Click(object sender, EventArgs e)
        {
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Ausruestung WHERE Status IS 'Ausleihbar'", conn))
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
            Form prompt = new Form();
            prompt.Width = 300;
            prompt.Height = 300; // adjust the height to accommodate another TextBox
            prompt.Text = "Wählen Sie ein Ausrüstungsteil aus und geben Sie die Mitarbeiter-ID ein";

            // Erzeugt eine TextBox und eine ListBox
            TextBox equipmentBox = new TextBox() { Dock = DockStyle.Top };

            ListBox equipmentListBox = new ListBox() { Dock = DockStyle.Top };

            TextBox employeeBox = new TextBox() { Dock = DockStyle.Top }; // New TextBox for employee ID
            ListBox employeeListBox = new ListBox() { Dock = DockStyle.Top };

            // Erzeugt einen neuen Button zum Einreichen der ausgewählten MitarbeiterID
            Button confirmation = new Button() { Text = "Ok", Dock = DockStyle.Bottom };
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

            List<cEquipment> allEquipment = new List<cEquipment>();
            List<cWorker> allEmployee = new List<cWorker>();

            // Füllen Sie die ComboBox mit den EquipmentIDs aus Ihrer Datenbank,
            // die noch nicht eingecheckt haben.
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT ID,Art,Farbe,Position \r\nFROM Ausruestung WHERE Status IS 'Ausleihbar' \r\n", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var equipmentItem = new cEquipment
                            {
                                ID = reader.GetInt32(0).ToString(),
                                Name = reader.GetString(1),
                                Color = reader.GetString(2),
                                Position = reader.GetString(3)
                            };
                            allEquipment.Add(equipmentItem);
                            equipmentListBox.Items.Add(equipmentItem);
                        }
                    }
                }
            }
            // Füllen Sie die ComboBox mit den MitarbeiterIDs aus Ihrer Datenbank,
            // die noch nicht eingecheckt haben.
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT MitarbeiterID, Vorname || ' ' || Nachname AS Name FROM Mitarbeiter\r\n WHERE CheckInState IS 'true'", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var employeeItem = new cWorker
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
                var matches = allEquipment.Where(item =>
                    searchTerms.All(term => item.ID.Contains(term.Trim())
                                        || item.Name.ToLower().Contains(term.Trim())
                                        || item.Color.ToLower().Contains(term.Trim())
                                        || item.Position.ToLower().Contains(term.Trim())
                                    )
                );
                equipmentListBox.Items.Clear();
                foreach (var match in matches)
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
                bool bDoesEmployeeExist = false;
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Ausruestung WHERE ID = @AusruestungID AND Status IS 'Ausleihbar'", conn))
                    {
                        cmd.Parameters.AddWithValue("@AusruestungID", oCurrentID);
                    }
                    conn.Close();
                }
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"
                        UPDATE Ausruestung
                        SET Status = @status,
                        MitarbeiterID = @memberID
                        WHERE ID = @id AND Status IS 'Ausleihbar'";
                        cmd.Parameters.AddWithValue("@id", oCurrentID);
                        cmd.Parameters.AddWithValue("@status", "Ausgeliehen");
                        cmd.Parameters.AddWithValue("@memberID", oCurrentMemberID);

                        cmd.ExecuteNonQuery();
                    }
                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"
                        UPDATE Mitarbeiter
                        SET Position = @position
                        WHERE MitarbeiterID = @id";
                        cmd.Parameters.AddWithValue("@id", oCurrentMemberID);
                        cmd.Parameters.AddWithValue("@position", oCurrentPosition);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                buildDatabase();
            }
        }

        private void cEquipmentRent_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            if (this.isAdmin)
            {
                cAdminView cAdminView = new cAdminView();
                cAdminView.ShowDialog();
            }
            else
            {
                cMemberView cMemberView = new cMemberView();
                cMemberView.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            cFunk cFunk = new cFunk();
            cFunk.ShowDialog();
        }
    }
}
