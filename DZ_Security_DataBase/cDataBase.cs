using System.Data.SQLite;

namespace DZ_Security_DataBase
{
    internal static class cDataBase
    {
        private static bool freshlyCreated = false;
        public static string DbPath { get; set; }

        private static string GetConnectionString()
        {
            return $"Data Source={DbPath}\\Dz_Security.sqlite;Version=3;";
        }

        internal static void createDatabase()
        {
            // If DbPath is null, ask the user for a path.
            if (DbPath == null)
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
            }

            // If the database file doesn't exist, create it.
            if (!File.Exists($"{DbPath}\\Dz_Security.sqlite"))
            {
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
            // Only proceed if the database was freshly created
            if (DbPath == null || !freshlyCreated)
            {
                return;
            }

            using (var conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    int[] mitarbeiterIds = { 00353485, 00353486, 00353487, 00353484 };
                    string[] names = { "Max Mustermann", "Max Mustermann", "John Doe", "Test Name" };
                    string[] positions = { "Norden", "Süden", "Osten", "Westen" };
                    string rfidChipNummer = "123456";
                    string weitereInformationen = "Keine weiteren Informationen";

                    for (int i = 0; i < mitarbeiterIds.Length; i++)
                    {
                        cmd.CommandText = $"SELECT COUNT(*) FROM Mitarbeiter WHERE MitarbeiterID = @MitarbeiterID";
                        cmd.Parameters.AddWithValue("@MitarbeiterID", mitarbeiterIds[i]);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count == 0)
                        {
                            cmd.CommandText = @"INSERT INTO Mitarbeiter 
                            (MitarbeiterID, Name, Position, RFIDChipNummer, WeitereInformationen) 
                            VALUES (@MitarbeiterID, @Name, @Position, @RFIDChipNummer, @WeitereInformationen);";
                            cmd.Parameters.AddWithValue("@Name", names[i]);
                            cmd.Parameters.AddWithValue("@Position", positions[i]);
                            cmd.Parameters.AddWithValue("@RFIDChipNummer", rfidChipNummer);
                            cmd.Parameters.AddWithValue("@WeitereInformationen", weitereInformationen);

                            cmd.ExecuteNonQuery();
                        }
                        cmd.Parameters.Clear();
                    }
                }
            }
        }
    }
}