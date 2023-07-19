using System.Data.SQLite;
using System.Globalization;

namespace Festival_Manager
{
    public partial class cPersonalOverview : Form
    {
        static string folderPath = cDataBase.DbPath;
        static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        bool isAdmin = false;
        string username;
        cWorker selectedWorker;
        string selectedCompany;
        int currentIndex;
        int isLoading;
        string chosenID;
        public cPersonalOverview(bool isAdmin, string username, string chosenID = null)
        {
            InitializeComponent();
            this.chosenID = chosenID;
            this.username = username;
            this.isAdmin = isAdmin;
        }

        private void insertDatabaseInComboBox()
        {
            cbMitarbeiterID.Items.Clear();
            cbCompany.Items.Clear();
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT DISTINCT ChipNummer,MitarbeiterID, Nachname || ' ' || Vorname AS Name,Position FROM Mitarbeiter", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            string id = reader["MitarbeiterID"].ToString();
                            string position = reader["Position"].ToString();
                            string chipNumber = reader["ChipNummer"].ToString();

                            // Create a new cWorker object
                            cWorker mitarbeiter = new cWorker { ID = id, Name = name, Position = position, ChipNumber = chipNumber };
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

                using (var cmd = new SQLiteCommand("SELECT MitarbeiterID,ChipNummer, Nachname || ' ' || Vorname AS Name, Position FROM Mitarbeiter WHERE Firma = @Company", conn))
                {
                    cmd.Parameters.AddWithValue("@Company", selectedCompany);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            string id = reader["MitarbeiterID"].ToString();
                            string position = reader["Position"].ToString();
                            string chipNumber = reader["ChipNummer"].ToString();

                            // Create a new cWorker object
                            cWorker mitarbeiter = new cWorker { ID = id, Name = name, Position = position, ChipNumber = chipNumber };
                            cbMitarbeiterID.Items.Add(mitarbeiter);
                        }
                    }
                }
            }
        }

        private void cbMitarbeiterID_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillData();
            CheckChipIDForCurrentEmployee();
        }
        private void cPersonalOverview_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            isLoading = 2;
            insertDatabaseInComboBox();
            if (chosenID != null)
            {
                // Find the index of the item in the ComboBox that matches chosenID
                int index = -1;
                for (int i = 0; i < cbMitarbeiterID.Items.Count; i++)
                {
                    cWorker item = cbMitarbeiterID.Items[i] as cWorker;  // Cast the item to Mitarbeiter
                    if (item != null && item.ID == chosenID) // Check if the MitarbeiterID matches chosenID
                    {
                        index = i;
                        break;
                    }
                }

                // If the item is found, set the selected index to the index of the found item
                if (index != -1)
                {
                    cbMitarbeiterID.SelectedIndex = index;
                    this.currentIndex = cbMitarbeiterID.SelectedIndex;
                }
            }
            else
            {
                cbMitarbeiterID.SelectedIndex = 0;
            }
            FillData();
        }



        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            InsertEmployeesBasedOnCompanyIntoComboBox();

        }

        private void bAddWorker_Click(object sender, EventArgs e)
        {
            bool hasRights = CheckRights();
            if (!hasRights)
            {
                return;
            }
            cPersonalManuellHinzufügen cPersonalManuellHinzufügen = new cPersonalManuellHinzufügen(username);
            cPersonalManuellHinzufügen.ShowDialog();
            insertDatabaseInComboBox();
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

        private void button1_Click(object sender, EventArgs e)
        {

            bool hasRights = CheckRights();
            if (!hasRights)
            {
                return;
            }
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
            DateTime dt;
            object geburtsdatumValue = DBNull.Value;
            if (DateTime.TryParseExact(tbBirthday.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                geburtsdatumValue = dt.ToString("dd.MM.yyyy");
            }
            else if (string.IsNullOrWhiteSpace(tbBirthday.Text))
            {

            }
            else
            {
                MessageBox.Show("Ungültiges Datumsformat! Bitte geben Sie das Datum im Format 'Tag.Monat.Jahr' ein. Beispiel: 01.01.2020.");
                return;
            }




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
                    cmd.Parameters.AddWithValue("@Geburtsdatum", string.IsNullOrWhiteSpace(tbBirthday.Text) ? (object)DBNull.Value : geburtsdatumValue);
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
        private void FillData()
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

        private bool CheckRights()
        {
            bool hasRights = false;
            Form validateUser = new Form
            {
                Width = 220,
                Height = 150,
                Text = "Enter-PIN"
            };
            validateUser.StartPosition = FormStartPosition.CenterScreen;
            Label lblPIN = new Label() { Left = 50, Top = 0, Width = 200, Text = "PIN:" };
            TextBox txtPin = new TextBox() { Left = 50, Top = 20, Width = 200 };
            Button bConfirm = new Button() { Text = "Bestätigen", Dock = DockStyle.Bottom };
            bConfirm.Width = 100; // Setzt die Breite
            bConfirm.Height = 30; // Setzt die Höhe

            cPasswordManager.CheckRights checkRights = new cPasswordManager.CheckRights();
            bConfirm.Click += (sender, e) =>
            {
                bool canEdit = checkRights.AuthenticateUser(username, txtPin.Text);
                if (!canEdit)
                {
                    MessageBox.Show("Ihre PIN ist falsch, bitte probieren Sie es erneut",
                                    "Falsche PIN", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    hasRights = false;
                }
                else
                {
                    validateUser.Close();
                    hasRights = true;
                }
            };

            validateUser.Controls.Add(lblPIN);
            validateUser.Controls.Add(txtPin);
            validateUser.Controls.Add(bConfirm);
            validateUser.StartPosition = FormStartPosition.CenterScreen;

            if (checkRights.CanEdit(username))
            {
                validateUser.TopMost = true;
                validateUser.ShowDialog();
            }
            else if (checkRights.rightCheck(username) == "admin")
            {
                hasRights = true;

            }
            else
            {
                MessageBox.Show("Sie dürfen dies nicht.",
                                "Fehlende Rechte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                hasRights = false;
            }
            return hasRights;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            bool hasRights = CheckRights();
            if (!hasRights)
            {
                return;
            }
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

        private void CheckChipIDForCurrentEmployee()
        {
            if (isLoading != 0)
            {
                isLoading = isLoading - 1;
                return;
            }
            cbMitarbeiterID.SelectedIndex = currentIndex;
            this.selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
            string employeeId = selectedWorker.ID;

            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"SELECT ChipNummer FROM Mitarbeiter WHERE MitarbeiterID = @EmployeeId";
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                    var result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                    {
                        MessageBox.Show("Der aktuelle Mitarbeiter hat keine Chip-Nummer. Bitte fügen Sie eine Chip-Nummer hinzu.",
                                        "Fehlende Chip-Nummer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void cPersonalOverview_Shown(object sender, EventArgs e)
        {
            CheckChipIDForCurrentEmployee();
        }
    }
}