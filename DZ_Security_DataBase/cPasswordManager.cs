using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace Festival_Manager
{
    internal class cPasswordManager
    {
        public class Registration
        {

            static string folderPath = cDataBase.DbPath;
            static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
            public bool RegisterUser(string username, string password, string rights, bool canEdit, string pin = null)
            {
                // Generate a new salt
                string salt = GenerateSalt();

                // Hash the password with the salt
                string hashedPassword = HashPasswordWithSalt(password, salt);
                // Now store the username, hashed password and salt in the database
                // Validiert die PIN-Eingabe, wenn die CheckBox aktiviert ist
                if (canEdit)
                {
                    if (int.TryParse(pin, out int pinNumber) && pin.Length == 4)
                    {
                        using (var conn = new SQLiteConnection(stConnectionString))
                        {
                            conn.Open();
                            using (var cmd = new SQLiteCommand("INSERT INTO Passwort (Username, HashedPassword, Salt, Rights, canEdit, PIN) VALUES (@username, @password, @salt, @rights, @canEdit, @pin)", conn))
                            {
                                cmd.Parameters.AddWithValue("@username", username);
                                cmd.Parameters.AddWithValue("@password", hashedPassword);
                                cmd.Parameters.AddWithValue("@salt", salt);
                                cmd.Parameters.AddWithValue("@rights", rights);
                                cmd.Parameters.AddWithValue("@canEdit", canEdit);
                                cmd.Parameters.AddWithValue("@pin", pinNumber == 0 ? DBNull.Value : pin);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Die PIN muss eine vierstellige Zahl sein.");
                        return false;
                    }
                }
                else
                {
                    using (var conn = new SQLiteConnection(stConnectionString))
                    {
                        conn.Open();
                        using (var cmd = new SQLiteCommand("INSERT INTO Passwort (Username, HashedPassword, Salt, Rights, canEdit, PIN) VALUES (@username, @password, @salt, @rights, @canEdit, @pin)", conn))
                        {
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", hashedPassword);
                            cmd.Parameters.AddWithValue("@salt", salt);
                            cmd.Parameters.AddWithValue("@rights", rights);
                            cmd.Parameters.AddWithValue("@canEdit", canEdit);
                            cmd.Parameters.AddWithValue("@pin", DBNull.Value);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    return true;
                }
            }

            private static string GenerateSalt()
            {
                byte[] saltBytes = new byte[32];
                using (var provider = new RNGCryptoServiceProvider())
                {
                    provider.GetBytes(saltBytes);
                }

                return Convert.ToBase64String(saltBytes);
            }

            private static string HashPasswordWithSalt(string password, string salt)
            {
                byte[] saltBytes = Convert.FromBase64String(salt);  // Convert the salt string to a byte array
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000);
                byte[] passwordBytes = pbkdf2.GetBytes(20);
                return Convert.ToBase64String(passwordBytes);  // Convert the hashed password to a string
            }

        }
        public class CheckRights
        {
            static string folderPath = cDataBase.DbPath;
            static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
            public bool CanEdit(string username)
            {
                string userRights = null;
                string canEditString;
                bool canEdit = false;
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    // SQL-Abfrage erstellen
                    string sql = "SELECT canEdit,Rights FROM Passwort WHERE Username = @username";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        // Benutzernamen als Parameter hinzufügen, um SQL-Injection zu vermeiden
                        cmd.Parameters.AddWithValue("@username", username);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                canEditString = reader.GetString(0);
                                userRights = reader.GetString(1);
                                canEdit = canEditString == "1" ? true : false;
                            }
                        }
                    }
                    conn.Close();
                }

                // Überprüft, ob PIN mit dem gespeicherten PIN übereinstimmt
                return canEdit ? true : false;

            }
            public string rightCheck(string username)
            {
                string userRights = null;
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    // SQL-Abfrage erstellen
                    string sql = "SELECT Rights FROM Passwort WHERE Username = @username";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        // Benutzernamen als Parameter hinzufügen, um SQL-Injection zu vermeiden
                        cmd.Parameters.AddWithValue("@username", username);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Salz, gehashtes Passwort und Rolle aus der Datenbank abrufen
                                userRights = reader.GetString(0);
                            }
                        }
                    }
                    conn.Close();
                }

                // Überprüft, ob PIN mit dem gespeicherten PIN übereinstimmt
                return userRights;
            }
            public bool AuthenticateUser(string username, string pin)
            {
                string userRights = null;
                string userPin = null;
                bool canEdit = false;
                // Verbindung zur Datenbank herstellen
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    // SQL-Abfrage erstellen
                    string sql = "SELECT canEdit,PIN,Rights FROM Passwort WHERE Username = @username";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        // Benutzernamen als Parameter hinzufügen, um SQL-Injection zu vermeiden
                        cmd.Parameters.AddWithValue("@username", username);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Salz, gehashtes Passwort und Rolle aus der Datenbank abrufen
                                canEdit = reader.GetString(0) == "1";
                                userPin = reader.GetString(1);
                                userRights = reader.GetString(2);
                            }
                        }
                    }
                    conn.Close();
                }

                // Überprüft, ob PIN mit dem gespeicherten PIN übereinstimmt

                return canEdit ? pin == userPin ? true : false : userRights == "admin" ? true : false;
            }
        }
        public class Login
        {
            static string folderPath = cDataBase.DbPath;
            static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
            public string AuthenticateUser(string username, string password)
            {
                string storedSalt = "";
                string storedHashedPassword = "";
                string userRights = null;

                // Verbindung zur Datenbank herstellen
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    // SQL-Abfrage erstellen
                    string sql = "SELECT Salt, HashedPassword, Rights FROM Passwort WHERE Username = @username";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        // Benutzernamen als Parameter hinzufügen, um SQL-Injection zu vermeiden
                        cmd.Parameters.AddWithValue("@username", username);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Salz, gehashtes Passwort und Rolle aus der Datenbank abrufen
                                storedSalt = reader.GetString(0);
                                storedHashedPassword = reader.GetString(1);
                                userRights = reader.GetString(2);
                            }
                        }
                    }
                    conn.Close();
                }

                // Hash das eingegebene Passwort mit dem gespeicherten Salz
                string hashedPassword = HashPasswordWithSalt(password, storedSalt);

                // Überprüft, ob das gehashte Passwort mit dem gespeicherten Passwort übereinstimmt
                return hashedPassword == storedHashedPassword ? userRights : null;
            }

            private static string HashPasswordWithSalt(string password, string salt)
            {
                byte[] saltBytes = Convert.FromBase64String(salt);  // Convert the salt string to a byte array
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000);
                byte[] passwordBytes = pbkdf2.GetBytes(20);
                return Convert.ToBase64String(passwordBytes);  // Convert the hashed password to a string
            }
            public bool IsDatabaseEmpty()
            {
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Passwort WHERE Rights IS 'admin'", conn))
                    {
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count == 0;
                    }
                }
            }
        }
        public class UserManagement
        {
            static string folderPath = cDataBase.DbPath;
            static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
            public List<string> GetUsernames()
            {
                List<string> usernames = new List<string>();

                using (var connection = new SQLiteConnection(stConnectionString))
                {
                    connection.Open();
                    var command = new SQLiteCommand("SELECT Username FROM Passwort", connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usernames.Add(reader.GetString(0));
                        }
                    }
                }

                return usernames;
            }

            public bool ChangeUserRights(string adminUsername, string targetUsername, string newRights)
            {
                // Verify the requesting user is an admin
                if (!IsAdmin(adminUsername))
                {
                    MessageBox.Show("Only admin users can change user rights.");
                    return false;
                }
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand("UPDATE Passwort SET Rights = @rights WHERE Username = @username", conn))
                    {
                        cmd.Parameters.AddWithValue("@username", targetUsername);
                        cmd.Parameters.AddWithValue("@rights", newRights);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }

            public bool ChangeUserPassword(string adminUsername, string targetUsername, string newPassword)
            {
                if (!IsAdmin(adminUsername))
                {
                    MessageBox.Show("Only admin users can change user passwords.");
                    return false;
                }

                string newSalt = GenerateSalt();
                string newHashedPassword = HashPasswordWithSalt(newPassword, newSalt);
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand("UPDATE Passwort SET HashedPassword = @password, Salt = @salt WHERE Username = @username", conn))
                    {
                        cmd.Parameters.AddWithValue("@username", targetUsername);
                        cmd.Parameters.AddWithValue("@password", newHashedPassword);
                        cmd.Parameters.AddWithValue("@salt", newSalt);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }

            public bool ChangeUserPin(string adminUsername, string username, string newPin)
            {
                if (!IsAdmin(adminUsername))
                {
                    MessageBox.Show("Only admin users can change user passwords.");
                    return false;
                }
                if (newPin.Length != 4 || !int.TryParse(newPin, out _))
                {
                    MessageBox.Show("The PIN must be a four-digit number.");
                    return false;
                }

                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand("UPDATE Passwort SET PIN = @pin WHERE Username = @username", conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@pin", newPin);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }

            private static string GenerateSalt()
            {
                byte[] saltBytes = new byte[32];
                using (var provider = new RNGCryptoServiceProvider())
                {
                    provider.GetBytes(saltBytes);
                }

                return Convert.ToBase64String(saltBytes);
            }

            private static string HashPasswordWithSalt(string password, string salt)
            {
                byte[] saltBytes = Convert.FromBase64String(salt);  // Convert the salt string to a byte array
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000);
                byte[] passwordBytes = pbkdf2.GetBytes(20);
                return Convert.ToBase64String(passwordBytes);  // Convert the hashed password to a string
            }

            private bool IsAdmin(string username)
            {
                string userRights = null;
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();
                    string sql = "SELECT Rights FROM Passwort WHERE Username = @username";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userRights = reader.GetString(0);
                            }
                        }
                    }
                    conn.Close();
                }
                return userRights == "admin";
            }
            public bool DeleteUser(string targetUsername)
            {
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand("DELETE FROM Passwort WHERE Username = @username", conn))
                    {
                        cmd.Parameters.AddWithValue("@username", targetUsername);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }

        }
    }
}
