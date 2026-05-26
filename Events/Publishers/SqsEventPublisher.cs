namespace UsersAPI.Events.Publishers;

using Amazon.SQS;
using Amazon.SQS.Model;
using System.Text.Json;
using UsersAPI.Events.Models;

public class SqsEventPublisher(IAmazonSQS sqs) : IEventPublisher
{
    private readonly IAmazonSQS _sqs = sqs;

    public async Task PublishAsync<T>(
        string queueName,
        T message
    )
    {
        var QUEUE_SQS_BASE_URL = Environment.GetEnvironmentVariable("QUEUE_SQS_BASE_URL");

        if (QUEUE_SQS_BASE_URL == null) 
        { 
            throw new ArgumentNullException("QUEUE_SQS_BASE_URL", "Environment variable QUEUE_SQS_BASE_URL is not set.");
        }

        var queueUrl = $"{QUEUE_SQS_BASE_URL}{queueName}";

        var payload = new
        {
            MessageType = typeof(T).Name,
            Message = message
        };

        await _sqs.SendMessageAsync(new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = JsonSerializer.Serialize(payload)
        });
    }
}
