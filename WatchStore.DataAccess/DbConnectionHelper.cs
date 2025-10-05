using Microsoft.Data.Sqlite;

namespace WatchStore.DataAccess
{
    public static class DbConnectionHelper
    {
        public static string ConnectionString = "Data Source=WatchStore.db";

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(ConnectionString);
        }
    }
}