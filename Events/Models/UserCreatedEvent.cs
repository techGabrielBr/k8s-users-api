namespace Events.Models;

public record UserCreatedEvent
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; } 
}