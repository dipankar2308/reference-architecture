using Application.Ports;
using Azure.Messaging.ServiceBus;
using System.Text.Json;

namespace Infrastructure.Messaging;

public class ServiceBusEventPublisher : IEventPublisher
{
    private readonly ServiceBusSender _sender;

    public ServiceBusEventPublisher(ServiceBusClient client, string topicName)
        => _sender = client.CreateSender(topicName);

    public async Task PublishAsync<T>(T domainEvent, CancellationToken ct = default) where T : class
    {
        var message = new ServiceBusMessage(JsonSerializer.Serialize(domainEvent))
        {
            Subject = typeof(T).Name,
            ContentType = "application/json",
            MessageId = Guid.NewGuid().ToString()
        };
        await _sender.SendMessageAsync(message, ct);
    }
}
