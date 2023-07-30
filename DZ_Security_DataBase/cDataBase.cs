using System.Configuration;
using System.Data.SQLite;
using System.Security.Cryptography;

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
                using (FolderBrowserDialog fbd = new())
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
                    else
                    {
                        // Der Benutzer hat keinen Pfad ausgewählt oder den Dialog abgebrochen. Wiederholen Sie den Vorgang oder beenden Sie die Methode.
                        goto restart;
                    }
                }
            }
            else
            {
                string dbFile = System.IO.Path.Combine(DbPath, "Dz_Security.sqlite");
                if (!File.Exists(dbFile))
                {
                    // Öffnet eine Dialogbox und lässt den Benutzer den Pfad auswählen
                    using (FolderBrowserDialog fbd = new())
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
                        else
                        {
                            // Der Benutzer hat keinen Pfad ausgewählt oder den Dialog abgebrochen. Wiederholen Sie den Vorgang oder beenden Sie die Methode.
                            goto restart;
                        }
                    }
                }
            }



            if (string.IsNullOrEmpty(DbPath))
            {
                throw new InvalidOperationException("DbPath is null or empty.");
            }

            string dbFilePath = System.IO.Path.Combine(DbPath, "Dz_Security.sqlite");

            if (!File.Exists(dbFilePath))
            {
                freshlyCreated = true;
                SQLiteConnection.CreateFile(dbFilePath);
                using (SQLiteConnection m_dbConnection = new(GetConnectionString()))
                {
                    m_dbConnection.Open();


                    // Mitarbeiter Tabelle erstellen
                    string sql = @"CREATE TABLE Mitarbeiter (
                                    MitarbeiterID INTEGER PRIMARY KEY AUTOINCREMENT, 
                                    Firma TEXT NOT NULL,
                                    Vorname TEXT NOT NULL, 
                                    Nachname TEXT NOT NULL,
                                    Geburtsname TEXT,
                                    Geburtsort TEXT,
                                    Geburtsland TEXT,
                                    Geburtsdatum TEXT NOT NULL,
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
                                    Gender TEXT,
                                    Position TEXT,
                                    ChipNummer TEXT,
                                    CheckInState TEXT DEFAULT 'false' NOT NULL,
                                    IstKrank TEXT,
                                    RentState TEXT DEFAULT 'false' NOT NULL,
                                    WeitereInformationen TEXT
                                   );";

                    using (SQLiteCommand command = new(sql, m_dbConnection))
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

                    using (SQLiteCommand command = new(sql, m_dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                    sql = @"CREATE TABLE ArbeitszeitenSoll (
                             MitarbeiterID INT NOT NULL, 
                             CheckedInSoll DATETIME,
                             CheckedOutSoll DATETIME,
                             Nacht Text DEFAULT 'false' NOT NULL,
                             Position TEXT,
                             FOREIGN KEY(MitarbeiterID) REFERENCES Mitarbeiter(MitarbeiterID)
                             );";

                    using (SQLiteCommand command = new(sql, m_dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                    sql = @"CREATE TABLE MitarbeiterSprachen (
                             MitarbeiterID INT NOT NULL, 
                             Sprache NOT NULL,
                             Muttersprache DEFAULT 'false' NOT NULL,
                             FOREIGN KEY(MitarbeiterID) REFERENCES Mitarbeiter(MitarbeiterID)
                             );";

                    using (SQLiteCommand command = new(sql, m_dbConnection))
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

                    using (SQLiteCommand command = new(sql, m_dbConnection))
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

                    using (SQLiteCommand command = new(sql, m_dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                    // Ausrüstungs Tabelle erstellen
                    sql = @"CREATE TABLE Ausruestung (
                             ID TEXT PRIMARY KEY NOT NULL, 
                             Art TEXT NOT NULL,
                             Farbe TEXT NOT NULL,
                             MitarbeiterID INT,
                             Zustand TEXT DEFAULT 'gut',
                             FOREIGN KEY(MitarbeiterID) REFERENCES Mitarbeiter(MitarbeiterID)
                             );";

                    using (SQLiteCommand command = new(sql, m_dbConnection))
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
                             Vorgesetzter INT
                             );";

                    using (SQLiteCommand command = new(sql, m_dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                    // Ausrüstungs Tabelle erstellen
                    sql = @"CREATE TABLE Funkgeraete (
                             ID TEXT NOT NULL, 
                             Bleibt TEXT DEFAULT 'false' NOT NULL,
                             Akku INT DEFAULT 0 NOT NULL,
                             Funkgeraet TEXT DEFAULT 'false' NOT NULL,
                             Tarn_Headset TEXT DEFAULT 'false' NOT NULL,
                             Rasierer TEXT DEFAULT 'false' NOT NULL,
                             Mikimaus TEXTDEFAULT 'false' NOT NULL,
                             MitarbeiterID INT,
                             Verbrauchsmaterial TEXT,
                             Sonstiges TEXT,
                             FOREIGN KEY(MitarbeiterID) REFERENCES Mitarbeiter(MitarbeiterID)
                             );";

                    using (SQLiteCommand command = new(sql, m_dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Standard-Admin hinzufügen
                    string username = "admin";
                    string password = "admin";
                    string rights = "admin";

                    // Salt und hashed Password generieren
                    RNGCryptoServiceProvider rng = new();
                    byte[] saltBytes = new byte[32];
                    rng.GetBytes(saltBytes);
                    string salt = Convert.ToBase64String(saltBytes);

                    Rfc2898DeriveBytes pbkdf2 = new(password, saltBytes, 10000);
                    byte[] passwordBytes = pbkdf2.GetBytes(20);
                    string hashedPassword = Convert.ToBase64String(passwordBytes);

                    sql = @"INSERT INTO Passwort (Username, HashedPassword, Salt, Rights) 
               VALUES (@username, @hashedPassword, @salt, @rights)";
                    using (SQLiteCommand cmd = new(sql, m_dbConnection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@hashedPassword", hashedPassword);
                        cmd.Parameters.AddWithValue("@salt", salt);
                        cmd.Parameters.AddWithValue("@rights", rights);
                        cmd.ExecuteNonQuery();
                    }

                }
            }
        }
    }
}