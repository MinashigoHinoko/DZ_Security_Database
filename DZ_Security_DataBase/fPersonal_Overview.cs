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
        private bool isUpdatingData = false;
        private bool isUserChange = true;
        private bool orderChanged = false;
        private Thread chipReaderThread;
        private AutoResetEvent chipReaderWaitHandle = new(false);
        private bool isUpdatingComboBox = false;
        private System.Timers.Timer chipCheckTimer;
        private bool running = true;


        public cPersonalOverview(bool isAdmin, string username)
        {
            InitializeComponent();
            this.username = username;
            this.isAdmin = isAdmin;

            chipReaderThread = new Thread(ChipReaderThread);
            chipReaderThread.Start();
        }
        // Die Methode, die auf dem Hintergrund-Thread ausgeführt wird

        private void ChipReaderThread()
        {
            // Initialize the timer
            chipCheckTimer = new System.Timers.Timer(1000); // 1000 milliseconds = 1 second
            chipCheckTimer.Elapsed += (sender, e) => CheckChip();
            chipCheckTimer.AutoReset = true; // Make sure the timer triggers again and again
            chipCheckTimer.Enabled = true; // Start the timer

            while (running)
            {
                // Wait for the signal that a chip has been read
                chipReaderWaitHandle.WaitOne();

                // Your remaining code...
                string chipId = backgroundChipCheck();
                if (chipId == string.Empty)
                {
                }
                else
                {
                    using (SQLiteConnection conn = new(stConnectionString))
                    {
                        conn.Open();
                        using (SQLiteCommand cmd = new(conn))
                        {
                            cmd.CommandText = @"SELECT COUNT(*) FROM Mitarbeiter WHERE ChipNummer = @chip";
                            cmd.Parameters.AddWithValue("@chip", chipId);

                            int count = Convert.ToInt32(cmd.ExecuteScalar());
                            if (!(count > 0))
                            {
                                MessageBox.Show("Chip-Nummer nicht im System");
                            }
                        }
                    }

                    // Set the ComboBox index on the main thread
                    Invoke((MethodInvoker)(() =>
                    {
                        isUserChange = false;
                        cWorker matchedWorker = null; // Initiate to null, indicating no match found yet

                        foreach (cWorker worker in allWorkers)
                        {
                            if (worker.ChipNumber.ToLower() == chipId.ToLower())
                            {
                                matchedWorker = worker; // If a match is found, set matchedWorker to the current worker
                                break;
                            }
                        }

                        if (matchedWorker != null)
                        {
                            cbMitarbeiterID.SelectedItem = matchedWorker; // Set the selected item to the matched worker
                            currentIndex = cbMitarbeiterID.SelectedIndex;
                        }
                        isUserChange = true;

                    }));
                }
            }
        }

        private void CheckChip()
        {
            string chipId = backgroundChipCheck();

            if (!string.IsNullOrEmpty(chipId))
            {
                // Signal the main thread that a chip has been read
                chipReaderWaitHandle.Set();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // Signal to the background thread to start reading
            chipReaderWaitHandle.Set();
        }

        private string backgroundChipCheck()
        {
            // Your code to read the chip
            string chipId = string.Empty;

            // Start of NFC reader code
            using (SCardContext context = new())
            {
                context.Establish(SCardScope.System);
                string readerName = context.GetReaders().FirstOrDefault();
                if (!string.IsNullOrEmpty(readerName) && readerName.Contains("ACR122"))
                {
                    using (SCardReader reader = new(context))
                    {
                        // Maximum number of connection attempts
                        SCardError sc;
                        // Connection attempts in a loop
                        do
                        {
                            // Connect to the card using T1 protocol
                            sc = reader.Connect(readerName, SCardShareMode.Shared, SCardProtocol.T1);
                            if (sc == SCardError.Success)
                            {
                                byte[] receiveBuffer = new byte[256];
                                // Transmit the command to the card
                                sc = reader.Transmit(
                                    SCardPCI.T1, // Protocol Control Information (PCI) for the send protocol
                                    new byte[] { 0xFF, 0xCA, 0x00, 0x00, 0x00 }, // Command APDU
                                    ref receiveBuffer); // Receive buffer
                                if (sc == SCardError.Success)
                                {
                                    string rawData = BitConverter.ToString(receiveBuffer).Replace("-", "");
                                    rawData = rawData.Substring(0, rawData.Length - 4); // Removes the last 4 characters (9000)
                                    chipId = rawData; // Assign chipId here
                                }
                                else
                                {
                                    // Error reading chip ID
                                }
                            }
                        }
                        while (sc != SCardError.Success);
                    }
                }
            }

            // At the end of the method, the read chip number should be returned
            return chipId; // The read chip number
        }

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

            // Wenn weniger als 3 Zeichen eingegeben wurden, führen Sie die Suche nicht durch
            if (searchText.Length < 3)
            {
                return;
            }

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
                cbMitarbeiterID.SelectedIndex = cbMitarbeiterID.Items.IndexOf(matchingWorkers[0]);
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
                    bCheckOut.Focus();
                }
                else
                {
                    bCheckIn.Focus();
                }
            }
            else
            {
                isUpdatingComboBox = true;
                cbMitarbeiterID.Items.Clear();
                foreach (cWorker worker in matchingWorkers)
                {
                    cbMitarbeiterID.Items.Add(worker);
                }
                cbMitarbeiterID.Text = searchText;
                cbMitarbeiterID.SelectionStart = searchText.Length;
                isUpdatingComboBox = false;
            }

            cbMitarbeiterID.DroppedDown = true;
        }
        private void insertDatabaseInComboBox()
        {
            isUserChange = false;
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
                conn.Close();
            }

            cbCompany.Items.Clear();
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new("SELECT DISTINCT Firma FROM Mitarbeiter", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        cbCompany.Items.Add("Alle Mitarbeiter");  // Add "All Employees" as the first item
                        while (reader.Read())
                        {
                            cbCompany.Items.Add(reader["Firma"].ToString());
                        }
                    }
                }
                conn.Close();
            }


            if (orderChanged)
            {
                cbMitarbeiterID.SelectedIndex = 0;
                currentIndex = cbMitarbeiterID.SelectedIndex;
                orderChanged = false;
            }
            isUserChange = true;
        }
        private void InsertEmployeesBasedOnCompanyIntoComboBox()
        {
            isUserChange = false;
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
                conn.Close();
            }
            cbMitarbeiterID.DroppedDown = true;
            isUserChange = true;
        }
        private void cbMitarbeiterID_Leave(object sender, EventArgs e)
        {
            isFirstKeyPress = true;
        }
        private void cbMitarbeiterID_SelectedIndexChanged(object sender, EventArgs e)
        {
            isUserChange = false;
            // If the data is currently being updated, do nothing
            if (isUpdatingData)
            {
                return;
            }

            // Only call FillData if the SelectedIndex has actually changed

            if (cbMitarbeiterID.SelectedIndex != currentIndex)
            {
                currentIndex = cbMitarbeiterID.SelectedIndex;
                if (cbMitarbeiterID.DroppedDown == true)
                {
                    cbMitarbeiterID.DroppedDown = false;
                }
                FillData();
                isFirstKeyPress = true;
                cbMitarbeiterID.SelectedIndex = currentIndex;
            }
            isUserChange = true;
        }
        private void cPersonalOverview_Load(object sender, EventArgs e)
        {
            isUserChange = false;
            button1.Visible = true; button2.Visible = true; bAddWorker.Visible = true;
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
            isUserChange = true;
        }
        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isUserChange)
            {
                return;
            }

            // If "Alle Mitarbeiter" is selected, show all employees
            if (cbCompany.SelectedItem.ToString() == "Alle Mitarbeiter")
            {
                insertDatabaseInComboBox();
                cbMitarbeiterID.DroppedDown = true;
            }
            else
            {
                // If a specific company is selected, show only the employees from that company
                InsertEmployeesBasedOnCompanyIntoComboBox();
            }
        }

        private void bAddWorker_Click(object sender, EventArgs e)
        {
            bool hasRights = CheckRights();
            if (hasRights)
            {
                chipCheckTimer.Stop();
                orderChanged = true;
                cPersonalManuellHinzufügen cPersonalManuellHinzufügen = new(username);
                cPersonalManuellHinzufügen.ShowDialog();
                insertDatabaseInComboBox();
                chipCheckTimer.Start();
            }
            else { MessageBox.Show("Sie haben keine Rechte hierfür", "error"); return; }
        }
        private void cEquipmentRent_FormClosed(object sender, FormClosedEventArgs e)
        {
            running = false;  // Dies wird die while-Schleife beenden
            chipCheckTimer.Stop();  // Stoppt den Timer
            Hide();

            if (isAdmin)
            {
                cAdminView cAdminView = new(username);
                cAdminView.ShowDialog();
            }
            else
            {
                cLoginMenu cLoginMenu = new();
                cLoginMenu.ShowDialog();
            }
        }
        private void UpdateEmployeeLanguages(string employeeId, string motherLanguage, List<string> otherLanguages)
        {
            isUserChange = false;
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
                conn.Close();
                isUserChange = true;

            }
        }
        private void UpdateEmployeeData(string employeeId)
        {
            isUserChange = false;
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
                        Gender = @Gender,
                        Position = @Position
                        WHERE MitarbeiterID = @EmployeeId";
                    cmd.Parameters.AddWithValue("@Firma", string.IsNullOrWhiteSpace(cbCompany.Text) ? DBNull.Value : cbCompany.Text);
                    cmd.Parameters.AddWithValue("@Vorname", string.IsNullOrWhiteSpace(tbName.Text) ? DBNull.Value : tbName.Text);
                    cmd.Parameters.AddWithValue("@Nachname", string.IsNullOrWhiteSpace(tbSurName.Text) ? DBNull.Value : tbSurName.Text);
                    cmd.Parameters.AddWithValue("@Geburtsdatum", string.IsNullOrWhiteSpace(tbBirthday.Text) ? DBNull.Value : geburtsdatumValue);
                    cmd.Parameters.AddWithValue("@Geburtsland", string.IsNullOrWhiteSpace(tbBirthPlace.Text) ? DBNull.Value : tbBirthPlace.Text);
                    cmd.Parameters.AddWithValue("@Wohnort", string.IsNullOrWhiteSpace(tbLiving.Text) ? DBNull.Value : tbLiving.Text);
                    cmd.Parameters.AddWithValue("@Gender", string.IsNullOrWhiteSpace(cbGender.Text) ? DBNull.Value : cbGender.Text);
                    cmd.Parameters.AddWithValue("@geburtsname", string.IsNullOrWhiteSpace(tbBirthName.Text) ? DBNull.Value : tbBirthName.Text);
                    cmd.Parameters.AddWithValue("@Position", string.IsNullOrWhiteSpace(tbPosition.Text) ? DBNull.Value : tbPosition.Text);
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    cmd.ExecuteNonQuery();
                    string motherLanguage = tbLanguage.Text;
                    List<string> otherLanguages = cbOtherLanguage.Items.Cast<string>().ToList();
                    UpdateEmployeeLanguages(employeeId, motherLanguage, otherLanguages);
                }
                conn.Close();
                isUserChange = true;
            }
        }
        private void FillData()
        {
            isUserChange = false; ;
            if (cbMitarbeiterID.Items.Count > 0)
            {
                // Reset the ComboBox index and reload the data
                cbMitarbeiterID.SelectedIndex = currentIndex;
                selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
                if (selectedWorker != null)
                {
                    string oCurrentID = selectedWorker.ID; // now oEquiptID is the ID string
                    FillEmployeeData(oCurrentID);
                }
                cbMitarbeiterID.SelectedIndex = currentIndex;

            }
            isUserChange = true;
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
                            lbRented.Text = reader["RentState"].ToString() == "true" ? "Ja" : "Nein";
                            lbChip.Text = reader["ChipNummer"].ToString() == "" ? "Keine Gefunden" : reader["ChipNummer"].ToString();
                            cbGender.Text = reader["Gender"].ToString();
                            tbBirthName.Text = reader["Geburtsname"].ToString();
                            tbPosition.Text = reader["Position"].ToString();
                        }
                    }
                }
                string ansprechpartnerPosition = "";
                using (SQLiteCommand cmd = new(
    @"
                        SELECT 
                        Vorgesetzter
                        FROM Position
                        Where Nr = @posID
                        ", conn))
                {
                    cmd.Parameters.AddWithValue("@posID", tbPosition.Text);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ansprechpartnerPosition = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                        }
                    }
                }
                if (ansprechpartnerPosition != string.Empty)
                {
                    using (SQLiteCommand cmd = new(
    @"
                        SELECT 
                        Vorname,Nachname
                        FROM Mitarbeiter
                        Where Position = @posID
                        ", conn))
                    {
                        cmd.Parameters.AddWithValue("@posID", ansprechpartnerPosition);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string ansprechpartnerVorname = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                                string ansprechpartnerNachname = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                                tbContact.Text = string.Concat(ansprechpartnerNachname + ", " + ansprechpartnerVorname);
                            }
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

                conn.Close();

            }

        }
        private bool CheckRights()
        {
            isUserChange = false;
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
                hasRights = false;
            }
            isUserChange = true;
            return hasRights;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            bool hasRight = CheckRights();
            if (hasRight)
            {
                chipCheckTimer.Stop();
                isUserChange = false;
                try
                {
                    selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
                    string currentID = selectedWorker.ID; // now oEquiptID is the ID string
                    currentIndex = cbMitarbeiterID.SelectedIndex;

                    // Ask the user to confirm the deletion
                    DialogResult confirmResult = MessageBox.Show("Sind Sie sicher, dass Sie diesen Mitarbeiter löschen möchten?",
                                                         "Bestätigen Sie die Löschung!",
                                                         MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        orderChanged = true;
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
                            conn.Close();
                        }

                        // Refresh the data in the UI
                        insertDatabaseInComboBox();
                        FillData();
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., show an error message to the user)
                    MessageBox.Show("Fehler beim Löschen des Mitarbeiters: " + ex.Message);
                }
            }
            else { MessageBox.Show("Sie haben keine Rechte hierfür", "error"); return; }
            chipCheckTimer.Start();
            isUserChange = true;
        }
        private void CheckChipIDForCurrentEmployee()
        {
            isUserChange = false;
            string chipId = string.Empty; // Initialisieren Sie chipId außerhalb der If-Else-Anweisung
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
                    restart:
                        // Start der NFC-Reader-Code
                        using (SCardContext context = new())
                        {
                            context.Establish(SCardScope.System);

                            string readerName = context.GetReaders().FirstOrDefault();

                            if (string.IsNullOrEmpty(readerName) || !readerName.Contains("ACR122"))
                            {
                                // Manuelle Eingabe der Chip-ID
                                bool uniqueChipIdFound = false;
                                string chipIdString = string.Empty;
                                int chipIdInt = 0;

                                while (!uniqueChipIdFound)
                                {
                                    chipIdString = Microsoft.VisualBasic.Interaction.InputBox("Bitte geben Sie die Chip-ID manuell ein:", "Manuelle Eingabe", "");

                                    if (!int.TryParse(chipIdString, out chipIdInt))
                                    {
                                        MessageBox.Show("Die eingegebene Chip-ID ist keine gültige Zahl. Bitte geben Sie eine gültige Zahl ein.", "Ungültige Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        continue;
                                    }

                                    chipId = chipIdString; // Weisen Sie chipId hier zu

                                    // Überprüfen Sie, ob die Chip-ID bereits in der Datenbank existiert
                                    using (SQLiteCommand checkCmd = new(conn))
                                    {
                                        checkCmd.CommandText = @"SELECT COUNT(*) FROM Mitarbeiter WHERE ChipNummer = @ChipNummer";
                                        checkCmd.Parameters.AddWithValue("@ChipNummer", chipId);
                                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                                        if (count == 0)
                                        {
                                            uniqueChipIdFound = true;
                                        }
                                        else
                                        {
                                            cLogger.LogDatabaseChange($"Versuch, eine bereits vorhandene Chip-ID {chipId} zu verwenden", username);
                                            MessageBox.Show($"Die Chip-ID {chipId} ist bereits vergeben. Bitte geben Sie eine andere Chip-ID ein.", "Doppelte Chip-ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            continue;
                                        }
                                    }
                                }
                                if (chipIdInt == 0)
                                {
                                    isUserChange = true;
                                    return;
                                }
                                cmd.CommandText = @"UPDATE Mitarbeiter SET ChipNummer = @ChipNummer WHERE MitarbeiterID = @EmployeeId";
                                cmd.Parameters.AddWithValue("@ChipNummer", chipId);
                                cmd.ExecuteNonQuery();
                                conn.Close();
                                // Set the flag to true before updating the data
                                isUpdatingData = true;
                                currentIndex = cbMitarbeiterID.SelectedIndex;
                                insertDatabaseInComboBox();
                                FillData();

                                // Reset the flag to false after updating the data
                                isUpdatingData = false;
                                cLogger.LogDatabaseChange($"Die Chip-ID für {employeeId} wurde erfolgreich gesetzt: {chipId}", username);
                            }
                            else
                            {

                                using (SCardReader reader = new(context))
                                {
                                    // Maximale Anzahl von Verbindungsversuchen
                                    SCardError sc;
                                    bool isWindowOpen = false;
                                    // Erstellen Sie eine neue Form
                                    Form waitingForChipForm = new()
                                    {
                                        Text = "Warte auf Chip...",
                                        Size = new Size(400, 100),
                                        StartPosition = FormStartPosition.CenterScreen
                                    };

                                    // Erstellen Sie ein neues Label-Steuerelement
                                    Label waitingLabel = new()
                                    {
                                        Text = "Bitte legen Sie den Chip auf das Lesegerät...",
                                        AutoSize = true,
                                    };
                                    waitingLabel.Dock = DockStyle.Fill;

                                    // Fügen Sie das Label zur Form hinzu
                                    waitingForChipForm.Controls.Add(waitingLabel);

                                    // Verbindungsversuche in einer Schleife
                                    do
                                    {
                                        // Connect to the card using T1 protocol
                                        sc = reader.Connect(readerName, SCardShareMode.Shared, SCardProtocol.T1);
                                        if (!isWindowOpen)
                                        {
                                            isWindowOpen = true;
                                            // Zeigen Sie die Form in einem separaten Thread an
                                            Thread thread = new(() =>
                                            {
                                                waitingForChipForm.ShowDialog();
                                            });
                                            thread.Start();
                                        }
                                        waitingForChipForm.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

                                        void Form1_FormClosing(object sender, FormClosingEventArgs e)
                                        {
                                            isWindowOpen = false;
                                        }
                                        if (sc == SCardError.Success)
                                        {
                                            waitingForChipForm.Close();
                                        }
                                    }
                                    while (sc != SCardError.Success);

                                    waitingForChipForm.Close();
                                    if (sc == SCardError.Success)
                                    {

                                        byte[] receiveBuffer = new byte[256];
                                        bool uniqueChipIdFound = false;

                                        while (!uniqueChipIdFound)
                                        {
                                            // Transmit the command to the card
                                            sc = reader.Transmit(
                                                SCardPCI.T1, // Protocol Control Information (PCI) for the send protocol
                                                new byte[] { 0xFF, 0xCA, 0x00, 0x00, 0x00 }, // Command APDU
                                                ref receiveBuffer); // Receive buffer

                                            if (sc != SCardError.Success)
                                            {
                                                MessageBox.Show("Fehler beim Auslesen der Chip-ID.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                isUserChange = true;
                                                return;
                                            }
                                            string rawData = BitConverter.ToString(receiveBuffer).Replace("-", "");
                                            rawData = rawData.Substring(0, rawData.Length - 4); // Entfernt die letzten 4 Zeichen (9000)

                                            chipId = rawData; // Weisen Sie chipId hier zu

                                            // Überprüfen Sie, ob die Chip-ID bereits in der Datenbank existiert
                                            using (SQLiteCommand checkCmd = new(conn))
                                            {
                                                checkCmd.CommandText = @"SELECT COUNT(*) FROM Mitarbeiter WHERE ChipNummer = @ChipNummer";
                                                checkCmd.Parameters.AddWithValue("@ChipNummer", chipId);
                                                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                                                if (count == 0)
                                                {
                                                    uniqueChipIdFound = true;
                                                }
                                                else
                                                {
                                                    cLogger.LogDatabaseChange($"Versuch, eine bereits vorhandene Chip-ID {chipId} zu verwenden", username);
                                                    MessageBox.Show($"Die Chip-ID {chipId} ist bereits vergeben. Bitte geben Sie eine andere Chip-ID ein.", "Doppelte Chip-ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                }
                                            }
                                        }
                                        cmd.CommandText = @"UPDATE Mitarbeiter SET ChipNummer = @ChipNummer WHERE MitarbeiterID = @EmployeeId";
                                        cmd.Parameters.AddWithValue("@ChipNummer", chipId);
                                        cmd.ExecuteNonQuery();
                                        conn.Close();
                                        // Set the flag to true before updating the data

                                        isUpdatingData = true;
                                        currentIndex = cbMitarbeiterID.SelectedIndex;
                                        insertDatabaseInComboBox();
                                        FillData();
                                        // Reset the flag to false after updating the data
                                        isUpdatingData = false;
                                        cLogger.LogDatabaseChange($"Die Chip-ID für {employeeId} wurde erfolgreich gesetzt: {chipId}", username);
                                        isUserChange = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show($"Fehler beim Verbinden mit dem NFC-Lesegerät. Fehlercode: {sc}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        goto restart;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        DialogResult res = MessageBox.Show("Der Nutzer hat bereits eine Chip-Nummer, möchten Sie diese Löschen?", "Information", MessageBoxButtons.YesNo);
                        if (res == DialogResult.Yes)
                        {
                            bool hasRights = CheckRights();
                            if (hasRights)
                            {
                                // Der Benutzer hat "Ja" ausgewählt, also löschen Sie die Chip-Nummer.
                                using (SQLiteCommand deleteCmd = new(conn))
                                {
                                    deleteCmd.CommandText = @"UPDATE Mitarbeiter SET ChipNummer = NULL WHERE MitarbeiterID = @EmployeeId";
                                    deleteCmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                                    deleteCmd.ExecuteNonQuery();
                                }



                                isUpdatingData = true;
                                currentIndex = cbMitarbeiterID.SelectedIndex;
                                insertDatabaseInComboBox();
                                FillData();
                                // Reset the flag to false after updating the data
                                isUpdatingData = false;
                                MessageBox.Show("Die Chip-Nummer wurde erfolgreich gelöscht.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                cLogger.LogDatabaseChange($"Die Chip-ID für {employeeId} wurde gelöscht : {chipId}", username);
                                isUserChange = true;
                            }
                            else { MessageBox.Show("Sie haben keine Rechte hierfür", "error"); return; }
                        }
                        else
                        {
                            // Der Benutzer hat "Nein" ausgewählt, also tun Sie nichts.
                        }


                    }
                }
            }
            isUserChange = true;
        }
        private void cPersonalOverview_Shown(object sender, EventArgs e)
        {
        }

        private void cbOtherLanguage_TextChanged(object sender, EventArgs e)
        {
            isUserChange = false;
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
            isUserChange = true;
        }

        private void cbOtherLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            isUserChange = false;
            langIndex = cbOtherLanguage.SelectedIndex;
            isUserChange = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            bool hasRights = CheckRights();
            if (hasRights)
            {
                chipCheckTimer.Stop();
                isUserChange = false;
                if (cbMitarbeiterID.SelectedItem is cWorker selectedWorker)
                {
                    string oCurrentID = selectedWorker.ID;
                    currentIndex = cbMitarbeiterID.SelectedIndex;

                    DialogResult confirmResult = MessageBox.Show("Möchten Sie die Änderungen speichern?",
                                                        "Bestätigen Sie das Speichern!",
                                                        MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        if (deleteLanguage)
                        {
                            if (cbOtherLanguage.Items.Count >= 0)
                            {
                                cLogger.LogDatabaseChange($"Sprache {cbOtherLanguage.Items[langIndex].ToString()} vom nutzer {cbMitarbeiterID.SelectedItem.ToString()} wurde gelöscht", username);
                                cbOtherLanguage.Items.RemoveAt(langIndex);
                            }
                            deleteLanguage = false;
                        }
                        string selectedLanguage = cbOtherLanguage.Text;
                        if (!string.IsNullOrEmpty(selectedLanguage))
                        {
                            if (!cbOtherLanguage.Items.Contains(selectedLanguage))
                            {
                                cLogger.LogDatabaseChange($"Sprache {selectedLanguage} wurde zum nutzer {cbMitarbeiterID.SelectedItem.ToString()} hinzugefügt", username);
                                cbOtherLanguage.Items.Add(selectedLanguage);
                            }
                        }

                        try
                        {
                            UpdateEmployeeData(oCurrentID);
                            cLogger.LogDatabaseChange($"Aktualisierung der Mitarbeiterdaten für ID: {oCurrentID} erfolgreich", username);
                            insertDatabaseInComboBox();
                            if (currentIndex < cbMitarbeiterID.Items.Count)
                            {
                                cbMitarbeiterID.SelectedIndex = currentIndex;
                                FillData();
                            }
                        }
                        catch (Exception ex)
                        {
                            cLogger.LogDatabaseChange($"Fehler beim Aktualisieren der Mitarbeiterdaten für ID: {oCurrentID}. Fehler: {ex.Message}", username);
                            if (currentIndex < cbMitarbeiterID.Items.Count)
                            {
                                cbMitarbeiterID.SelectedIndex = currentIndex;
                                if (cbMitarbeiterID.SelectedItem is cWorker retrySelectedWorker)
                                {
                                    oCurrentID = retrySelectedWorker.ID;
                                    UpdateEmployeeData(oCurrentID);
                                    cLogger.LogDatabaseChange($"Erneuter Versuch, Mitarbeiterdaten für ID: {oCurrentID} zu aktualisieren", username);
                                    insertDatabaseInComboBox();
                                    cbMitarbeiterID.SelectedIndex = currentIndex;
                                    FillData();
                                }
                            }
                        }
                        cbMitarbeiterID.SelectedIndex = currentIndex;
                        cbMitarbeiterID.SelectedIndex = currentIndex;
                        cLogger.LogDatabaseChange($"Änderungen an {cbMitarbeiterID.SelectedItem.ToString()} gespeichert", username);
                        MessageBox.Show($"Änderungen an '{cbMitarbeiterID.SelectedItem.ToString()}' erfolgreisch gespeichert!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Keinen Mitarbeiter ausgewählt.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else { MessageBox.Show("Sie haben keine Rechte hierfür", "error"); return; }
            chipCheckTimer.Start();
            isUserChange = true;
        }
        private void bCheckIn_Click(object sender, EventArgs e)
        {
            chipCheckTimer.Stop();
            isUserChange = false;
            cbMitarbeiterID.SelectedIndex = currentIndex;
            cWorker selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
            string oCurrentID = selectedWorker.ID;
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new("SELECT Count(*) FROM Mitarbeiter WHERE MitarbeiterID = @employeeID AND ChipNummer IS NOT NULL", conn))
                {
                    cmd.Parameters.AddWithValue("@employeeID", oCurrentID);
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    if (rowCount > 0)
                    {
                    }
                    else
                    {
                        DialogResult res = MessageBox.Show("Das Personal hat noch keine ChipNummer zugeordnet bekommen, wollen Sie das noch machen?", "information", MessageBoxButtons.YesNo);
                        if (res == DialogResult.Yes)
                        {
                            CheckChipIDForCurrentEmployee();
                        }

                    }

                }
                using (SQLiteCommand cmd = new("SELECT Count(*) FROM Mitarbeiter WHERE MitarbeiterID = @employeeID AND CheckInState IS 'false'", conn))
                {
                    cmd.Parameters.AddWithValue("@employeeID", oCurrentID);
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    if (rowCount > 0)
                    {
                    }
                    else
                    {
                        MessageBox.Show("Bitte checken sie den Mitarbeiter zuerst aus");
                        isUserChange = true;
                        chipCheckTimer.Start();
                        return;
                    }

                }
                conn.Close();
            }
            object checkInState;
            if (!selectedWorker.CheckInState)
            {
                bool getRented = false;
                DateTime checkInTime = DateTime.Now;
                // Erstellen Sie eine TimeSpan für 18:00 Uhr
                TimeSpan nigthCheck = new(10, 0, 0);
                DateTime scheduledCheckInTime;

                using (SQLiteConnection conn = new(stConnectionString))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = new("SELECT CheckedInSoll FROM ArbeitszeitenSoll WHERE MitarbeiterID = @EmployeeId AND Nacht IS @nacht", conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", oCurrentID);
                        cmd.Parameters.AddWithValue("@nacht", checkInTime.TimeOfDay < nigthCheck ? "false" : "true");
                        checkInState = cmd.ExecuteScalar();
                        if (checkInState != null && checkInState != DBNull.Value)
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
                        string pos = "";
                        using (SQLiteCommand cmd = new(conn))
                        {
                            cmd.CommandText = @"
                                                SELECT Position
                                                FROM ArbeitszeitenSoll
                                                WHERE MitarbeiterID = @id AND Nacht IS @nacht";
                            cmd.Parameters.AddWithValue("id", oCurrentID);
                            cmd.Parameters.AddWithValue("@nacht", checkInTime.TimeOfDay < nigthCheck ? "false" : "true");

                            pos = Convert.ToString(cmd.ExecuteScalar());

                        }
                        using (SQLiteCommand cmd = new(conn))
                        {
                            cmd.CommandText = @"
                                                UPDATE Mitarbeiter
                                                SET CheckInState = @state,
                                                Position = @pos
                                                WHERE MitarbeiterID =@id ";
                            cmd.Parameters.AddWithValue("@state", "true");
                            cmd.Parameters.AddWithValue("id", oCurrentID);
                            cmd.Parameters.AddWithValue("@pos", pos);

                            cmd.ExecuteNonQuery();

                        }
                        conn.Close();

                        // Set the flag to true before updating the data
                        isUpdatingData = true;
                        currentIndex = cbMitarbeiterID.SelectedIndex;
                        insertDatabaseInComboBox();
                        FillData();

                        // Reset the flag to false after updating the data
                        isUpdatingData = false;

                    }
                    MessageBox.Show($"Eingechecked mit der Zeit {checkInTime}", "Bestätigung", MessageBoxButtons.OK);
                    checkInTimeForm.Close();
                    bFRent_Click(sender, e);
                };
                checkInTimeForm.AcceptButton = confirmButton;
                checkInTimeForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Bitte trage zuerst den Aus-Zeitstempel ein", "Falsche Nutzung", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            chipCheckTimer.Start();
            isUserChange = true;
        }

        private void bCheckOut_Click(object sender, EventArgs e)
        {
            chipCheckTimer.Stop();
            isUserChange = false;
            cbMitarbeiterID.SelectedIndex = currentIndex;
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
                    using (SQLiteCommand cmd = new("SELECT COUNT(*) FROM Mitarbeiter WHERE MitarbeiterID = @EmployeeId AND CheckInState = 'true'", conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", oCurrentID);

                        int rowCount = Convert.ToInt32(cmd.ExecuteScalar());

                        if (!(rowCount > 0))
                        {
                            MessageBox.Show("Bitte checke zuerst ein!", "Falsche Nutzung", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            isUserChange = true;
                            chipCheckTimer.Start();
                            return;
                        }
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
                    }
                    // Set the flag to true before updating the data
                    isUpdatingData = true;
                    currentIndex = cbMitarbeiterID.SelectedIndex;
                    insertDatabaseInComboBox();
                    FillData();

                    // Reset the flag to false after updating the data
                    isUpdatingData = false;
                    MessageBox.Show($"Ausgechecked mit der Zeit {checkOutTime}", "Bestätigung", MessageBoxButtons.OK);

                    checkOutTimeForm.Close();
                };
                checkOutTimeForm.AcceptButton = confirmButton;
                checkOutTimeForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Bitte trage zuerst den Aus-Zeitstempel ein", "Falsche Nutzung", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            chipCheckTimer.Start();
            isUserChange = true;
        }
        private void bRent_Click(object sender, EventArgs e)
        {
            chipCheckTimer.Stop();
            isUserChange = false; ;
            cbMitarbeiterID.SelectedIndex = currentIndex;
            cWorker selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
            string oCurrentID = selectedWorker.ID;
            Form prompt = new();
            prompt.Width = 500;
            prompt.Height = 500; // Adjusted to accommodate labels
            prompt.Text = "Wählen Sie ein Ausrüstungsteil aus und geben Sie die Mitarbeiter-ID ein";
            prompt.StartPosition = FormStartPosition.CenterScreen;
            RadioButton colorRedRadioButton = new() { Dock = DockStyle.Top, Text = "rot" };
            RadioButton colorBlackRadioButton = new() { Dock = DockStyle.Top, Text = "schwarz" };
            RadioButton colorBlueRadioButton = new() { Dock = DockStyle.Top, Text = "blau" };
            Label colorLabel = new() { Text = "Farbe", Dock = DockStyle.Top };
            RadioButton conditionDamagedRadioButton = new() { Dock = DockStyle.Top, Text = "beschädigt" };
            RadioButton conditionUsedRadioButton = new() { Dock = DockStyle.Top, Text = "gebraucht" };
            RadioButton conditionGoodRadioButton = new() { Dock = DockStyle.Top, Text = "gut" };
            Label conditionLabel = new() { Text = "Zustand", Dock = DockStyle.Top };
            TextBox equipmentTypeOtherBox = new() { Dock = DockStyle.Top, Visible = false };
            ComboBox equipmentTypeComboBox = new() { Dock = DockStyle.Top, Items = { "weste", "polo", "jacke", "windbreaker", "sonstiges" } };
            equipmentTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            equipmentTypeComboBox.SelectedIndex = 0;
            Label equipmentTypeLabel = new() { Text = "Art", Dock = DockStyle.Top };
            TextBox equipmentNumberBox = new() { Dock = DockStyle.Top };
            Label equipmentNumberLabel = new() { Text = "Nummer", Dock = DockStyle.Top };
            Label equipmentLabel = new() { Text = "Ausrüstungsteil", Dock = DockStyle.Top };

            Button confirmation = new() { Text = "Ok", Dock = DockStyle.Bottom, Height = 50 };
            TextBox colorOtherBox = new() { Dock = DockStyle.Top, Visible = false }; // Make this box invisible by default

            // Create a panel for color radio buttons
            Panel colorPanel = new() { Dock = DockStyle.Top, Height = 150 };
            RadioButton colorOtherRadioButton = new() { Dock = DockStyle.Top, Text = "sonstiges" }; // Radio button for other colors
            bool swapColor = false;
            // Add the panels to the form

            // Add the event to change the visibility of equipmentTypeOtherBox
            equipmentTypeComboBox.SelectedIndexChanged += (sender, e) =>
            {
                if (equipmentTypeComboBox.SelectedItem.ToString() == "sonstiges")
                {
                    equipmentTypeOtherBox.Visible = true;
                }
                else
                {
                    equipmentTypeOtherBox.Visible = false;
                }
            };

            colorOtherRadioButton.CheckedChanged += (sender, e) =>
            {
                swapColor = !swapColor;
                colorOtherBox.Visible = swapColor;
            };


            // Add radio buttons to the color panel
            colorPanel.Controls.AddRange(new Control[] { colorOtherBox, colorOtherRadioButton, colorRedRadioButton, colorBlackRadioButton, colorBlueRadioButton, colorLabel });

            // Create a panel for condition radio buttons
            Panel conditionPanel = new() { Dock = DockStyle.Top, Height = 100 };

            // Add radio buttons to the condition panel
            conditionPanel.Controls.AddRange(new Control[] { conditionDamagedRadioButton, conditionUsedRadioButton, conditionGoodRadioButton, conditionLabel });


            prompt.Controls.Add(confirmation);
            prompt.Controls.AddRange(new Control[] { colorPanel, conditionPanel });
            prompt.Controls.Add(equipmentTypeOtherBox);
            prompt.Controls.Add(equipmentTypeComboBox);
            prompt.Controls.Add(equipmentTypeLabel);
            prompt.Controls.Add(equipmentNumberBox);
            prompt.Controls.Add(equipmentNumberLabel);
            prompt.Controls.Add(equipmentLabel);

            confirmation.Click += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(equipmentNumberBox.Text)
                    && (conditionGoodRadioButton.Checked || conditionUsedRadioButton.Checked || conditionDamagedRadioButton.Checked)
                    && (colorBlueRadioButton.Checked || colorBlackRadioButton.Checked || colorRedRadioButton.Checked || !string.IsNullOrEmpty(colorOtherBox.Text))
                    && (equipmentTypeComboBox.SelectedItem != null || !string.IsNullOrEmpty(equipmentTypeOtherBox.Text)))
                {
                    string equipmentNumber = equipmentNumberBox.Text.ToLower();
                    string equipmentType = equipmentTypeComboBox.SelectedItem.ToString() == "sonstiges" ? equipmentTypeOtherBox.Text.ToLower() : equipmentTypeComboBox.SelectedItem.ToString().ToLower();
                    string condition = conditionGoodRadioButton.Checked ? "gut" : conditionUsedRadioButton.Checked ? "gebraucht" : "beschädigt";
                    string color = colorBlueRadioButton.Checked ? "blau" : colorBlackRadioButton.Checked ? "schwarz" : colorRedRadioButton.Checked ? "rot" : colorOtherBox.Text.ToLower();

                    cLogger.LogDatabaseChange($"Verleihe Equipment {equipmentNumber} an {oCurrentID}", username);

                    using (SQLiteConnection conn = new(stConnectionString))
                    {
                        conn.Open();

                        // Check if the equipment already exists
                        using (SQLiteCommand cmdCheck = new("SELECT COUNT(*) FROM Ausruestung WHERE ID = @id AND Art = @type AND Farbe = @color", conn))
                        {
                            cmdCheck.Parameters.AddWithValue("@id", equipmentNumber);
                            cmdCheck.Parameters.AddWithValue("@type", equipmentType);
                            cmdCheck.Parameters.AddWithValue("@color", color);

                            int count = Convert.ToInt32(cmdCheck.ExecuteScalar());

                            if (count == 0)  // If equipment does not exist, insert it
                            {
                                using (SQLiteCommand cmdInsert = new(conn))
                                {
                                    cmdInsert.CommandText = @"
                        INSERT INTO Ausruestung
                        (ID, Art, Farbe, MitarbeiterID, Zustand)
                        VALUES
                        (@id,@type,@color,@mitarbeiterID,@condition)";
                                    cmdInsert.Parameters.AddWithValue("@id", equipmentNumber);
                                    cmdInsert.Parameters.AddWithValue("@type", equipmentType);
                                    cmdInsert.Parameters.AddWithValue("@color", color);
                                    cmdInsert.Parameters.AddWithValue("@mitarbeiterID", oCurrentID);
                                    cmdInsert.Parameters.AddWithValue("@condition", condition);

                                    cmdInsert.ExecuteNonQuery();
                                }
                            }
                            else  // If equipment exists, update it
                            {
                                using (SQLiteCommand cmdUpdate = new(conn))
                                {
                                    cmdUpdate.CommandText = @"
                        UPDATE Ausruestung
                        SET 
                        MitarbeiterID = @mitarbeiterID,
                        Zustand = @condition
                        WHERE ID = @id AND Art = @type AND Farbe = @color";
                                    cmdUpdate.Parameters.AddWithValue("@mitarbeiterID", oCurrentID);
                                    cmdUpdate.Parameters.AddWithValue("@condition", condition);
                                    cmdUpdate.Parameters.AddWithValue("@id", equipmentNumber);
                                    cmdUpdate.Parameters.AddWithValue("@type", equipmentType);
                                    cmdUpdate.Parameters.AddWithValue("@color", color);
                                    cmdUpdate.ExecuteNonQuery();
                                }
                            }
                        }

                        // Always update RentState in Mitarbeiter table
                        using (SQLiteCommand cmd = new(conn))
                        {
                            cmd.CommandText = @"
                UPDATE Mitarbeiter
                SET 
                RentState = @rentstate
                WHERE MitarbeiterID = @mitarbeiterID";
                            cmd.Parameters.AddWithValue("@rentstate", "true");
                            cmd.Parameters.AddWithValue("@mitarbeiterID", oCurrentID);

                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                        cLogger.LogDatabaseChange($"Verleihe Equipment {equipmentNumber} an {oCurrentID}", username);
                        MessageBox.Show($"Equipment: {equipmentType} mit der farbe {color} und der nummer {equipmentNumber} wurde an {cbMitarbeiterID.SelectedItem.ToString()} verliehen", "Erfolg", MessageBoxButtons.OK);
                        // Set the flag to true before updating the data
                        isUpdatingData = true;
                        currentIndex = cbMitarbeiterID.SelectedIndex;
                        insertDatabaseInComboBox();
                        FillData();

                        // Reset the flag to false after updating the data
                        isUpdatingData = false;
                    }
                    prompt.Close();
                }
                else
                {
                    MessageBox.Show("Bitte füllen Sie alle erforderlichen Felder aus",
                                    "Erforderliche Informationen fehlen",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                }
            };
            prompt.ShowDialog();
            chipCheckTimer.Start();
            isUserChange = true;
        }

        private void bReturn_Click(object sender, EventArgs e)
        {
            chipCheckTimer.Stop();
            isUserChange = false;
            cbMitarbeiterID.SelectedIndex = currentIndex;
            cWorker selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
            string employeeID = selectedWorker.ID;
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new("SELECT COUNT(*) FROM Ausruestung WHERE MitarbeiterID = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", employeeID);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        MessageBox.Show("Es gibt derzeit keine ausgeliehene Ausrüstung.",
                                        "Keine ausgeliehene Ausrüstung",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        isUserChange = true;
                        chipCheckTimer.Start();
                        return;
                    }
                }
                conn.Close();
            }

            Form prompt = new();
            prompt.Width = 300;
            prompt.Height = 200;
            prompt.Text = "Wählen Sie ein Ausrüstungsteil aus.";
            prompt.StartPosition = FormStartPosition.CenterScreen;

            TextBox equipmentBox = new() { Dock = DockStyle.Top };
            ListBox equipmentListBox = new() { Dock = DockStyle.Top };

            Button confirmation = new() { Text = "Ok", Dock = DockStyle.Bottom };
            confirmation.Width = 100; // Set the width
            confirmation.Height = 30; // Set the height
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

            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new("SELECT ID,Art,Farbe FROM Ausruestung WHERE MitarbeiterID NOT NULL", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cEquipment equipmentItem = new()
                            {
                                ID = reader.GetString(0),
                                Name = reader.GetString(1),
                                Color = reader.GetString(2),
                            };
                            allEquipment.Add(equipmentItem);
                            equipmentListBox.Items.Add(equipmentItem);
                        }
                    }
                }
                conn.Close();
            }

            equipmentBox.TextChanged += (sender, e) =>
            {
                string[] searchTerms = equipmentBox.Text.ToLower().Split(',');

                IEnumerable<cEquipment> matches = allEquipment.Where(item =>
                    searchTerms.All(term =>
                        (item.ID != null && item.ID.Contains(term.Trim())) ||
                        (item.Name != null && item.Name.ToLower().Contains(term.Trim())) ||
                        (item.Color != null && item.Color.ToLower().Contains(term.Trim())) ||
                        (item.Position != null && item.Position.ToLower().Contains(term.Trim()))
                    )
                );

                // Clear the ListBox and add the matching items
                equipmentListBox.Items.Clear();
                foreach (cEquipment match in matches)
                {
                    equipmentListBox.Items.Add(match);
                }

                // If there is only one matching item, select it
                if (equipmentListBox.Items.Count == 1)
                {
                    equipmentListBox.SelectedIndex = 0;
                }
            };

            prompt.Controls.Add(equipmentBox);
            prompt.Controls.Add(equipmentListBox);
            prompt.Controls.Add(confirmation);
            prompt.ShowDialog();

            if (equipmentListBox.SelectedItem != null)
            {
                cEquipment selectedEquipment = equipmentListBox.SelectedItem as cEquipment;
                string oCurrentID = selectedEquipment.ID;

                cLogger.LogDatabaseChange($"Rückgabe Equipment {oCurrentID} von {employeeID}", username);
                using (SQLiteConnection conn = new(stConnectionString))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = new(conn))
                    {
                        cmd.CommandText = "UPDATE Ausruestung" +
                                          "SET MitarbeiterID = @mitarbeiterID" +
                                          "WHERE ID = @id";
                        cmd.Parameters.AddWithValue("@id", oCurrentID);
                        cmd.Parameters.AddWithValue("@status", "Ausleihbar");
                        cmd.Parameters.AddWithValue("@mitarbeiterID", null);

                        cmd.ExecuteNonQuery();
                    }
                    using (SQLiteCommand cmd = new(conn))
                    {
                        cmd.CommandText = @"
                        UPDATE Mitarbeiter
                        SET RentState = @state
                        WHERE MitarbeiterID = @id";
                        cmd.Parameters.AddWithValue("@id", employeeID);
                        cmd.Parameters.AddWithValue("@state", "false");

                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                    cLogger.LogDatabaseChange($"Verleihe Equipment {oCurrentID} an {employeeID}", username);
                    MessageBox.Show($"Equipment: {selectedEquipment.Name} mit der farbe {selectedEquipment.Color} und der nummer {selectedEquipment.ID} wurde von {cbMitarbeiterID.SelectedItem.ToString()} zurück gegeben.", "Erfolg", MessageBoxButtons.OK);
                    // Set the flag to true before updating the data
                    isUpdatingData = true;
                    currentIndex = cbMitarbeiterID.SelectedIndex;
                    insertDatabaseInComboBox();
                    FillData();

                    // Reset the flag to false after updating the data
                    isUpdatingData = false;
                }
            }
            chipCheckTimer.Start();
            isUserChange = true;

        }

        private void bPrintReceipt_Click(object sender, EventArgs e)
        {
            chipCheckTimer.Stop();
            isUserChange = false;
            cbMitarbeiterID.SelectedIndex = currentIndex;
            cViewManager viewManager = new();
            cWorker selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
            viewManager.printReceipt(sender, e, username, selectedWorker.ID);
            isUserChange = true;
            chipCheckTimer.Start();
        }

        private void bFRent_Click(object sender, EventArgs e)
        {
            chipCheckTimer.Stop();
            isUserChange = false; ;
            cbMitarbeiterID.SelectedIndex = currentIndex;
            cWorker selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
            string oCurrentID = selectedWorker.ID;
            Form prompt = new();
            prompt.Width = 500;
            prompt.Height = 500; // Adjusted to accommodate labels
            prompt.Text = "Wählen Sie ein Ausrüstungsteil aus und geben Sie die Mitarbeiter-ID ein";
            prompt.StartPosition = FormStartPosition.CenterScreen;

            TextBox idBox = new() { Dock = DockStyle.Top };
            Label idLabel = new() { Text = "ID", Dock = DockStyle.Top };
            CheckBox bleibtCheckBox = new() { Text = "Bleibt", Dock = DockStyle.Top };
            NumericUpDown akkuNumericUpDown = new() { Dock = DockStyle.Top };
            Label akkuLabel = new() { Text = "Akku", Dock = DockStyle.Top };
            CheckBox funkgeraetCheckBox = new() { Text = "Funkgerät", Dock = DockStyle.Top };
            CheckBox tarnHeadsetCheckBox = new() { Text = "Tarn Headset", Dock = DockStyle.Top };
            CheckBox rasiererCheckBox = new() { Text = "Rasierer", Dock = DockStyle.Top };
            CheckBox mikimausCheckBox = new() { Text = "Mikiemaus", Dock = DockStyle.Top };
            TextBox verbrauchsmaterialBox = new() { Dock = DockStyle.Top };
            Label verbrauchsmaterialLabel = new() { Text = "Verbrauchsmaterial", Dock = DockStyle.Top };
            TextBox sonstigesBox = new() { Dock = DockStyle.Top };
            Label sonstigesLabel = new() { Text = "Sonstiges", Dock = DockStyle.Top };
            Button confirmation = new() { Text = "Ok", Dock = DockStyle.Bottom, Height = 50 };

            Panel radioPanel = new() { Dock = DockStyle.Top, Height = 150 };
            radioPanel.Controls.AddRange(new Control[] { mikimausCheckBox, rasiererCheckBox, tarnHeadsetCheckBox, funkgeraetCheckBox });
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();

                // Always update RentState in Mitarbeiter table
                using (SQLiteCommand cmd = new(conn))
                {
                    cmd.CommandText = @"
                Select Position
                FROM Mitarbeiter
                WHERE MitarbeiterID = @mitarbeiterID";
                    cmd.Parameters.AddWithValue("@mitarbeiterID", oCurrentID);

                    string pos = cmd.ExecuteScalar().ToString();
                    idBox.Text = pos;
                }
                conn.Close();
            }
            confirmation.Click += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(idBox.Text))
            {
                string id = idBox.Text;
                string bleibt = bleibtCheckBox.Checked ? "true" : "false";
                int akku = (int)akkuNumericUpDown.Value;
                string funkgeraet = funkgeraetCheckBox.Checked ? "true" : "false";
                string tarnHeadset = tarnHeadsetCheckBox.Checked ? "true" : "false";
                string rasierer = rasiererCheckBox.Checked ? "true" : "false";
                string mikimaus = mikimausCheckBox.Checked ? "true" : "false";
                string verbrauchsmaterial = verbrauchsmaterialBox.Text;
                string sonstiges = sonstigesBox.Text;

                cLogger.LogDatabaseChange($"Verleihe Funkgeraet {id} an {oCurrentID}", username);

                using (SQLiteConnection conn = new(stConnectionString))
                {
                    conn.Open();


                    // Check if the Funkgeraet already exists
                    using (SQLiteCommand cmdCheck = new("SELECT COUNT(*) FROM Funkgeraete WHERE ID = @id", conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@id", id);

                        int count = Convert.ToInt32(cmdCheck.ExecuteScalar());

                        if (count == 0)  // If Funkgeraet does not exist, insert it
                        {
                            using (SQLiteCommand cmdInsert = new(conn))
                            {
                                cmdInsert.CommandText = @"
                        INSERT INTO Funkgeraete
                        (ID, Bleibt, Akku,
                        Funkgeraet, Tarn_Headset, Rasierer, Mikimaus,
                        MitarbeiterID, Verbrauchsmaterial, Sonstiges)
                        VALUES
                        (@id, @bleibt, @akku,
                        @funkgeraet, @tarnHeadset, @rasierer, @mikimaus,
                        @mitarbeiterID, @verbrauchsmaterial, @sonstiges)";
                                cmdInsert.Parameters.AddWithValue("@id", id);
                                cmdInsert.Parameters.AddWithValue("@bleibt", bleibt);
                                cmdInsert.Parameters.AddWithValue("@akku", akku);
                                cmdInsert.Parameters.AddWithValue("@funkgeraet", funkgeraet);
                                cmdInsert.Parameters.AddWithValue("@tarnHeadset", tarnHeadset);
                                cmdInsert.Parameters.AddWithValue("@rasierer", rasierer);
                                cmdInsert.Parameters.AddWithValue("@mikimaus", mikimaus);
                                cmdInsert.Parameters.AddWithValue("@mitarbeiterID", oCurrentID);
                                cmdInsert.Parameters.AddWithValue("@verbrauchsmaterial", verbrauchsmaterial);
                                cmdInsert.Parameters.AddWithValue("@sonstiges", sonstiges);

                                cmdInsert.ExecuteNonQuery();
                            }
                        }
                        else  // If Funkgeraet exists, update it
                        {
                            using (SQLiteCommand cmdUpdate = new(conn))
                            {
                                cmdUpdate.CommandText = @"
                        UPDATE Funkgeraete
                        SET 
                        Bleibt = @bleibt,
                        Akku = @akku,
                        Funkgeraet = @funkgeraet,
                        Tarn_Headset = @tarnHeadset,
                        Rasierer = @rasierer,
                        Mikimaus = @mikimaus,
                        MitarbeiterID = @mitarbeiterID,
                        Verbrauchsmaterial = @verbrauchsmaterial,
                        Sonstiges = @sonstiges
                        WHERE ID = @id";
                                cmdUpdate.Parameters.AddWithValue("@bleibt", bleibt);
                                cmdUpdate.Parameters.AddWithValue("@akku", akku);
                                cmdUpdate.Parameters.AddWithValue("@funkgeraet", funkgeraet);
                                cmdUpdate.Parameters.AddWithValue("@tarnHeadset", tarnHeadset);
                                cmdUpdate.Parameters.AddWithValue("@rasierer", rasierer);
                                cmdUpdate.Parameters.AddWithValue("@mikimaus", mikimaus);
                                cmdUpdate.Parameters.AddWithValue("@mitarbeiterID", oCurrentID);
                                cmdUpdate.Parameters.AddWithValue("@verbrauchsmaterial", verbrauchsmaterial);
                                cmdUpdate.Parameters.AddWithValue("@sonstiges", sonstiges);
                                cmdUpdate.Parameters.AddWithValue("@id", id);

                                cmdUpdate.ExecuteNonQuery();
                            }
                        }
                    }

                    // Always update RentState in Mitarbeiter table
                    using (SQLiteCommand cmd = new(conn))
                    {
                        cmd.CommandText = @"
                UPDATE Mitarbeiter
                SET 
                RentState = @rentstate
                WHERE MitarbeiterID = @mitarbeiterID";
                        cmd.Parameters.AddWithValue("@rentstate", "true");
                        cmd.Parameters.AddWithValue("@mitarbeiterID", oCurrentID);

                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                    List<string> items = new();
                    if (funkgeraet == "true")
                    {
                        items.Add("Funkgerät");
                    }

                    if (tarnHeadset == "true")
                    {
                        items.Add("Tarn Headset");
                    }

                    if (rasierer == "true")
                    {
                        items.Add("Rasierer");
                    }

                    if (mikimaus == "true")
                    {
                        items.Add("Mikiemaus");
                    }

                    string itemsString = string.Join(", ", items);
                    string bleibtString = bleibt == "true" ? "bleibt" : "bleibt nicht";

                    cLogger.LogDatabaseChange($"{itemsString} mit der {id} und {akku} Batterien erfolgreich an {oCurrentID} verliehen, das Gerät {bleibtString} an der Position", username);
                    MessageBox.Show($"{itemsString} mit der {id} und {akku} Batterien erfolgreich an {oCurrentID} verliehen, das Gerät {bleibtString} an der Position", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                idBox.Text = "";
                bleibtCheckBox.CheckState = CheckState.Unchecked;
                verbrauchsmaterialBox.Text = "";
                sonstigesBox.Text = "";
                akku = 0;
            }
            else
            {
                MessageBox.Show("Bitte geben Sie die ID des Funkgeraets ein",
                                "Erforderliche Informationen fehlen",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
            prompt.Close();
        };
            prompt.Controls.Add(sonstigesBox);
            prompt.Controls.Add(sonstigesLabel);
            prompt.Controls.Add(verbrauchsmaterialBox);
            prompt.Controls.Add(verbrauchsmaterialLabel);
            prompt.Controls.Add(radioPanel);
            prompt.Controls.Add(akkuNumericUpDown);
            prompt.Controls.Add(akkuLabel);
            prompt.Controls.Add(bleibtCheckBox);
            prompt.Controls.Add(idBox);
            prompt.Controls.Add(idLabel);
            prompt.Controls.Add(confirmation);
            prompt.ShowDialog();

            chipCheckTimer.Start();
            isUserChange = true;
            bRent_Click(sender, e);
        }

        private void bFReturn_Click(object sender, EventArgs e)
        {
            chipCheckTimer.Stop();
            isUserChange = false;
            cbMitarbeiterID.SelectedIndex = currentIndex;
            cWorker selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;
            string employeeID = selectedWorker.ID;
            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new("SELECT COUNT(*) FROM Funkgeraete WHERE MitarbeiterID = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", employeeID);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        MessageBox.Show("Es gibt derzeit keine ausgeliehenen Funkgeräte.",
                                        "Keine ausgeliehenen Funkgeräte",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        isUserChange = true;
                        chipCheckTimer.Start();
                        return;
                    }
                }
                conn.Close();
            }

            Form prompt = new();
            prompt.Width = 300;
            prompt.Height = 200;
            prompt.Text = "Wählen Sie ein Funkgerät aus.";
            prompt.StartPosition = FormStartPosition.CenterScreen;

            TextBox equipmentBox = new() { Dock = DockStyle.Top };
            ListBox equipmentListBox = new() { Dock = DockStyle.Top };

            Button confirmation = new() { Text = "Ok", Dock = DockStyle.Bottom, Height = 50 };
            confirmation.Click += (sender, e) =>
            {
                if (equipmentListBox.SelectedItem != null)
                {
                    prompt.Close();
                }
                else
                {
                    MessageBox.Show("Bitte wählen Sie ein Funkgerät aus",
                                    "Erforderliche Informationen fehlen",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                }
            };

            List<cRadioEquipment> allEquipment = new();

            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new("SELECT ID, Bleibt, Akku, Funkgeraet, Tarn_Headset, Rasierer, Mikimaus, Verbrauchsmaterial, Sonstiges FROM Funkgeraete WHERE MitarbeiterID = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", employeeID);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cRadioEquipment equipmentItem = new()
                            {
                                ID = reader.GetString(0),
                                Permanent = reader.GetString(1),
                                Battery = reader.GetInt32(2),
                                Radio = reader.GetString(3),
                                CamouflageHeadset = reader.GetString(4),
                                Razor = reader.GetString(5),
                                MickeyMouse = reader.GetString(6),
                                Consumables = reader.GetString(7),
                                Others = reader.GetString(8),
                            };
                            allEquipment.Add(equipmentItem);
                            equipmentListBox.Items.Add(equipmentItem);
                        }
                    }
                }
                conn.Close();
            }

            equipmentBox.TextChanged += (sender, e) =>
            {
                string[] searchTerms = equipmentBox.Text.ToLower().Split(',');

                IEnumerable<cRadioEquipment> matches = allEquipment.Where(item =>
                    searchTerms.All(term =>
                        (item.ID != null && item.ID.Contains(term.Trim())) ||
                        (item.Radio != null && item.Radio.ToLower().Contains(term.Trim())) ||
                        (item.Consumables != null && item.Consumables.ToLower().Contains(term.Trim())) ||
                        (item.Others != null && item.Others.ToLower().Contains(term.Trim()))
                    )
                );

                equipmentListBox.Items.Clear();
                foreach (cRadioEquipment match in matches)
                {
                    equipmentListBox.Items.Add(match);
                }

                if (equipmentListBox.Items.Count == 1)
                {
                    equipmentListBox.SelectedIndex = 0;
                }
            };

            prompt.Controls.Add(equipmentBox);
            prompt.Controls.Add(equipmentListBox);
            prompt.Controls.Add(confirmation);
            prompt.ShowDialog();

            if (equipmentListBox.SelectedItem != null)
            {
                cRadioEquipment selectedEquipment = equipmentListBox.SelectedItem as cRadioEquipment;
                string oCurrentID = selectedEquipment.ID;

                cLogger.LogDatabaseChange($"Rückgabe Funkgerät {oCurrentID} von {employeeID}", username);
                using (SQLiteConnection conn = new(stConnectionString))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = new(conn))
                    {
                        cmd.CommandText = @"
                UPDATE Funkgeraete
                SET MitarbeiterID = null
                WHERE ID = @id AND MitarbeiterID = @mitarbeiterID";
                        cmd.Parameters.AddWithValue("@id", oCurrentID);
                        cmd.Parameters.AddWithValue("@mitarbeiterID", employeeID);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            MessageBox.Show($"Es konnte kein Funkgerät mit der ID {oCurrentID}, das an den Mitarbeiter {employeeID} verliehen wurde, gefunden werden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            isUserChange = true;
                            chipCheckTimer.Start();
                            return;
                        }
                    }

                    using (SQLiteCommand cmd = new(conn))
                    {
                        cmd.CommandText = @"
                UPDATE Mitarbeiter
                SET RentState = 'false'
                WHERE MitarbeiterID = @id";
                        cmd.Parameters.AddWithValue("@id", employeeID);

                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();

                    MessageBox.Show($"Funkgerät {oCurrentID} erfolgreich zurückgegeben von {employeeID}.", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    isUpdatingData = true;
                    currentIndex = cbMitarbeiterID.SelectedIndex;
                    insertDatabaseInComboBox();
                    FillData();

                    isUpdatingData = false;
                }
            }
            chipCheckTimer.Start();
            isUserChange = true;
        }

        private void bRead_Click(object sender, EventArgs e)
        {
            chipCheckTimer.Stop();
            isUserChange = false;
            CheckChipIDForCurrentEmployee();
            isUserChange = true;
            chipCheckTimer.Start();
        }
    }
}
