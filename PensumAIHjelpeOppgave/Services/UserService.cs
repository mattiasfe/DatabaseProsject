using Backend.Models;
using PensumAIHjelpeOppgave.Data;
using PensumAIHjelpeOppgave.Models;
using Microsoft.Data.Sqlite;

namespace PensumAIHjelpeOppgave.Services;

public class UserService
{
    private readonly Database _db;

    public UserService(Database db)
    {
        _db = db;
    }

    public List<User> GetAll()
    {
        using var conn = _db.GetConnection();
        conn.Open(); // VIKTIG: Forbindelsen må åpnes
        
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Email, Name, CreatedAt FROM Users";

        using var reader = cmd.ExecuteReader();
        var users = new List<User>();

        while (reader.Read())
        {
            users.Add(new User(
                reader.GetInt64(0),
                reader.GetString(1),
                reader.GetString(2),
                DateTime.Parse(reader.GetString(3))
            ));
        }

        return users;
    }

    public void Create(CreateUserDto dto)
    {
        using var conn = _db.GetConnection();
        conn.Open(); // VIKTIG: Forbindelsen må åpnes
        
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
                          INSERT INTO Users (Email, Name, CreatedAt)
                          VALUES ($email, $name, $createdAt)
                          """;

        cmd.Parameters.AddWithValue("$email", dto.Email);
        cmd.Parameters.AddWithValue("$name", dto.Name);
        cmd.Parameters.AddWithValue("$createdAt", DateTime.UtcNow.ToString("O")); // ISO 8601 format er tryggest for SQLite

        cmd.ExecuteNonQuery();
    }
}