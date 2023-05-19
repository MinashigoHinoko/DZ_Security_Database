using Microsoft.Extensions.DependencyInjection;
using System.Data.SQLite;

namespace DZ_Security_DataBase
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //SQLiteConnection.CreateFile("MeineDatenbank.sqlite");
            builDatabase();
            //editDatabase();
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
        static void builDatabase()
        {
            try
            {
                string connectionString = "Data Source=D:\\MEGA\\Freelancing\\DZ_Security\\DZ_Security_DataBase\\DZ_Security_DataBase\\bin\\Debug\\net6.0-windows\\MeineDatenbank.sqlite;Version=3;";
                using (var m_dbConnection = new SQLiteConnection(connectionString))
                {
                    m_dbConnection.Open();

                    // Mitarbeiter Tabelle erstellen
                    string sql = @"CREATE TABLE Mitarbeiter (
                    MitarbeiterID INT PRIMARY KEY NOT NULL, 
                    Name TEXT NOT NULL, 
                    Position TEXT,
                    RFIDChipNummer TEXT,
                    WeitereInformationen TEXT
                   );";

                    using (var command = new SQLiteCommand(sql, m_dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Arbeitszeiten Tabelle erstellen
                    sql = @"CREATE TABLE Arbeitszeiten (
                     MitarbeiterID INT NOT NULL, 
                     ZeitstempelEingetragen DATETIME,
                     ZeitstempelAusgetragen DATETIME,
                     FOREIGN KEY(MitarbeiterID) REFERENCES Mitarbeiter(MitarbeiterID)
                     );";

                    using (var command = new SQLiteCommand(sql, m_dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {

                return;
            }
        }
        static void editDatabase()
        {
            string connectionString = "Data Source=D:\\MEGA\\Freelancing\\DZ_Security\\DZ_Security_DataBase\\DZ_Security_DataBase\\bin\\Debug\\net6.0-windows\\MeineDatenbank.sqlite;Version=3;";
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"INSERT INTO Mitarbeiter 
                            (MitarbeiterID, Name, Position, RFIDChipNummer, WeitereInformationen) 
                            VALUES (@MitarbeiterID, @Name, @Position, @RFIDChipNummer, @WeitereInformationen);";

                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@MitarbeiterID", 4);
                    cmd.Parameters.AddWithValue("@Name", "Max Mustermann");
                    cmd.Parameters.AddWithValue("@Position", "South");
                    cmd.Parameters.AddWithValue("@RFIDChipNummer", "123456");
                    cmd.Parameters.AddWithValue("@WeitereInformationen", "Keine weiteren Informationen");

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}