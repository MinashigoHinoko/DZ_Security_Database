using System.Data.SQLite;

namespace Festival_Manager
{
    internal class cLogger
    {
        private static string folderPath = cDataBase.DbPath;
        private static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        public static void LogDatabaseChange(string action, string username)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logEntry = $"[{timestamp}] {username} hat eine Aktion ausgeführt: {action}";

            using (SQLiteConnection conn = new(stConnectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new(conn))
                {
                    cmd.CommandText = @"
            INSERT INTO LogTable (Timestamp, User, Action)
            VALUES (@timestamp, @user, @action)";
                    cmd.Parameters.AddWithValue("@timestamp", timestamp);
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@action", action);

                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
        }

    }
}
