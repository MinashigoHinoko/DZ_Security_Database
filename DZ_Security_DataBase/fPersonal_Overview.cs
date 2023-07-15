using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DZ_Security_DataBase
{
    public partial class cPersonalOverview : Form
    {
        static string folderPath = cDataBase.DbPath;
        static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        public cPersonalOverview()
        {
            InitializeComponent();
        }

        private void insertDatabaseInComboBox()
        {
            cbMitarbeiterID.Items.Clear();
            cbCompany.Items.Clear();
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT DISTINCT MitarbeiterID, Vorname || ' ' || Nachname AS Name FROM Mitarbeiter", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            string id = reader["MitarbeiterID"].ToString();

                            // Create a new cWorker object
                            cWorker mitarbeiter = new cWorker { ID = id, Name = name };
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
            string selectedCompany = cbCompany.SelectedItem.ToString();

            // Clear the items in the cbMitarbeiterID ComboBox
            cbMitarbeiterID.Items.Clear();

            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT MitarbeiterID, Vorname || ' ' || Nachname AS Name FROM Mitarbeiter WHERE Firma = @Company", conn))
                {
                    cmd.Parameters.AddWithValue("@Company", selectedCompany);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            string id = reader["MitarbeiterID"].ToString();

                            // Create a new cWorker object
                            cWorker mitarbeiter = new cWorker { ID = id, Name = name };
                            cbMitarbeiterID.Items.Add(mitarbeiter);
                        }
                    }
                }
            }
        }

        private void cbMitarbeiterID_SelectedIndexChanged(object sender, EventArgs e)
        {
            cWorker selectedWorker = cbMitarbeiterID.SelectedItem as cWorker;

            string oCurrentID = selectedWorker.ID; // now oCurrentID is the ID string
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT * FROM Mitarbeiter WHERE MitarbeiterID = @EmployeeId", conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", oCurrentID);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cbCompany.Text = reader["Firma"].ToString();
                            lbName.Text = reader["Vorname"].ToString();
                            lbSurName.Text = reader["Nachname"].ToString();
                            lbBirthday.Text = reader["Geburtsdatum"].ToString();
                            lbBirthCountry.Text = reader["Geburtsland"].ToString();
                            lbLiving.Text = reader["Wohnort"].ToString();
                            lbCheckedIn.Text = reader["CheckInState"].ToString() == "true" ? "Eingechecked" : "Ausgechecked";
                            lbNFCNumber.Text = reader["ChipNummer"].ToString();
                            lbGender.Text = reader["Gender"].ToString();
                            lbLanguage.Text = reader["Muttersprache"].ToString();
                            lbLanguageLvL.Text = reader["Sprachen"].ToString();
                            lbMobileNumber.Text = reader["TelefonNummer"].ToString();
                            lbContact.Text = reader["Ansprechpartner"].ToString();
                            lbPosition.Text = reader["Position"].ToString();
                        }
                    }
                }
            }

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
    }
}