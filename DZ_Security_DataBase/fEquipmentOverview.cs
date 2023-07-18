using System.Data;
using System.Data.SQLite;

namespace Festival_Manager
{
    public partial class cEquipmentOverview : Form
    {
        static string folderPath = cDataBase.DbPath;
        static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        bool isAdmin = false;
        string username;
        public cEquipmentOverview(bool isAdmin, string username)
        {
            this.isAdmin = isAdmin;
            InitializeComponent();
            this.username = username;
        }
        private void insertDatabaseInComboBox()
        {
            cbEquipment.Items.Clear();
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT ID,Art,Farbe, Position FROM Ausruestung", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader["ID"].ToString();
                            string art = reader["Art"].ToString();
                            string farbe = reader["Farbe"].ToString();
                            string position = reader["Position"].ToString();

                            // Create a new cWorker object
                            cEquipment equipment = new cEquipment { ID = id, Name = art, Color = farbe, Position = position };
                            cbEquipment.Items.Add(equipment);
                        }
                    }
                }
            }
        }

        private void cEquipmentOverview_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            insertDatabaseInComboBox();
        }

        public void cbEquipment_SelectedIndexChanged(object sender, EventArgs e)
        {
            cEquipment selectedEquipment = cbEquipment.SelectedItem as cEquipment;
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT COUNT(DISTINCT ID) FROM Ausruestung WHERE Art = @art AND Zustand IS NOT 'Defekt'", conn))
                {
                    cmd.Parameters.AddWithValue("@art", selectedEquipment.Name);
                    long countRentable = (long)cmd.ExecuteScalar();
                    lbBestand.Text = countRentable.ToString();
                }

                using (var cmd = new SQLiteCommand("SELECT COUNT(DISTINCT ID) FROM Ausruestung WHERE Art = @art AND Zustand IS 'Defekt'", conn))
                {
                    cmd.Parameters.AddWithValue("@art", selectedEquipment.Name);
                    long countRentable = (long)cmd.ExecuteScalar();
                    lbDefect.Text = countRentable.ToString();
                }
                conn.Close();
            }
        }

        private void bAddWorker_Click(object sender, EventArgs e)
        {
            Form prompt = new Form();
            prompt.Width = 300;
            prompt.Height = 300; // adjust the height to accommodate another TextBox
            prompt.Text = "Wählen Sie ein Ausrüstungsteil aus";

            // Erzeugt eine TextBox und eine ListBox
            TextBox equipmentBox = new TextBox() { Dock = DockStyle.Top };

            ListBox equipmentListBox = new ListBox() { Dock = DockStyle.Top };
            // Erzeugt einen neuen Button zum Einreichen der ausgewählten MitarbeiterID
            Button confirmation = new Button() { Text = "Ok", Dock = DockStyle.Bottom };
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

            List<cEquipment> allEquipment = new List<cEquipment>();
            List<cWorker> allEmployee = new List<cWorker>();

            // Füllen Sie die ComboBox mit den EquipmentIDs aus Ihrer Datenbank,
            // die noch nicht eingecheckt haben.
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT ID,Art,Farbe,Position \r\nFROM Ausruestung WHERE Zustand IS NOT 'Defekt' \r\n", conn))
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
                bool bDoesEmployeeExist = false;
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Ausruestung WHERE ID = @AusruestungID AND Zustand IS NOT 'Defekt'", conn))
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
    }
}

