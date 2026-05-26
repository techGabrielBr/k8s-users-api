namespace UsersAPI.Events.Models
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(string queueName, T message);
    }
}