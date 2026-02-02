namespace PensumAIHjelpeOppgave.Events;

public void Log(string type, object payload)
{
    using var conn = _db.GetConnection();
    using var cmd = conn.CreateCommand();

    cmd.CommandText = """
        INSERT INTO Events (Type, Payload, CreatedAt)
        VALUES ($type, $payload, $time)
    """;

    cmd.Parameters.AddWithValue("$type", type);
    cmd.Parameters.AddWithValue("$payload", JsonSerializer.Serialize(payload));
    cmd.Parameters.AddWithValue("$time", DateTime.UtcNow);

    cmd.ExecuteNonQuery();
}
