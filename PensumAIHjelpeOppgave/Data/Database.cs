using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Hosting;

namespace PensumAIHjelpeOppgave.Data;

public sealed class Database
{
    private readonly string _connectionString;

    public Database(IHostEnvironment env)
    {
        var dbFolder = Path.Combine(env.ContentRootPath, "App_Data");
        Directory.CreateDirectory(dbFolder);
        var dbPath = Path.Combine(dbFolder, "app.db");

        var csb = new SqliteConnectionStringBuilder
        {
            DataSource = dbPath,
            Cache = SqliteCacheMode.Shared
        };

        _connectionString = csb.ToString();
    }

    public SqliteConnection GetConnection()
    {
        var conn = new SqliteConnection(_connectionString);
        conn.Open();
        return conn;
    }

    public void EnsureCreated()
    {
        using var conn = GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Email TEXT NOT NULL UNIQUE,
    Name TEXT NOT NULL,
    CreatedAt TEXT NOT NULL
);
CREATE INDEX IF NOT EXISTS IX_Users_Email ON Users(Email);";
        cmd.ExecuteNonQuery();
    }

    // Seede dummy data dersom det mangler brukere
    public void SeedIfEmpty(int targetCount = 10)
    {
        using var conn = GetConnection();

        // Finn eksisterende antall
        using (var countCmd = conn.CreateCommand())
        {
            countCmd.CommandText = "SELECT COUNT(*) FROM Users";
            var existing = Convert.ToInt32(countCmd.ExecuteScalar());
            if (existing >= targetCount) return;

            var toInsert = targetCount - existing;
            using var tx = conn.BeginTransaction();

            var names = new[]
            {
                "Ola Nordmann", "Kari Nordmann", "Per Hansen", "Lise Johansen",
                "Nils Olsen", "Anne Pedersen", "Marius Larsen", "Sara Nilsen",
                "Jonas Berg", "Emma Haugen", "Tobias Lie", "Ingrid Strand"
            };

            using var insertCmd = conn.CreateCommand();
            insertCmd.CommandText = @"
INSERT INTO Users (Email, Name, CreatedAt)
VALUES ($email, $name, $createdAt)";
            var pEmail = insertCmd.CreateParameter();
            pEmail.ParameterName = "$email";
            insertCmd.Parameters.Add(pEmail);

            var pName = insertCmd.CreateParameter();
            pName.ParameterName = "$name";
            insertCmd.Parameters.Add(pName);

            var pCreated = insertCmd.CreateParameter();
            pCreated.ParameterName = "$createdAt";
            insertCmd.Parameters.Add(pCreated);

            var now = DateTime.UtcNow;

            for (int i = 0; i < toInsert; i++)
            {
                var name = names[i % names.Length];
                var slug = name.ToLowerInvariant()
                               .Replace(" ", ".")
                               .Replace("æ", "ae")
                               .Replace("ø", "o")
                               .Replace("å", "a");
                var email = $"{slug}.{existing + i + 1}@example.com"; // unikt pga teller

                pEmail.Value = email;
                pName.Value = name;
                pCreated.Value = now.AddMinutes(-i).ToString("O");

                insertCmd.ExecuteNonQuery();
            }

            tx.Commit();
        }
    }
}