using Microsoft.Data.Sqlite;
using System.Data.SqlClient;

namespace Parser_Lesegais_ru.DataBase
{
    class DBUtils : DBMSSUtils
    {
        public static SqlConnection GetMSSQLDBConnection()
        {
            string datasource = @"BORMO-PC\SQLEXPRESS";
            string database = "DataBaseForTestTask";

            string username = "sa";
            string password = "111111";

            return DBMSSUtils.GetMSSQLConnection(datasource, database, username, password);
        }

        public static SqliteConnection GetSQLiteDBConnection()
        {
            //local memory
            string datasourse = "Data Source=InMemorySample;Mode=Memory;Cache=Shared";

            return DBMSSUtils.GetSQLiteConnection(datasourse);
        }
    }
    public class DBMSSUtils
    {
        protected static SqlConnection GetMSSQLConnection(string datasource, string database, string username, string password)
        {
            string connString = @"Data Source=" + datasource + ";Initial Catalog="
                        + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

            SqlConnection conn = new SqlConnection(connString);

            return conn;
        }
        protected static SqliteConnection GetSQLiteConnection(string dataSourse)
        {
            SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder();

            builder.DataSource = dataSourse;

            SqliteConnection connection = new SqliteConnection(builder.ConnectionString);

            return connection;
        }
    }
}