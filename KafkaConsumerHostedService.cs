using System.Text;
using Microsoft.Extensions.Hosting;
using Kafka.Public;
using Kafka.Public.Loggers;

namespace ConsoleApp1;

public class KafkaConsumerHostedService : IHostedService
{
    private readonly ClusterClient _cluster;
    private readonly Action<string> _onMessageReceived;

    public KafkaConsumerHostedService(string groupId, IEnumerable<string> topics, Action<string> onMessageReceived)
    {
        _onMessageReceived = onMessageReceived;
        _cluster = new ClusterClient(new Configuration
        {
            Seeds = "localhost:9092",
            
        }, new ConsoleLogger());
        
        _cluster.Subscribe(groupId, topics, new ConsumerGroupConfiguration());
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cluster.MessageReceived += record =>
        {
            var message = Encoding.UTF8.GetString(record.Value as byte[] ?? Array.Empty<byte>());
            _onMessageReceived(message);
        };

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cluster?.Dispose();
        return Task.CompletedTask;
    }
}
