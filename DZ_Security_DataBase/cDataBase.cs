using System.Configuration;
using System.Data.SQLite;

namespace Festival_Manager
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
            // Lesen Sie den Pfad aus der App.config-Datei
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationElement dbPathSetting = config.AppSettings.Settings["DbPath"];

            // Überprüfen Sie, ob die Einstellung existiert
            if (dbPathSetting == null)
            {
                dbPathSetting = new KeyValueConfigurationElement("DbPath", string.Empty);
                config.AppSettings.Settings.Add(dbPathSetting);
            }

            DbPath = dbPathSetting.Value;

            // Wenn DbPath null oder ungültig ist, fragen Sie den Benutzer nach einem Pfad.
            if (string.IsNullOrEmpty(DbPath) || !Directory.Exists(DbPath))
            {
                // Öffnet eine Dialogbox und lässt den Benutzer den Pfad auswählen
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        cDataBase.DbPath = fbd.SelectedPath; // Speichern des Pfades in der statischen Eigenschaft

                        // Pfad in der App.config-Datei aktualisieren
                        dbPathSetting.Value = DbPath;
                        config.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");
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
                                    Firma TEXT,
                                    Vorname TEXT, 
                                    Nachname TEXT,
                                    Geburtsname TEXT,
                                    Geburtsort TEXT,
                                    Geburtsland TEXT,
                                    Geburtsdatum TEXT,
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
                                    RentState TEXT DEFAULT 'false' NOT NULL,
                                    Nacht Text DEFAULT 'false' NOT NULL,
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
                    sql = @"CREATE TABLE ArbeitszeitenSoll (
                             MitarbeiterID INT NOT NULL, 
                             CheckedInSoll DATETIME,
                             CheckedOutSoll DATETIME,
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
                             Rights TEXT NOT NULL,
                             canEdit TEXT DEFAULT 'false' NOT NULL,
                             PIN TEXT
                             );";

                    using (var command = new SQLiteCommand(sql, m_dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                    // Arbeitszeiten Tabelle erstellen
                    sql = @"CREATE TABLE LogTable (
                            ID INTEGER PRIMARY KEY AUTOINCREMENT,
                            Timestamp TEXT NOT NULL,
                            User TEXT NOT NULL,
                            Action TEXT NOT NULL
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
                             Quadrant TEXT,
                             Farbe TEXT,
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
                             Akku INT DEFAULT 0 NOT NULL,
                             Funkgeraet TEXT,
                             Tarn_Headset TEXT,
                             Rasierer TEXT,
                             Mikimaus TEXT,
                             Status TEXT DEFAULT 'Ausleihbar' NOT NULL,
                             MitarbeiterID INT,
                             Verbrauchsmaterial TEXT,
                             Sonstiges TEXT,
                             FOREIGN KEY(MitarbeiterID) REFERENCES Mitarbeiter(MitarbeiterID)
                             );";

                    using (var command = new SQLiteCommand(sql, m_dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}