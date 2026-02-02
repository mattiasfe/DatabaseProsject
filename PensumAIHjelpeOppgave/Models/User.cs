namespace PensumAIHjelpeOppgave.Models;

public record User(
    long Id,
    string Email,
    string Name,
    DateTime CreatedAt
);