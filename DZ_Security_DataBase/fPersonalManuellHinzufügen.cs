using Microsoft.IdentityModel.Tokens;
using System.Data.SQLite;

namespace DZ_Security_DataBase
{
    public partial class cPersonalManuellHinzufügen : Form
    {
        static string folderPath = cDataBase.DbPath;
        static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        public cPersonalManuellHinzufügen()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        private void bAddWorker_Click(object sender, EventArgs e)
        {

            try
            {
                long workerID = 0;
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    string query = @"INSERT INTO Mitarbeiter 
                            (MitarbeiterID,Firma, Vorname, Nachname, Geburtsdatum, Geburtsland, Wohnort, ChipNummer, Gender, Muttersprache, Sprachen, TelefonNummer, Ansprechpartner, Position) 
                            VALUES 
                            (@ID,@Firma, @Vorname, @Nachname, @Geburtsdatum, @Geburtsland, @Wohnort, @ChipNummer, @Gender, @Muttersprache, @Sprachen, @TelefonNummer, @Ansprechpartner, @Position)";
                    using (var cmd = new SQLiteCommand("SELECT COUNT(MitarbeiterID) FROM Mitarbeiter", conn))
                    {
                        workerID = (long)cmd.ExecuteScalar() + 1;
                    }
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", workerID);
                        cmd.Parameters.AddWithValue("@Firma", tbCompany.Text.IsNullOrEmpty() ? null : tbCompany.Text);
                        cmd.Parameters.AddWithValue("@Vorname", tbName.Text.IsNullOrEmpty() ? null : tbName.Text);
                        cmd.Parameters.AddWithValue("@Nachname", tbSurname.Text.IsNullOrEmpty() ? null : tbSurname.Text);
                        cmd.Parameters.AddWithValue("@Geburtsdatum", tbBirthDay.Text.IsNullOrEmpty() ? null : tbBirthDay.Text);
                        cmd.Parameters.AddWithValue("@Geburtsland", tbBirthCountry.Text.IsNullOrEmpty() ? null : tbBirthCountry.Text);
                        cmd.Parameters.AddWithValue("@Wohnort", tbLiving.Text.IsNullOrEmpty() ? null : tbLiving.Text);
                        cmd.Parameters.AddWithValue("@ChipNummer", tbChipNumber.Text.IsNullOrEmpty() ? null : tbChipNumber.Text);
                        cmd.Parameters.AddWithValue("@Gender", tbGender.Text.IsNullOrEmpty() ? null : tbGender.Text);
                        cmd.Parameters.AddWithValue("@Muttersprache", tbMainLanguage.Text.IsNullOrEmpty() ? null : tbMainLanguage.Text);
                        cmd.Parameters.AddWithValue("@Sprachen", tbLanguage.Text.IsNullOrEmpty() ? null : tbLanguage.Text);
                        cmd.Parameters.AddWithValue("@TelefonNummer", tbMobileNumber.Text.IsNullOrEmpty() ? null : tbMobileNumber.Text);
                        cmd.Parameters.AddWithValue("@Ansprechpartner", tbContact.Text.IsNullOrEmpty() ? null : tbContact.Text);
                        cmd.Parameters.AddWithValue("@Position", tbPosition.Text.IsNullOrEmpty() ? null : tbPosition.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Mitarbeiter erfolgreich hinzugefügt!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Hinzufügen des Mitarbeiters: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cPersonalManuellHinzufügen_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
        }
    }
}
