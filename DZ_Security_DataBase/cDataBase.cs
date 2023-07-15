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
            restart:
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
            if (DbPath == null)
            {
                DialogResult result = MessageBox.Show("Zugriff von Ihrem System verweigert, wollen Sie es erneut versuchen?", "Fehler", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                if (result == DialogResult.Cancel)
                {
                    // User clicked "Beenden"
                    Environment.Exit(0); // Exit the application
                }
                goto restart; 
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
                                    Firma TEXT NOT NULL,
                                    Vorname TEXT, 
                                    Nachname TEXT,
                                    Geburtsname TEXT,
                                    Geburtsort TEXT,
                                    Geburtsland TEXT,
                                    Geburtsdatum DATE,
                                    Nationalitaet TEXT,
                                    Straße TEXT,
                                    Hausnummer TEXT,
                                    PLZ INT,
                                    Wohnort TEXT,
                                    Bundesland TEXT,
                                    Ausweis_Art TEXT,
                                    Ausweis_Nr TEXT,
                                    Bewacherregister_Nr TEXT,
                                    Security_Typ TEXT,
                                    Muttersprache TEXT,
                                    Sprachen TEXT,
                                    SprachNiveau TEXT,
                                    Gender TEXT,
                                    TelefonNummer TEXT,
                                    Ansprechpartner TEXT,
                                    Position TEXT,
                                    ChipNummer INT,
                                    CheckInState TEXT DEFAULT 'false' NOT NULL,
                                    IstKrank TEXT,
                                    WeitereInformationen TEXT
                                   );";

                    using (var command = new SQLiteCommand(sql, m_dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Arbeitszeiten Tabelle erstellen
                    sql = @"CREATE TABLE Arbeitszeiten (
                             MitarbeiterID INT NOT NULL, 
                             CheckedIn DATETIME,
                             CheckedOut DATETIME,
                             FOREIGN KEY(MitarbeiterID) REFERENCES Mitarbeiter(MitarbeiterID)
                             );";

                    using (var command = new SQLiteCommand(sql, m_dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                    // Passwort Tabelle erstellen
                    sql = @"CREATE TABLE Passwort (
                             Username TEXT PRIMARY KEY NOT NULL, 
                             HashedPassword TEXT NOT NULL,
                             Salt TEXT NOT NULL,
                             Rights TEXT NOT NULL
                             );";

                    using (var command = new SQLiteCommand(sql, m_dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                    // Ausrüstungs Tabelle erstellen
                    sql = @"CREATE TABLE Ausruestung (
                             ID INT PRIMARY KEY NOT NULL, 
                             Art TEXT NOT NULL,
                             Farbe TEXT NOT NULL,
                             Position TEXT NOT NULL,
                             Status TEXT DEFAULT 'Ausleihbar' NOT NULL,
                             MitarbeiterID INT,
                             Zustand TEXT DEFAULT 'Gut' NOT NULL,
                             FOREIGN KEY(MitarbeiterID) REFERENCES Mitarbeiter(MitarbeiterID)
                             );";

                    using (var command = new SQLiteCommand(sql, m_dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                    // Position Tabelle erstellen
                    sql = @"CREATE TABLE Position (
                             Nr INT PRIMARY KEY NOT NULL, 
                             Geschlecht TEXT,
                             Quadrat INT,
                             Bezeichnung TEXT,
                             Zusatz TEXT,
                             Bemerkung TEXT,
                             Benötigt TEXT,
                             Vorgesetzer INT
                             );";

                    using (var command = new SQLiteCommand(sql, m_dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                    // Ausrüstungs Tabelle erstellen
                    sql = @"CREATE TABLE Funkgeraete (
                             ID INT PRIMARY KEY NOT NULL, 
                             Bleibt TEXT NOT NULL,
                             Akku INT NOT NULL,
                             Tarn_Headset TEXT,
                             Rasierer TEXT,
                             Mikimaus TEXT,
                             Verbrauchsmaterial TEXT,
                             Sonstiges TEXT
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
                    int[] mitarbeiterIds = { 1, 2, 3, 4 };
                    string[] surNames = { "Max", "Max", "John", "Test" };
                    string[] name = { "Mustermann", "Mustermann", "Doe", "Name" };
                    string firma = "DZ_Security";
                    string ChipNummer = "123456";
                    string weitereInformationen = "Keine weiteren Informationen";

                    for (int i = 0; i < mitarbeiterIds.Length; i++)
                    {
                        cmd.CommandText = $"SELECT COUNT(*) FROM Mitarbeiter WHERE MitarbeiterID = @MitarbeiterID";
                        cmd.Parameters.AddWithValue("@MitarbeiterID", mitarbeiterIds[i]);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count == 0)
                        {
                            cmd.CommandText = @"INSERT INTO Mitarbeiter 
                            (MitarbeiterID,Firma, Vorname, Nachname, ChipNummer, WeitereInformationen) 
                            VALUES (@MitarbeiterID,@Firma, @Vorname,@Nachname, @ChipNummer, @WeitereInformationen);";
                            cmd.Parameters.AddWithValue("@MitarbeiterID", mitarbeiterIds[i]);
                            cmd.Parameters.AddWithValue("@Firma", firma);
                            cmd.Parameters.AddWithValue("@Vorname", surNames[i]);
                            cmd.Parameters.AddWithValue("@Nachname", name[i]);
                            cmd.Parameters.AddWithValue("@ChipNummer", ChipNummer);
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