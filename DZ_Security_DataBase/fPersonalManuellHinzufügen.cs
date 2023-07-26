using Microsoft.IdentityModel.Tokens;
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
            TopMost = true;
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
                        cmd.Parameters.AddWithValue("@ChipNummer", tbChipNumber.Text.IsNullOrEmpty() ? null : tbChipNumber.Text);
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

        private void cPersonalManuellHinzufügen_Load(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
