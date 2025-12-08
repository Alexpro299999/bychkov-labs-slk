using Microsoft.Data.Sqlite;

namespace WatchShop.DataAccess
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