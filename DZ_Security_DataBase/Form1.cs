using OfficeOpenXml;
using System.Data;
using System.Data.SQLite;

namespace DZ_Security_DataBase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            buildDatabase();
            insertDatabaseInComboBox();
        }
        private void buildDatabase()
        {
                string connectionString = "Data Source=D:\\MEGA\\Freelancing\\DZ_Security\\DZ_Security_DataBase\\DZ_Security_DataBase\\bin\\Debug\\net6.0-windows\\MeineDatenbank.sqlite;Version=3;";
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                using (var cmd = new SQLiteCommand("SELECT MitarbeiterID, strftime('%Y-%m-%d %H:%M:%S', ZeitstempelEingetragen) AS ZeitstempelEingetragen, strftime('%Y-%m-%d %H:%M:%S', ZeitstempelAusgetragen) AS ZeitstempelAusgetragen FROM Arbeitszeiten", conn))

                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                        // Hier setzen wir die Datenquelle des DataGridView auf das DataTable.
                        this.dgvArbeitszeit.DataSource = dt;
                        }
                    }
                }
            }
        private void insertDatabaseInComboBox()
        {
            string connectionString = "Data Source=D:\\MEGA\\Freelancing\\DZ_Security\\DZ_Security_DataBase\\DZ_Security_DataBase\\bin\\Debug\\net6.0-windows\\MeineDatenbank.sqlite;Version=3;";
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT MitarbeiterID FROM Mitarbeiter", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            this.cbMitarbeiterID.Items.Add(reader["MitarbeiterID"].ToString());
                        }
                    }
                }
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            object oCurrentID = cbMitarbeiterID.SelectedItem;
            string connectionString = "Data Source=D:\\MEGA\\Freelancing\\DZ_Security\\DZ_Security_DataBase\\DZ_Security_DataBase\\bin\\Debug\\net6.0-windows\\MeineDatenbank.sqlite;Version=3;";
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand($"SELECT * FROM Mitarbeiter WHERE MitarbeiterID = {oCurrentID}", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Führen Sie hier den Code aus, den Sie für jeden Mitarbeiter mit der MitarbeiterID 3 ausführen möchten.
                            // Zum Beispiel:
                            Console.WriteLine($"Name: {reader["Name"]}, Position: {reader["Position"]}");
                            lbName.Text = reader["Name"].ToString();
                            lbPosition.Text= reader["Position"].ToString();
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            object oCurrentID = cbMitarbeiterID.SelectedItem;
            string connectionString = "Data Source=D:\\MEGA\\Freelancing\\DZ_Security\\DZ_Security_DataBase\\DZ_Security_DataBase\\bin\\Debug\\net6.0-windows\\MeineDatenbank.sqlite;Version=3;";
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"
                        INSERT INTO Arbeitszeiten (MitarbeiterID, ZeitstempelEingetragen, ZeitstempelAusgetragen)
                        VALUES (@id, @jetzt, @ausgetragen)";
                        cmd.Parameters.AddWithValue("@id", oCurrentID);
                        cmd.Parameters.AddWithValue("@jetzt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@ausgetragen", "");

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"
                    UPDATE Arbeitszeiten
                    SET ZeitstempelEingetragen = @eingetragen
                    WHERE MitarbeiterID = @id";
                        cmd.Parameters.AddWithValue("@eingetragen", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@id", oCurrentID);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            buildDatabase();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            object oCurrentID = cbMitarbeiterID.SelectedItem;
            string connectionString = "Data Source=D:\\MEGA\\Freelancing\\DZ_Security\\DZ_Security_DataBase\\DZ_Security_DataBase\\bin\\Debug\\net6.0-windows\\MeineDatenbank.sqlite;Version=3;";
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                    UPDATE Arbeitszeiten
                    SET ZeitstempelAusgetragen = @ausgetragen
                    WHERE MitarbeiterID = @id";
                    cmd.Parameters.AddWithValue("@ausgetragen", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@id", oCurrentID);

                    cmd.ExecuteNonQuery();
                }
            }
            buildDatabase();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataTable dt;

            string filePath = @"D:\YourExcelFile.xlsx";

            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Arbeitszeiten");
                ws.Cells["A1"].LoadFromDataTable(dt, true);
                pck.SaveAs(new FileInfo(filePath));
            }
        }
    }
}