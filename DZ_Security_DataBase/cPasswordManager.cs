using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace DZ_Security_DataBase
{
    internal class cPasswordManager
    {
        public class Registration
        {
            static string folderPath = cDataBase.DbPath;
            static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
            public void RegisterUser(string username, string password)
            {
                // Generate a new salt
                string salt = GenerateSalt();

                // Hash the password with the salt
                string hashedPassword = HashPasswordWithSalt(password, salt);

                // Now store the username, hashed password and salt in the database
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand("INSERT INTO Passwort (Username, HashedPassword, Salt) VALUES (@username, @password, @salt)", conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.Parameters.AddWithValue("@salt", salt);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            private static string GenerateSalt()
            {
                byte[] saltBytes = new byte[32];
                using (var provider = new RNGCryptoServiceProvider())
                {
                    provider.GetNonZeroBytes(saltBytes);
                }

                return Convert.ToBase64String(saltBytes);
            }

            private static string HashPasswordWithSalt(string password, string salt)
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    var saltedPassword = $"{salt}{password}";
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));

                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }
        }

        public class Login
        {
            static string folderPath = cDataBase.DbPath;
            static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
            public bool AuthenticateUser(string username, string password)
            {
                string storedSalt = "";
                string storedHashedPassword = "";

                // Verbindung zur Datenbank herstellen
                using (var conn = new SQLiteConnection(stConnectionString))
                {
                    conn.Open();

                    // SQL-Abfrage erstellen
                    string sql = "SELECT Salt, HashedPassword FROM Passwort WHERE Username = @username";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        // Benutzernamen als Parameter hinzufügen, um SQL-Injection zu vermeiden
                        cmd.Parameters.AddWithValue("@username", username);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Salz und gehashtes Passwort aus der Datenbank abrufen
                                storedSalt = reader.GetString(0);
                                storedHashedPassword = reader.GetString(1);
                            }
                        }
                    }
                    conn.Close();
                }

                // Hash das eingegebene Passwort mit dem gespeicherten Salz
                string hashedPassword = HashPasswordWithSalt(password, storedSalt);

                // Überprüft, ob das gehashte Passwort mit dem gespeicherten Passwort übereinstimmt
                return hashedPassword == storedHashedPassword;
            }


            private static string HashPasswordWithSalt(string password, string salt)
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    var saltedPassword = $"{salt}{password}";
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));

                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }
        }



    }
}
