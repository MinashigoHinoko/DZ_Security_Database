using System.Security.Cryptography;
using System.Text;

namespace DZ_Security_DataBase
{
    internal class cPasswordManager
    {
        public class Registration
        {
            public void RegisterUser(string username, string password)
            {
                // Generate a new salt
                string salt = GenerateSalt();

                // Hash the password with the salt
                string hashedPassword = HashPasswordWithSalt(password, salt);

                // Now store the username, hashed password and salt in the database
                // ...
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


    }
}
