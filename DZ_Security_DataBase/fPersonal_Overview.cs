using System.Data.SQLite;

namespace DZ_Security_DataBase
{
    public partial class cPersonalOverview : Form
    {
        static string folderPath = cDataBase.DbPath;
        static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        bool isAdmin = false;
        cWorker selectedWorker;
        string selectedCompany;
        int currentIndex;
        public cPersonalOverview(bool isAdmin)
        {
            InitializeComponent();
            this.isAdmin = isAdmin;
        }

        private void insertDatabaseInComboBox()
        {
            cbMitarbeiterID.Items.Clear();
            cbCompany.Items.Clear();
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT DISTINCT MitarbeiterID, Vorname || ' ' || Nachname AS Name,Position FROM Mitarbeiter", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            string id = reader["MitarbeiterID"].ToString();
                            string position = reader["Position"].ToString();

                            // Create a new cWorker object
                            cWorker mitarbeiter = new cWorker { ID = id, Name = name, Position = position };
                            cbMitarbeiterID.Items.Add(mitarbeiter);
                        }
                    }
                }
            }

            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT DISTINCT Firma FROM Mitarbeiter", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cbCompany.Items.Add(reader["Firma"].ToString());
                        }
                    }
                }
            }

        }
        private void InsertEmployeesBasedOnCompanyIntoComboBox()
        {
            selectedCompany = cbCompany.SelectedItem.ToString();

            // Clear the items in the cbMitarbeiterID ComboBox
            cbMitarbeiterID.Items.Clear();

            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT MitarbeiterID, Vorname || ' ' || Nachname AS Name, Position FROM Mitarbeiter WHERE Firma = @Company", conn))
                {
                    cmd.Parameters.AddWithValue("@Company", selectedCompany);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            string id = reader["MitarbeiterID"].ToString();
                            string position = reader["Position"].ToString();

                            // Create a new cWorker object
                            cWorker mitarbeiter = new cWorker { ID = id, Name = name, Position = position };
                            cbMitarbeiterID.Items.Add(mitarbeiter);
                        }
                    }
                }
            }
        }

        private void cbMitarbeiterID_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillData();
        }
        private void cPersonalOverview_Load(object sender, EventArgs e)
        {
            insertDatabaseInComboBox();
        }

        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            InsertEmployeesBasedOnCompanyIntoComboBox();
        }

        private void bAddWorker_Click(object sender, EventArgs e)
        {
            cPersonalManuellHinzufügen cPersonalManuellHinzufügen = new cPersonalManuellHinzufügen();
            cPersonalManuellHinzufügen.ShowDialog();
            insertDatabaseInComboBox();
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
            try
            {
                this.selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
                string oCurrentID = selectedWorker.ID; // now oCurrentID is the ID 
                this.currentIndex = cbMitarbeiterID.SelectedIndex;

                // Ask the user to confirm the update
                var confirmResult = MessageBox.Show("Möchten Sie die Änderungen speichern?",
                                                     "Bestätigen Sie das Speichern!",
                                                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    UpdateEmployeeData(oCurrentID);
                    insertDatabaseInComboBox();
                    cbMitarbeiterID.SelectedIndex = currentIndex;
                    FillData();
                }
            }
            catch
            {
                // Reset the ComboBox index and try to update the data again
                cbMitarbeiterID.SelectedIndex = currentIndex;
                this.selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
                string oCurrentID = selectedWorker.ID; // now oCurrentID is the ID 

                // Try to save again, but do not ask for confirmation this time
                UpdateEmployeeData(oCurrentID);
                insertDatabaseInComboBox();
                cbMitarbeiterID.SelectedIndex = currentIndex;
                FillData();
            }

        }

        private void UpdateEmployeeData(string employeeId)
        {
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"UPDATE Mitarbeiter
                                SET Firma = @Firma,
                                Vorname = @Vorname,
                                Nachname = @Nachname,
                                Geburtsdatum = @Geburtsdatum,
                                Geburtsland = @Geburtsland,
                                Wohnort = @Wohnort,
                                ChipNummer = @ChipNummer,
                                Gender = @Gender,
                                Muttersprache = @Muttersprache,
                                Sprachen = @Sprachen,
                                TelefonNummer = @TelefonNummer,
                                Ansprechpartner = @Ansprechpartner,
                                Position = @Position
                                WHERE MitarbeiterID = @EmployeeId";
                    cmd.Parameters.AddWithValue("@Firma", string.IsNullOrWhiteSpace(cbCompany.Text) ? (object)DBNull.Value : cbCompany.Text);
                    cmd.Parameters.AddWithValue("@Vorname", string.IsNullOrWhiteSpace(tbName.Text) ? (object)DBNull.Value : tbName.Text);
                    cmd.Parameters.AddWithValue("@Nachname", string.IsNullOrWhiteSpace(tbSurName.Text) ? (object)DBNull.Value : tbSurName.Text);
                    cmd.Parameters.AddWithValue("@Geburtsdatum", string.IsNullOrWhiteSpace(tbBirthday.Text) ? (object)DBNull.Value : tbBirthday.Text);
                    cmd.Parameters.AddWithValue("@Geburtsland", string.IsNullOrWhiteSpace(tbBirthPlace.Text) ? (object)DBNull.Value : tbBirthPlace.Text);
                    cmd.Parameters.AddWithValue("@Wohnort", string.IsNullOrWhiteSpace(tbLiving.Text) ? (object)DBNull.Value : tbLiving.Text);
                    cmd.Parameters.AddWithValue("@ChipNummer", string.IsNullOrWhiteSpace(tbChip.Text) ? (object)DBNull.Value : tbChip.Text);
                    cmd.Parameters.AddWithValue("@Gender", string.IsNullOrWhiteSpace(cbGender.Text) ? (object)DBNull.Value : cbGender.Text);
                    cmd.Parameters.AddWithValue("@Muttersprache", string.IsNullOrWhiteSpace(tbLanguage.Text) ? (object)DBNull.Value : tbLanguage.Text);
                    cmd.Parameters.AddWithValue("@Sprachen", string.IsNullOrWhiteSpace(cbOtherLanguage.Text) ? (object)DBNull.Value : cbOtherLanguage.Text);
                    cmd.Parameters.AddWithValue("@TelefonNummer", string.IsNullOrWhiteSpace(tbNumber.Text) ? (object)DBNull.Value : tbNumber.Text);
                    cmd.Parameters.AddWithValue("@Ansprechpartner", string.IsNullOrWhiteSpace(tbContact.Text) ? (object)DBNull.Value : tbContact.Text);
                    cmd.Parameters.AddWithValue("@Position", string.IsNullOrWhiteSpace(tbPosition.Text) ? (object)DBNull.Value : tbPosition.Text);
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void FillData()
        {
            try
            {
                this.selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
                string oCurrentID = selectedWorker.ID; // now oCurrentID is the ID string
                this.currentIndex = cbMitarbeiterID.SelectedIndex;
                FillEmployeeData(oCurrentID);
            }
            catch
            {
                // Reset the ComboBox index and reload the data
                cbMitarbeiterID.SelectedIndex = currentIndex;
                this.selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
                string oCurrentID = selectedWorker.ID; // now oCurrentID is the ID string
                FillEmployeeData(oCurrentID);
            }
        }

        private void FillEmployeeData(string employeeId)
        {
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT * FROM Mitarbeiter WHERE MitarbeiterID = @EmployeeId", conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cbCompany.Text = reader["Firma"].ToString();
                            tbName.Text = reader["Vorname"].ToString();
                            tbSurName.Text = reader["Nachname"].ToString();
                            tbBirthday.Text = reader["Geburtsdatum"].ToString();
                            tbBirthPlace.Text = reader["Geburtsland"].ToString();
                            tbLiving.Text = reader["Wohnort"].ToString();
                            lbCheckedIn.Text = reader["CheckInState"].ToString() == "true" ? "Eingechecked" : "Ausgechecked";
                            tbChip.Text = reader["ChipNummer"].ToString();
                            cbGender.Text = reader["Gender"].ToString();
                            tbLanguage.Text = reader["Muttersprache"].ToString();
                            cbOtherLanguage.Text = reader["Sprachen"].ToString();
                            tbNumber.Text = reader["TelefonNummer"].ToString();
                            tbContact.Text = reader["Ansprechpartner"].ToString();
                            tbPosition.Text = reader["Position"].ToString();
                        }
                    }
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                this.selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
                string currentID = selectedWorker.ID; // now oCurrentID is the ID string
                this.currentIndex = cbMitarbeiterID.SelectedIndex;

                // Ask the user to confirm the deletion
                var confirmResult = MessageBox.Show("Sind Sie sicher, dass Sie diesen Mitarbeiter löschen möchten?",
                                                     "Bestätigen Sie die Löschung!",
                                                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    using (var conn = new SQLiteConnection(stConnectionString))
                    {
                        conn.Open();
                        using (var cmd = new SQLiteCommand(conn))
                        {
                            // Delete the employee
                            cmd.CommandText = "DELETE FROM Mitarbeiter WHERE MitarbeiterID = @EmployeeId";
                            cmd.Parameters.AddWithValue("@EmployeeId", currentID);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Refresh the data in the UI
                    insertDatabaseInComboBox();
                    // Adjust the selected index to the next employee in the list
                    cbMitarbeiterID.SelectedIndex = currentIndex < cbMitarbeiterID.Items.Count ? currentIndex : cbMitarbeiterID.Items.Count - 1;
                    FillData();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., show an error message to the user)
                MessageBox.Show("Fehler beim Löschen des Mitarbeiters: " + ex.Message);
            }
        }
        }
}