using Microsoft.IdentityModel.Tokens;
using PCSC;
using System.Data.SQLite;

namespace Festival_Manager
{
    public partial class cPersonalManuellHinzufügen : Form
    {
        private static string folderPath = cDataBase.DbPath;
        private static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        private string username;
        public cPersonalManuellHinzufügen(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void bAddWorker_Click(object sender, EventArgs e)
        {
            try
            {
                long workerID = 0;
                using (SQLiteConnection conn = new(stConnectionString))
                {
                    conn.Open();

                    string query = @"INSERT INTO Mitarbeiter 
                    (Firma, Vorname, Nachname, Geburtsdatum, Geburtsland, Wohnort, ChipNummer, Gender, Position) 
                    VALUES 
                    (@Firma, @Vorname, @Nachname, @Geburtsdatum, @Geburtsland, @Wohnort, @ChipNummer, @Gender, @Position)";

                    string idQuery = "SELECT last_insert_rowid()";
                    using (SQLiteCommand idCmd = new(idQuery, conn))
                    {
                        workerID = (long)idCmd.ExecuteScalar();
                    }

                    using (SQLiteCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Firma", tbCompany.Text.IsNullOrEmpty() ? null : tbCompany.Text);
                        cmd.Parameters.AddWithValue("@Vorname", tbName.Text.IsNullOrEmpty() ? null : tbName.Text);
                        cmd.Parameters.AddWithValue("@Nachname", tbSurname.Text.IsNullOrEmpty() ? null : tbSurname.Text);
                        cmd.Parameters.AddWithValue("@Geburtsdatum", tbBirthDay.Text.IsNullOrEmpty() ? null : tbBirthDay.Text);
                        cmd.Parameters.AddWithValue("@Geburtsland", tbBirthCountry.Text.IsNullOrEmpty() ? null : tbBirthCountry.Text);
                        cmd.Parameters.AddWithValue("@Wohnort", tbLiving.Text.IsNullOrEmpty() ? null : tbLiving.Text);
                        cmd.Parameters.AddWithValue("@ChipNummer", lbChip.Text.IsNullOrEmpty() ? null : lbChip.Text);
                        cmd.Parameters.AddWithValue("@Gender", tbGender.Text.IsNullOrEmpty() ? null : tbGender.Text);
                        cmd.Parameters.AddWithValue("@Position", tbPosition.Text.IsNullOrEmpty() ? null : tbPosition.Text);

                        cmd.ExecuteNonQuery();
                    }

                    // Insert main language as mother tongue
                    string mainLanguage = tbMainLanguage.Text.Trim();
                    if (!string.IsNullOrEmpty(mainLanguage))
                    {
                        string insertMainLanguageQuery = @"INSERT INTO MitarbeiterSprachen (MitarbeiterID, Sprache, Muttersprache) VALUES (@ID, @Sprache, 'true')";
                        using (SQLiteCommand cmd = new(insertMainLanguageQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@ID", workerID);
                            cmd.Parameters.AddWithValue("@Sprache", mainLanguage);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Insert other languages
                    string[] languages = tbLanguage.Text.Split(',');
                    foreach (string language in languages)
                    {
                        string trimmedLanguage = language.Trim();
                        if (!string.IsNullOrEmpty(trimmedLanguage))
                        {
                            string insertLanguageQuery = @"INSERT INTO MitarbeiterSprachen (MitarbeiterID, Sprache) VALUES (@ID, @Sprache)";
                            using (SQLiteCommand cmd = new(insertLanguageQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@ID", workerID);
                                cmd.Parameters.AddWithValue("@Sprache", trimmedLanguage);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                }
                cLogger.LogDatabaseChange($"Added New Worker {workerID}", username);
                MessageBox.Show("Mitarbeiter erfolgreich hinzugefügt!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Hide();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Hinzufügen des Mitarbeiters: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cPersonalManuellHinzufügen_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hide();
        }
        private void CheckChipIDForCurrentEmployee()
        {
            string chipId = string.Empty; // Initialisieren Sie chipId außerhalb der If-Else-Anweisung

            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new(conn))
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

                            while (!uniqueChipIdFound)
                            {
                                chipIdString = Microsoft.VisualBasic.Interaction.InputBox("Bitte geben Sie die Chip-ID manuell ein:", "Manuelle Eingabe", "");

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
                            lbChip.Text = chipId;
                            cLogger.LogDatabaseChange($"Die Chip-ID {chipId} wurde eingelesen", username);
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

                                bool isCanceled = false;

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
                                    else
                                    {
                                        MessageBox.Show("Einlesen Abgebrochen");
                                        isCanceled = true;
                                    }
                                }
                                while (sc != SCardError.Success || isCanceled);

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
                                            conn.Close();
                                        }
                                    }
                                    lbChip.Text = chipId;
                                    cLogger.LogDatabaseChange($"Die Chip-ID {chipId} wurde eingescanned", username);
                                }
                                else
                                {
                                    if (isCanceled)
                                    {
                                        return;
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
                }
            }
        }

        private void cPersonalManuellHinzufügen_Load(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void bRead_Click(object sender, EventArgs e)
        {
            CheckChipIDForCurrentEmployee();
        }
    }
}
