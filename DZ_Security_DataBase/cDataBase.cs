using System.Data.SQLite;

namespace DZ_Security_DataBase
{
    internal static class cDataBase
    {
        static bool freshlyCreated = false;

        public static string DbPath { get; set; }

        private static string GetConnectionString()
        {
            return $"Data Source={DbPath}\\Dz_Security.sqlite;Version=3;";
        }

        internal static void createDatabase()
        {
            // If the database file doesn't exist, ask the user for a path.
            if (!File.Exists($"{DbPath}\\Dz_Security.sqlite"))
            {
                // Öffnet eine Dialogbox und lässt den Benutzer den Pfad auswählen
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        cDataBase.DbPath = fbd.SelectedPath; // Speichern des Pfades in der statischen Eigenschaft
                    }
                }

                freshlyCreated = true;
                SQLiteConnection.CreateFile($"{DbPath}\\Dz_Security.sqlite");
                using (var m_dbConnection = new SQLiteConnection(GetConnectionString()))
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
             ZeitstempelEingetragen DATETIME DEFAULT '0000-00-00 00:00:00',
             ZeitstempelAusgetragen DATETIME DEFAULT '0000-00-00 00:00:00',
             FOREIGN KEY(MitarbeiterID) REFERENCES Mitarbeiter(MitarbeiterID)
             );";

                    using (var command = new SQLiteCommand(sql, m_dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }




        internal static void editDatabase()
        {
            if (!freshlyCreated)
            {
                return;
            }
            using (var conn = new SQLiteConnection(GetConnectionString()))
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