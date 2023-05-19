using Microsoft.Extensions.DependencyInjection;
using System.Data.SQLite;

namespace DZ_Security_DataBase
{
    internal static class Program
    {
        static string folderName = "datenBank";
        static string folderPath = Path.Combine(Application.StartupPath, folderName);
        static string stConnectionString = $"Data Source={folderPath}\\MeineDatenbank.sqlite;Version=3;";
        static bool freshlyCreated = false;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            createDatabase();
            editDatabase();
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
        static void createDatabase()
        {
            if (Directory.Exists(folderPath))
            {
                return;
            }
            else
            {
                freshlyCreated = true;
                string connectionString = $"Data Source={folderPath}\\MeineDatenbank.sqlite;Version=3;";
                Directory.CreateDirectory(folderPath);
                SQLiteConnection.CreateFile($"{folderPath}\\MeineDatenbank.sqlite");
                using (var m_dbConnection = new SQLiteConnection(stConnectionString))
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
        }
        static void editDatabase()
        {
            if(!freshlyCreated)
            {
                return;
            }
            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"INSERT INTO Mitarbeiter 
                            (MitarbeiterID, Name, Position, RFIDChipNummer, WeitereInformationen) 
                            VALUES (@MitarbeiterID, @Name, @Position, @RFIDChipNummer, @WeitereInformationen);";

                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@MitarbeiterID", 00353485);
                    cmd.Parameters.AddWithValue("@Name", "Max Mustermann");
                    cmd.Parameters.AddWithValue("@Position", "Norden");
                    cmd.Parameters.AddWithValue("@RFIDChipNummer", "123456");
                    cmd.Parameters.AddWithValue("@WeitereInformationen", "Keine weiteren Informationen");
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"INSERT INTO Mitarbeiter 
                            (MitarbeiterID, Name, Position, RFIDChipNummer, WeitereInformationen) 
                            VALUES (@MitarbeiterID, @Name, @Position, @RFIDChipNummer, @WeitereInformationen);";

                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@MitarbeiterID", 00353486);
                    cmd.Parameters.AddWithValue("@Name", "Max Mustermann");
                    cmd.Parameters.AddWithValue("@Position", "Süden");
                    cmd.Parameters.AddWithValue("@RFIDChipNummer", "123456");
                    cmd.Parameters.AddWithValue("@WeitereInformationen", "Keine weiteren Informationen");
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"INSERT INTO Mitarbeiter 
                            (MitarbeiterID, Name, Position, RFIDChipNummer, WeitereInformationen) 
                            VALUES (@MitarbeiterID, @Name, @Position, @RFIDChipNummer, @WeitereInformationen);";

                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@MitarbeiterID", 00353487);
                    cmd.Parameters.AddWithValue("@Name", "John Doe");
                    cmd.Parameters.AddWithValue("@Position", "Osten");
                    cmd.Parameters.AddWithValue("@RFIDChipNummer", "123456");
                    cmd.Parameters.AddWithValue("@WeitereInformationen", "Keine weiteren Informationen");
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"INSERT INTO Mitarbeiter 
                            (MitarbeiterID, Name, Position, RFIDChipNummer, WeitereInformationen) 
                            VALUES (@MitarbeiterID, @Name, @Position, @RFIDChipNummer, @WeitereInformationen);";


                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@MitarbeiterID", 00353484);
                    cmd.Parameters.AddWithValue("@Name", "Test Name");
                    cmd.Parameters.AddWithValue("@Position", "Westen");
                    cmd.Parameters.AddWithValue("@RFIDChipNummer", "123456");
                    cmd.Parameters.AddWithValue("@WeitereInformationen", "Keine weiteren Informationen");

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}