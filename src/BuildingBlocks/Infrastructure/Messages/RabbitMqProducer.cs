using System.Text;
using Contracts.Common.Interfaces;
using Contracts.Messages;
using RabbitMQ.Client;

namespace Infrastructure.Messages;

public class RabbitMqProducer : IMessageProducer
{
    private readonly ISerializeService _serializeService;

    public RabbitMqProducer(ISerializeService serializeService)
    {
        _serializeService = serializeService;
    }

    public void SendMessage<T>(T message)
    {
        var connectionFactory = new ConnectionFactory()
        {
            HostName = "localhost"
        };

        var connection = connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare("orders", exclusive: false);

        var jsonData = _serializeService.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonData);

        channel.BasicPublish(exchange: "", routingKey: "orders", body: body);
    }
}