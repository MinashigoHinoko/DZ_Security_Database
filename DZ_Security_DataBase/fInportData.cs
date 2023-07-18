using ExcelDataReader;
using System.Data;
using System.Data.SQLite;

namespace Festival_Manager
{
    public partial class fInportData : Form
    {
        bool isAdmin = false;
        string username;
        static string folderPath = cDataBase.DbPath;
        static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        public fInportData(bool isAdmin, string username)
        {
            InitializeComponent();
            this.username = username;
            this.isAdmin = isAdmin;
        }

        private void fInportData_FormClosed(object sender, FormClosedEventArgs e)
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

        private void fInportData_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string excelPath = "";

            // Öffnet eine Dialogbox und lässt den Benutzer den Pfad auswählen
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                DialogResult resu = ofd.ShowDialog();
                if (resu == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.FileName))
                {
                    excelPath = ofd.FileName;
                }
            }
            string tagOderNacht = "";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var excelStream = File.Open(excelPath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(excelStream);

            var conf = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true,
                    ReadHeaderRow = rowReader => {
                        // F skip the first two rows
                        for (int i = 0; i < 2; i++)
                        {
                            rowReader.Read();
                        }
                    }
                }
            };

            var result = reader.AsDataSet(conf);
            DataTable table = result.Tables[0];

            // Verbindung zur SQLite-Datenbank herstellen
            using var conn = new SQLiteConnection(stConnectionString);
            conn.Open();

            foreach (DataRow row in table.Rows)
            {
                string position = row["Positionsbezeichnung"].ToString();
                if (position == "Tag" || position == "Nacht")
                {
                    tagOderNacht = position;
                    continue;
                }

                string name = row["Name, Vorname"].ToString();
                string[] splittedName = name.Split(',');
                string nachname = splittedName[0];
                string vorname = splittedName.Length > 1 ? splittedName[1] : "";
                string firma = row["Firma"].ToString();
                DateTime checkInSoll = DateTime.Parse(row["von"].ToString());
                DateTime checkOutSoll = DateTime.Parse(row["bis"].ToString());

                string sql = @"INSERT INTO Mitarbeiter (Vorname, Nachname, Firma, Position, CheckInSoll, CheckOutSoll, Nacht) 
               VALUES (@vorname, @nachname, @firma, @position, @checkInSoll, @checkOutSoll, @nacht)";

                using var cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@vorname", vorname);
                cmd.Parameters.AddWithValue("@nachname", nachname);
                cmd.Parameters.AddWithValue("@firma", firma);
                cmd.Parameters.AddWithValue("@position", position);
                cmd.Parameters.AddWithValue("@checkInSoll", checkInSoll);
                cmd.Parameters.AddWithValue("@checkOutSoll", checkOutSoll);
                cmd.Parameters.AddWithValue("@nacht", tagOderNacht == "Nacht" ? "true" : "false");

                cmd.ExecuteNonQuery();
            }

            conn.Close();
        }

    }
}
