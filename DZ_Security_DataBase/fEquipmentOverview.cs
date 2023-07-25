using System.Data;
using System.Data.SQLite;

namespace Festival_Manager
{
    public partial class cEquipmentOverview : Form
    {
        private static string folderPath = cDataBase.DbPath;
        private static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        private bool isAdmin = false;
        private string username;
        public cEquipmentOverview(bool isAdmin, string username)
        {
            this.isAdmin = isAdmin;
            InitializeComponent();
            this.username = username;
        }
        private void insertDatabaseInComboBox()
        {
            cbEquipment.Items.Clear();
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new("SELECT ID,Art,Farbe, Position FROM Ausruestung", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader["ID"].ToString();
                            string art = reader["Art"].ToString();
                            string farbe = reader["Farbe"].ToString();
                            string position = reader["Position"].ToString();

                            // Create a new cWorker object
                            cEquipment equipment = new() { ID = id, Name = art, Color = farbe, Position = position };
                            cbEquipment.Items.Add(equipment);
                        }
                    }
                }
            }
        }

        private void cEquipmentOverview_Load(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.CenterScreen;
            insertDatabaseInComboBox();
        }

        public void cbEquipment_SelectedIndexChanged(object sender, EventArgs e)
        {
            cEquipment selectedEquipment = cbEquipment.SelectedItem as cEquipment;
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new("SELECT COUNT(DISTINCT ID) FROM Ausruestung WHERE Art = @art AND Zustand IS NOT 'Defekt'", conn))
                {
                    cmd.Parameters.AddWithValue("@art", selectedEquipment.Name);
                    long countRentable = (long)cmd.ExecuteScalar();
                    lbBestand.Text = countRentable.ToString();
                }

                using (SQLiteCommand cmd = new("SELECT COUNT(DISTINCT ID) FROM Ausruestung WHERE Art = @art AND Zustand IS 'Defekt'", conn))
                {
                    cmd.Parameters.AddWithValue("@art", selectedEquipment.Name);
                    long countRentable = (long)cmd.ExecuteScalar();
                    lbDefect.Text = countRentable.ToString();
                }
                conn.Close();
                using (SQLiteCommand cmd = new("SELECT COUNT(*) FROM Ausruestung WHERE Art = @art AND Zustand IS NOT 'Defekt' AND Farbe IS 'blau'", conn))
                {
                    cmd.Parameters.AddWithValue("@art", selectedEquipment.Name);
                    long countRentable = (long)cmd.ExecuteScalar();
                    lbBlue.Text = countRentable.ToString();
                }
                conn.Close();
                using (SQLiteCommand cmd = new("SELECT COUNT(*) FROM Ausruestung WHERE Art = @art AND Zustand IS NOT 'Defekt' AND Farbe IS 'schwarz'", conn))
                {
                    cmd.Parameters.AddWithValue("@art", selectedEquipment.Name);
                    long countRentable = (long)cmd.ExecuteScalar();
                    lbBlack.Text = countRentable.ToString();
                }
                conn.Close();
                using (SQLiteCommand cmd = new("SELECT COUNT(*) FROM Ausruestung WHERE Art = @art AND Zustand IS NOT 'Defekt' AND Farbe IS 'rot'", conn))
                {
                    cmd.Parameters.AddWithValue("@art", selectedEquipment.Name);
                    long countRentable = (long)cmd.ExecuteScalar();
                    lbRed.Text = countRentable.ToString();
                }
                conn.Close();
            }
        }

        private void bAddWorker_Click(object sender, EventArgs e)
        {
            Form prompt = new();
            prompt.Width = 300;
            prompt.Height = 300; // adjust the height to accommodate another TextBox
            prompt.Text = "Wählen Sie ein Ausrüstungsteil aus";

            // Erzeugt eine TextBox und eine ListBox
            TextBox equipmentBox = new() { Dock = DockStyle.Top };

            ListBox equipmentListBox = new() { Dock = DockStyle.Top };
            // Erzeugt einen neuen Button zum Einreichen der ausgewählten MitarbeiterID
            Button confirmation = new() { Text = "Ok", Dock = DockStyle.Bottom };
            confirmation.Width = 100; // Set the width
            confirmation.Height = 30; // Set the height
                                      // Set the AcceptButton property of the Form
            prompt.AcceptButton = confirmation;
            confirmation.Click += (sender, e) =>
            {
                if (equipmentListBox.SelectedItem != null)
                {
                    prompt.Close();
                }
                else
                {
                    MessageBox.Show("Bitte wählen Sie ein Ausrüstungsteil aus",
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
                using (SQLiteCommand cmd = new("SELECT ID,Art,Farbe,Position \r\nFROM Ausruestung WHERE Zustand IS NOT 'Defekt' \r\n", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cEquipment equipmentItem = new()
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
            prompt.Controls.Add(equipmentBox);
            prompt.Controls.Add(equipmentListBox);
            prompt.Controls.Add(confirmation);
            prompt.ShowDialog();


            // Nach dem Schließen des Dialogs ist das ausgewählte Ausrüstungsteil das in der ListBox ausgewählte Ausrüstungsteil
            if (equipmentListBox.SelectedItem != null)
            {
                cEquipment selectedEquipment = equipmentListBox.SelectedItem as cEquipment;

                string oCurrentID = selectedEquipment.ID; // now oCurrentID is the ID string
                string oCurrentPosition = selectedEquipment.Position;
                cLogger.LogDatabaseChange($"Equipment Defekt Gemeldet: {oCurrentID}", username);
                using (SQLiteConnection conn = new(stConnectionString))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = new("SELECT COUNT(*) FROM Ausruestung WHERE ID = @AusruestungID AND Zustand IS NOT 'Defekt'", conn))
                    {
                        cmd.Parameters.AddWithValue("@AusruestungID", oCurrentID);
                    }
                    conn.Close();
                }
                using (SQLiteConnection conn = new(stConnectionString))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = new(conn))
                    {
                        cmd.CommandText = @"
                        UPDATE Ausruestung
                        SET Zustand = @zustand,
                        WHERE ID = @id";
                        cmd.Parameters.AddWithValue("@id", oCurrentID);
                        cmd.Parameters.AddWithValue("@zustand", "Defekt");

                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                cbEquipment_SelectedIndexChanged(sender, e);
            }
        }

        private void cEquipmentRent_FormClosed(object sender, FormClosedEventArgs e)
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
    }
}

