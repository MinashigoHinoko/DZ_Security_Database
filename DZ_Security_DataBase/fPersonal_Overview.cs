using PCSC;
using System.Data.SQLite;
using System.Globalization;

namespace Festival_Manager
{
    public partial class cPersonalOverview : Form
    {
        private static string folderPath = cDataBase.DbPath;
        private static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        private bool isAdmin = false;
        private string username;
        private cWorker selectedWorker;
        private string selectedCompany;
        private int currentIndex;
        private bool isFormLoading = true;
        private List<cWorker> allWorkers = new();
        private bool deleteLanguage = false;
        private int langIndex = 0;
        private string chosenID;
        private bool isFirstKeyPress = true;
        private bool userIsTyping = false;

        public cPersonalOverview(bool isAdmin, string username, string chosenID = null)
        {
            InitializeComponent();
            this.chosenID = chosenID;
            this.username = username;
            this.isAdmin = isAdmin;
        }
        private bool isUpdatingComboBox = false;
        private void cbMitarbeiterID_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if it's a letter or digit and it's the first key press
            if (isFirstKeyPress && char.IsLetterOrDigit((char)e.KeyCode))
            {
                cbMitarbeiterID.Text = "";
                userIsTyping = true;
                isFirstKeyPress = false;
            }
        }
        private void cbMitarbeiterID_KeyUp(object sender, KeyEventArgs e)
        {
            userIsTyping = false;
        }


        private void cbMitarbeiterID_TextChanged(object sender, EventArgs e)
        {
            if (isUpdatingComboBox)
            {
                return;
            }

            string searchText = cbMitarbeiterID.Text;
            List<cWorker> matchingWorkers = new();

            string[] parts = searchText.Split(' ');

            foreach (cWorker worker in allWorkers)
            {
                bool allPartMatch = true;
                foreach (string part in parts)
                {
                    if (!worker.ToString().ToLower().Contains(part.ToLower()))
                    {
                        allPartMatch = false;
                        break;
                    }
                }
                if (allPartMatch)
                {
                    matchingWorkers.Add(worker);
                }
            }

            if (matchingWorkers.Count == 1)
            {
                // Wenn nur ein Mitarbeiter übereinstimmt, setzen Sie den ausgewählten Index auf den Index des übereinstimmenden Mitarbeiters
                cbMitarbeiterID.SelectedIndex = cbMitarbeiterID.Items.IndexOf(matchingWorkers[0]);
                // Verschieben Sie den Fokus auf ein anderes Steuerelement, um zu verhindern, dass der Benutzer weiter tippt
                button2.Focus(); // Ersetzen Sie "SomeOtherControl" durch den Namen eines anderen Steuerelements in Ihrem Formular
            }
            else
            {
                // Wenn mehrere Mitarbeiter übereinstimmen, aktualisieren Sie die ComboBox mit den übereinstimmenden Mitarbeitern
                isUpdatingComboBox = true;
                cbMitarbeiterID.Items.Clear();
                foreach (cWorker worker in matchingWorkers)
                {
                    cbMitarbeiterID.Items.Add(worker);
                }
                cbMitarbeiterID.Text = searchText; // Setzen Sie den Text der ComboBox auf den gespeicherten Text
                cbMitarbeiterID.SelectionStart = searchText.Length; // Setzen Sie den Cursor am Ende des Textes
                isUpdatingComboBox = false;
            }

            cbMitarbeiterID.DroppedDown = true;
        }

        private void insertDatabaseInComboBox()
        {
            cbMitarbeiterID.Items.Clear();
            allWorkers.Clear(); // clear the allWorkers list

            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new("SELECT DISTINCT ChipNummer,MitarbeiterID, Nachname || ' ' || Vorname AS Name,Position FROM Mitarbeiter", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            string id = reader["MitarbeiterID"].ToString();
                            string position = reader["Position"].ToString();
                            string chipNumber = reader["ChipNummer"].ToString();

                            // Create a new cWorker object
                            cWorker mitarbeiter = new() { ID = id, Name = name, Position = position, ChipNumber = chipNumber };

                            allWorkers.Add(mitarbeiter); // Add the worker to the list of all workers
                            cbMitarbeiterID.Items.Add(mitarbeiter); // Add the worker object to the ComboBox
                        }
                    }
                }
            }

            cbCompany.Items.Clear();
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new("SELECT DISTINCT Firma FROM Mitarbeiter", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
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

            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new("SELECT MitarbeiterID,ChipNummer, Nachname || ' ' || Vorname AS Name, Position FROM Mitarbeiter WHERE Firma = @Company", conn))
                {
                    cmd.Parameters.AddWithValue("@Company", selectedCompany);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            string id = reader["MitarbeiterID"].ToString();
                            string position = reader["Position"].ToString();
                            string chipNumber = reader["ChipNummer"].ToString();

                            // Create a new cWorker object
                            cWorker mitarbeiter = new() { ID = id, Name = name, Position = position, ChipNumber = chipNumber };
                            cbMitarbeiterID.Items.Add(mitarbeiter);
                        }
                    }
                }
            }
        }
        // Event handler method
        private void cbMitarbeiterID_Leave(object sender, EventArgs e)
        {
            isFirstKeyPress = true;
        }
        private void cbMitarbeiterID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string checkInState;
            cWorker selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
            string oCurrentID = selectedWorker.ID.ToString().Trim();
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new("SELECT CheckInState FROM Mitarbeiter WHERE MitarbeiterID = @EmployeeId", conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", oCurrentID);
                    checkInState = cmd.ExecuteScalar().ToString();
                }
                conn.Close();
            }

            if (checkInState == "true")
            {
                bCheckIn.Visible = false;
                bCheckOut.Visible = true;
            }
            else
            {
                bCheckIn.Visible = true;
                bCheckOut.Visible = false;
            }

            // Only call FillData if the SelectedIndex has actually changed

            if (cbMitarbeiterID.SelectedIndex != currentIndex)
            {
                currentIndex = cbMitarbeiterID.SelectedIndex;
                cbMitarbeiterID.DroppedDown = false;
                FillData();
                CheckChipIDForCurrentEmployee();
                isFirstKeyPress = true;
            }
        }
        private void cPersonalOverview_Load(object sender, EventArgs e)
        {
            bool hasRights = CheckRights();
            if (hasRights)
            {
                button1.Visible = true; button2.Visible = true; bAddWorker.Visible = true;
            }
            cbMitarbeiterID.Leave += cbMitarbeiterID_Leave;
            cbMitarbeiterID.KeyDown += cbMitarbeiterID_KeyDown;
            StartPosition = FormStartPosition.CenterScreen;
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
                    currentIndex = cbMitarbeiterID.SelectedIndex;
                }
            }
            else
            {
                if (cbMitarbeiterID.Items.Count > 0)
                {
                    cbMitarbeiterID.SelectedIndex = 0;
                }
            }
            if (cbMitarbeiterID.SelectedIndex != -1) // Check if an item is selected in the ComboBox
            {
                FillData();
            }
            isFormLoading = false;
        }



        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {

            InsertEmployeesBasedOnCompanyIntoComboBox();

        }

        private void bAddWorker_Click(object sender, EventArgs e)
        {
            cPersonalManuellHinzufügen cPersonalManuellHinzufügen = new(username);
            cPersonalManuellHinzufügen.ShowDialog();
            insertDatabaseInComboBox();
        }
        private void cEquipmentRent_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (cbMitarbeiterID.SelectedItem is cWorker selectedWorker)
            {
                string oCurrentID = selectedWorker.ID; // now oCurrentID is the ID 
                currentIndex = cbMitarbeiterID.SelectedIndex;

                // Ask the user to confirm the update
                DialogResult confirmResult = MessageBox.Show("Möchten Sie die Änderungen speichern?",
                                                    "Bestätigen Sie das Speichern!",
                                                    MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    // Prüfen Sie, ob der Status "Löschen" ist
                    if (deleteLanguage)
                    {
                        // Entfernen Sie das aktuell ausgewählte Element aus der ComboBox
                        if (cbOtherLanguage.Items.Count >= 0)
                        {
                            cLogger.LogDatabaseChange($"Sprache {cbOtherLanguage.Items[langIndex].ToString()} vom nutzer {cbMitarbeiterID.SelectedItem.ToString()} wurde gelöscht", username);
                            cbOtherLanguage.Items.RemoveAt(langIndex);
                        }
                        // Setzen Sie den Status zurück
                        deleteLanguage = false;
                    }
                    string selectedLanguage = cbOtherLanguage.Text;
                    if (!string.IsNullOrEmpty(selectedLanguage))
                    {
                        // Wenn der ausgewählte Text nicht leer ist und die Sprache noch nicht existiert, fügen Sie sie hinzu
                        if (!cbOtherLanguage.Items.Contains(selectedLanguage))
                        {
                            cLogger.LogDatabaseChange($"Sprache {selectedLanguage} wurde zum nutzer {cbMitarbeiterID.SelectedItem.ToString()} hinzugefügt", username);
                            cbOtherLanguage.Items.Add(selectedLanguage);
                        }
                    }
                    try
                    {
                        UpdateEmployeeData(oCurrentID);
                        insertDatabaseInComboBox();
                        if (currentIndex < cbMitarbeiterID.Items.Count)
                        {
                            cbMitarbeiterID.SelectedIndex = currentIndex;
                            FillData();
                        }
                    }
                    catch
                    {
                        // Reset the ComboBox index and try to update the data again
                        if (currentIndex < cbMitarbeiterID.Items.Count)
                        {
                            cbMitarbeiterID.SelectedIndex = currentIndex;
                            if (cbMitarbeiterID.SelectedItem is cWorker retrySelectedWorker)
                            {
                                oCurrentID = retrySelectedWorker.ID;
                                // Try to save again, but do not ask for confirmation this time
                                UpdateEmployeeData(oCurrentID);
                                insertDatabaseInComboBox();
                                cbMitarbeiterID.SelectedIndex = currentIndex;
                                FillData();
                            }
                        }
                    }
                    cLogger.LogDatabaseChange($"Änderungen an {cbMitarbeiterID.SelectedItem.ToString()} gespeichert", username);
                    MessageBox.Show($"Änderungen an '{cbMitarbeiterID.SelectedItem.ToString()}' erfolgreisch gespeichert!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Keinen Mitarbeiter ausgewählt.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateEmployeeLanguages(string employeeId, string motherLanguage, List<string> otherLanguages)
        {
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new(conn))
                {
                    // Delete all languages of the employee
                    cmd.CommandText = "DELETE FROM MitarbeiterSprachen WHERE MitarbeiterID = @EmployeeId";
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    cmd.ExecuteNonQuery();

                    // Insert mother language of the employee
                    cmd.CommandText = "INSERT INTO MitarbeiterSprachen (MitarbeiterID, Sprache, Muttersprache) VALUES (@EmployeeId, @Language, @IsMotherLanguage)";
                    cmd.Parameters.AddWithValue("@Language", motherLanguage);
                    cmd.Parameters.AddWithValue("@IsMotherLanguage", true);
                    cmd.ExecuteNonQuery();

                    // Insert other languages of the employee
                    cmd.Parameters["@IsMotherLanguage"].Value = false;
                    foreach (string language in otherLanguages)
                    {
                        cmd.Parameters["@Language"].Value = language;
                        cmd.ExecuteNonQuery();
                    }
                }
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




            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new(conn))
                {
                    cmd.CommandText = @"UPDATE Mitarbeiter
                        SET Firma = @Firma,
                        Vorname = @Vorname,
                        Nachname = @Nachname,
                        Geburtsname = @geburtsname,
                        Geburtsdatum = @Geburtsdatum,
                        Geburtsland = @Geburtsland,
                        Wohnort = @Wohnort,
                        ChipNummer = @ChipNummer,
                        Gender = @Gender,
                        Ansprechpartner = @Ansprechpartner,
                        Position = @Position
                        WHERE MitarbeiterID = @EmployeeId";
                    cmd.Parameters.AddWithValue("@Firma", string.IsNullOrWhiteSpace(cbCompany.Text) ? DBNull.Value : cbCompany.Text);
                    cmd.Parameters.AddWithValue("@Vorname", string.IsNullOrWhiteSpace(tbName.Text) ? DBNull.Value : tbName.Text);
                    cmd.Parameters.AddWithValue("@Nachname", string.IsNullOrWhiteSpace(tbSurName.Text) ? DBNull.Value : tbSurName.Text);
                    cmd.Parameters.AddWithValue("@Geburtsdatum", string.IsNullOrWhiteSpace(tbBirthday.Text) ? DBNull.Value : geburtsdatumValue);
                    cmd.Parameters.AddWithValue("@Geburtsland", string.IsNullOrWhiteSpace(tbBirthPlace.Text) ? DBNull.Value : tbBirthPlace.Text);
                    cmd.Parameters.AddWithValue("@Wohnort", string.IsNullOrWhiteSpace(tbLiving.Text) ? DBNull.Value : tbLiving.Text);
                    cmd.Parameters.AddWithValue("@ChipNummer", string.IsNullOrWhiteSpace(tbChip.Text) ? DBNull.Value : tbChip.Text);
                    cmd.Parameters.AddWithValue("@Gender", string.IsNullOrWhiteSpace(cbGender.Text) ? DBNull.Value : cbGender.Text);
                    cmd.Parameters.AddWithValue("@geburtsname", string.IsNullOrWhiteSpace(tbBirthName.Text) ? DBNull.Value : tbBirthName.Text);
                    cmd.Parameters.AddWithValue("@Ansprechpartner", string.IsNullOrWhiteSpace(tbContact.Text) ? DBNull.Value : tbContact.Text);
                    cmd.Parameters.AddWithValue("@Position", string.IsNullOrWhiteSpace(tbPosition.Text) ? DBNull.Value : tbPosition.Text);
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    cmd.ExecuteNonQuery();
                    string motherLanguage = tbLanguage.Text;
                    List<string> otherLanguages = cbOtherLanguage.Items.Cast<string>().ToList();
                    UpdateEmployeeLanguages(employeeId, motherLanguage, otherLanguages);
                }
            }
        }
        private void FillData()
        {
            if (cbMitarbeiterID.Items.Count > 0)
            {
                // Reset the ComboBox index and reload the data
                cbMitarbeiterID.SelectedIndex = currentIndex;
                selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
                if (selectedWorker != null)
                {
                    string oCurrentID = selectedWorker.ID; // now oCurrentID is the ID string
                    FillEmployeeData(oCurrentID);
                }

            }
        }
        private void FillEmployeeData(string employeeId)
        {
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new("SELECT * FROM Mitarbeiter WHERE MitarbeiterID = @EmployeeId", conn))
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
                            tbBirthName.Text = reader["Geburtsname"].ToString();
                            tbContact.Text = reader["Ansprechpartner"].ToString();
                            tbPosition.Text = reader["Position"].ToString();
                        }
                    }
                }

                using (SQLiteCommand cmd = new("SELECT * FROM MitarbeiterSprachen WHERE MitarbeiterID = @EmployeeId", conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        List<string> languages = new();
                        string motherLanguage = string.Empty;
                        while (reader.Read())
                        {
                            bool isMotherLanguage = Convert.ToBoolean(reader["Muttersprache"]);
                            string language = reader["Sprache"].ToString();
                            if (isMotherLanguage)
                            {
                                motherLanguage = language;
                            }
                            else
                            {
                                languages.Add(language);
                            }
                        }

                        tbLanguage.Text = motherLanguage;
                        cbOtherLanguage.Items.Clear();  // Clear existing items
                        languages.Sort();  // Sort the languages list
                        foreach (string language in languages)
                        {
                            cbOtherLanguage.Items.Add(language);  // Add each language as a new item
                        }
                    }
                }
                if (cbOtherLanguage.Items.Count > 0)
                {
                    cbOtherLanguage.SelectedIndex = 0;  // Set the selected index only if there is at least one item
                }


            }
        }
        private bool CheckRights()
        {
            bool hasRights = false;
            Form validateUser = new()
            {
                Width = 220,
                Height = 150,
                Text = "Enter-PIN"
            };
            validateUser.StartPosition = FormStartPosition.CenterScreen;
            Label lblPIN = new() { Left = 50, Top = 0, Width = 200, Text = "PIN:" };
            TextBox txtPin = new() { Left = 50, Top = 20, Width = 200 };
            Button bConfirm = new() { Text = "Bestätigen", Dock = DockStyle.Bottom };
            bConfirm.Width = 100; // Setzt die Breite
            bConfirm.Height = 30; // Setzt die Höhe

            cPasswordManager.CheckRights checkRights = new();
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
            try
            {
                selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
                string currentID = selectedWorker.ID; // now oCurrentID is the ID string
                currentIndex = cbMitarbeiterID.SelectedIndex;

                // Ask the user to confirm the deletion
                DialogResult confirmResult = MessageBox.Show("Sind Sie sicher, dass Sie diesen Mitarbeiter löschen möchten?",
                                                     "Bestätigen Sie die Löschung!",
                                                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    using (SQLiteConnection conn = new(stConnectionString))
                    {
                        conn.Open();
                        using (SQLiteCommand cmd = new(conn))
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
            cbMitarbeiterID.SelectedIndex = currentIndex;
            selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
            string employeeId = selectedWorker.ID;

            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new(conn))
                {
                    cmd.CommandText = @"SELECT ChipNummer FROM Mitarbeiter WHERE MitarbeiterID = @EmployeeId";
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                    object result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                    {
                        MessageBox.Show("Der aktuelle Mitarbeiter hat keine Chip-Nummer. Bitte fügen Sie eine Chip-Nummer hinzu.",
                                        "Fehlende Chip-Nummer", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        // Start der NFC-Reader-Code
                        using (SCardContext context = new())
                        {
                            context.Establish(SCardScope.System);

                            string readerName = context.GetReaders().FirstOrDefault();

                            if (string.IsNullOrEmpty(readerName) || !readerName.Contains("ACR122U"))
                            {
                                string chipId = Microsoft.VisualBasic.Interaction.InputBox("Bitte geben Sie die Chip-ID manuell ein:", "Manuelle Eingabe", "");
                                cmd.CommandText = @"UPDATE Mitarbeiter SET ChipNummer = @ChipNummer WHERE MitarbeiterID = @EmployeeId";
                                cmd.Parameters.AddWithValue("@ChipNummer", chipId);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show($"Die Chip-ID wurde erfolgreich gesetzt: {chipId}", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                            using (SCardReader reader = new(context))
                            {
                                // Connect to the card using T1 protocol
                                SCardError sc = reader.Connect(readerName, SCardShareMode.Shared, SCardProtocol.T1);

                                if (sc == SCardError.Success)
                                {
                                    byte[] receiveBuffer = new byte[256];

                                    // Transmit the command to the card
                                    sc = reader.Transmit(
                                        SCardPCI.T1, // Protocol Control Information (PCI) for the send protocol
                                        new byte[] { 0xFF, 0xCA, 0x00, 0x00, 0x00 }, // Command APDU
                                        ref receiveBuffer); // Receive buffer

                                    if (sc != SCardError.Success)
                                    {
                                        MessageBox.Show("Fehler beim Auslesen der Chip-ID.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    string chipId = BitConverter.ToString(receiveBuffer);
                                    cmd.CommandText = @"UPDATE Mitarbeiter SET ChipNummer = @ChipNummer WHERE MitarbeiterID = @EmployeeId";
                                    cmd.Parameters.AddWithValue("@ChipNummer", chipId);
                                    cmd.ExecuteNonQuery();
                                    insertDatabaseInComboBox();
                                    currentIndex = cbMitarbeiterID.SelectedIndex;
                                    FillData();
                                    MessageBox.Show($"Die Chip-ID wurde erfolgreich gesetzt: {chipId}", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Fehler beim Verbinden mit dem NFC-Lesegerät.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        // Ende der NFC-Reader-Code
                    }
                }
            }
        }
        private void cPersonalOverview_Shown(object sender, EventArgs e)
        {
        }

        private void cbOtherLanguage_TextChanged(object sender, EventArgs e)
        {
            if (cbOtherLanguage != null)
            {
                // Prüfen Sie, ob der Textinhalt zu einem leeren String geändert wurde
                if (string.IsNullOrEmpty(cbOtherLanguage.Text))
                {
                    // Setzen Sie den Status auf "Löschen"
                    deleteLanguage = true;
                }
                else
                {
                    deleteLanguage = false;
                }
            }
        }

        private void cbOtherLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            langIndex = cbOtherLanguage.SelectedIndex;
        }

        private void bCheckIn_Click(object sender, EventArgs e)
        {
            cWorker selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
            string oCurrentID = selectedWorker.ID;
            object checkInState;
            if (!selectedWorker.CheckInState)
            {
                bool getRented = false;
                DateTime checkInTime = DateTime.Now;
                DateTime scheduledCheckInTime;

                using (SQLiteConnection conn = new(stConnectionString))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = new("SELECT CheckedInSoll FROM ArbeitszeitenSoll WHERE MitarbeiterID = @EmployeeId", conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", oCurrentID);
                        checkInState = cmd.ExecuteScalar();
                        if (checkInState != null)
                        {
                            scheduledCheckInTime = Convert.ToDateTime(checkInState);
                        }
                        else
                        {
                            scheduledCheckInTime = checkInTime;
                        }
                    }

                    using (SQLiteCommand cmd = new("SELECT COUNT(*) FROM Mitarbeiter WHERE MitarbeiterID = @EmployeeId AND RentState = 'true'", conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", oCurrentID);

                        int rowCount = Convert.ToInt32(cmd.ExecuteScalar());

                        getRented = rowCount > 0;
                    }
                    conn.Close();
                }

                // Dialog zur Auswahl der Check-In-Zeit
                Form checkInTimeForm = new();
                checkInTimeForm.Width = 300;
                checkInTimeForm.Height = 200;
                checkInTimeForm.Text = "Check-In-Zeit auswählen";
                checkInTimeForm.StartPosition = FormStartPosition.CenterScreen;

                RadioButton useScheduledTimeButton = new() { Text = "Soll-Zeit verwenden", Dock = DockStyle.Top, Checked = true };
                RadioButton enterOwnTimeButton = new() { Text = "Eigene Zeit eingeben", Dock = DockStyle.Top };


                DateTimePicker checkInTimePicker = new() { Format = DateTimePickerFormat.Time, Dock = DockStyle.Top };
                checkInTimePicker.Value = DateTime.Now;

                useScheduledTimeButton.Click += (sender, e) =>
                {

                    checkInTimePicker.Value = scheduledCheckInTime;
                };
                enterOwnTimeButton.Click += (sender, e) =>
                {
                    checkInTimePicker.Value = DateTime.Now;
                };


                checkInTimeForm.Controls.Add(useScheduledTimeButton);
                checkInTimeForm.Controls.Add(enterOwnTimeButton);
                checkInTimeForm.Controls.Add(checkInTimePicker);
                if (checkInState != null)
                {
                    useScheduledTimeButton.Visible = true;
                }
                else
                {
                    useScheduledTimeButton.Visible = false;
                }
                Button confirmButton = new() { Text = "Bestätigen", Dock = DockStyle.Bottom, Height = 50 };
                checkInTimeForm.Controls.Add(confirmButton);

                confirmButton.Click += (sender, e) =>
                {
                    checkInTime = checkInTimePicker.Value;
                    MessageBox.Show($"Eingechecked mit der Zeit {checkInTime}", "Bestätigung", MessageBoxButtons.OK);
                    checkInTimeForm.Close();
                };

                checkInTimeForm.ShowDialog();

                if (!getRented)
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
                        cbMitarbeiterID_SelectedIndexChanged(sender, e);

                    }
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
        }

        private void bCheckOut_Click(object sender, EventArgs e)
        {

            cWorker selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
            string oCurrentID = selectedWorker.ID;
            object checkInState;
            if (!selectedWorker.CheckInState)
            {
                bool getRented = false;
                DateTime checkOutTime = DateTime.Now;
                DateTime scheduledCheckOutTime;

                using (SQLiteConnection conn = new(stConnectionString))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = new("SELECT CheckedOutSoll FROM ArbeitszeitenSoll WHERE MitarbeiterID = @EmployeeId", conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", oCurrentID);
                        checkInState = cmd.ExecuteScalar();
                        if (checkInState != null)
                        {
                            scheduledCheckOutTime = Convert.ToDateTime(checkInState);
                        }
                        else
                        {
                            scheduledCheckOutTime = checkOutTime;
                        }
                    }

                    using (SQLiteCommand cmd = new("SELECT COUNT(*) FROM Mitarbeiter WHERE MitarbeiterID = @EmployeeId AND RentState = 'true'", conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", oCurrentID);

                        int rowCount = Convert.ToInt32(cmd.ExecuteScalar());

                        getRented = rowCount > 0;
                    }
                    conn.Close();
                }

                // Dialog zur Auswahl der Check-Out-Zeit
                Form checkOutTimeForm = new();
                checkOutTimeForm.Width = 300;
                checkOutTimeForm.Height = 200;
                checkOutTimeForm.Text = "Check-Out-Zeit auswählen";
                checkOutTimeForm.StartPosition = FormStartPosition.CenterScreen;

                RadioButton useScheduledTimeButton = new() { Text = "Soll-Zeit verwenden", Dock = DockStyle.Top };
                RadioButton enterOwnTimeButton = new() { Text = "Eigene Zeit eingeben", Dock = DockStyle.Top };


                DateTimePicker checkOutTimePicker = new() { Format = DateTimePickerFormat.Time, Dock = DockStyle.Top };
                checkOutTimePicker.Value = DateTime.Now;

                useScheduledTimeButton.Click += (sender, e) =>
                {

                    checkOutTimePicker.Value = scheduledCheckOutTime;
                };
                enterOwnTimeButton.Click += (sender, e) =>
                {
                    checkOutTimePicker.Value = DateTime.Now;
                };


                checkOutTimeForm.Controls.Add(useScheduledTimeButton);
                checkOutTimeForm.Controls.Add(enterOwnTimeButton);
                checkOutTimeForm.Controls.Add(checkOutTimePicker);
                if (checkInState != null)
                {
                    useScheduledTimeButton.Visible = true;
                    useScheduledTimeButton.Checked = true;
                }
                else
                {
                    useScheduledTimeButton.Visible = false;
                    enterOwnTimeButton.Checked = true;
                }
                Button confirmButton = new() { Text = "Bestätigen", Dock = DockStyle.Bottom, Height = 50 };
                checkOutTimeForm.Controls.Add(confirmButton);

                confirmButton.Click += (sender, e) =>
                {
                    checkOutTime = checkOutTimePicker.Value;
                    MessageBox.Show($"Ausgechecked mit der Zeit {checkOutTime}", "Bestätigung", MessageBoxButtons.OK);
                    checkOutTimeForm.Close();
                };

                checkOutTimeForm.ShowDialog();

                if (!getRented)
                {
                    cLogger.LogDatabaseChange($"CheckIn, MitarbeiterID: {oCurrentID}", username);
                    using (SQLiteConnection conn = new(stConnectionString))
                    {
                        conn.Open();

                        using (SQLiteCommand cmd = new(conn))
                        {
                            cmd.CommandText = @"
                                            UPDATE Arbeitszeiten 
                                            SET CheckedOut = @jetzt
                                            WHERE MitarbeiterID = @id AND CheckedOut IS NULL";
                            cmd.Parameters.AddWithValue("@id", oCurrentID);
                            cmd.Parameters.AddWithValue("@jetzt", checkOutTime.ToString("yyyy-MM-dd HH:mm:ss"));

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
                        cbMitarbeiterID_SelectedIndexChanged(sender, e);
                    }
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
        }

        private void bRent_Click(object sender, EventArgs e)
        {
            cEquipmentRent equip = new cEquipmentRent(isAdmin,username);
            equip.Show();
        }
    }
}