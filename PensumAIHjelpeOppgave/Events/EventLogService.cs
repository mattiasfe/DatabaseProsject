using System;
using System.Text.Json;

namespace PensumAIHjelpeOppgave.Events
{
    public sealed class EventLogger
    {
        private readonly PensumAIHjelpeOppgave.Data.Database _db;

        public EventLogger(PensumAIHjelpeOppgave.Data.Database db)
        {
            _db = db;
        }

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
            // Lagre tidspunkt som ISO 8601-streng for SQLite-kompatibilitet
            cmd.Parameters.AddWithValue("$time", DateTime.UtcNow.ToString("O"));

            cmd.ExecuteNonQuery();
        }
    }
}