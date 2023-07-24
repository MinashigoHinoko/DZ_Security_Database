using System.Data.SQLite;

namespace Festival_Manager
{
    internal class cLogger
    {
        static string folderPath = cDataBase.DbPath;
        static string stConnectionString = $"Data Source={folderPath}\\Dz_Security.sqlite;Version=3;";
        static public void LogDatabaseChange(string action, string username)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logEntry = $"[{timestamp}] {username} hat eine Aktion ausgeführt: {action}";

            using (var conn = new SQLiteConnection(stConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
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
