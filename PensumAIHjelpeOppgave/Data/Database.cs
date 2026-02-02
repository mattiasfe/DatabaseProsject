using Microsoft.Data.Sqlite;

namespace Backend.Data;

public class Database
{
    private readonly string _connectionString = "Data Source=app.db";

    public SqliteConnection GetConnection()
    {
        var conn = new SqliteConnection(_connectionString);
        conn.Open();
        return conn;
    }
}