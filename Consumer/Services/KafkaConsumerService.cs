using Confluent.Kafka;
using Consumer.Models;
using EtwDashboard.Services;
using System.Text.Json;

namespace Consumer.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly WebSocketService _wsManager;
        private readonly ElasticsearchService _esService;
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly string _topic;

        public KafkaConsumerService(
            IConfiguration configuration,
            WebSocketService wsManager,
            ElasticsearchService esService,
            ILogger<KafkaConsumerService> logger)
        {
            _wsManager = wsManager;
            _esService = esService;
            _logger = logger;

            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "",
                GroupId = configuration["Kafka:GroupId"] ?? "",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _topic = configuration["Kafka:Topic"] ?? "";
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                _consumer.Subscribe(_topic);

                while (!stoppingToken.IsCancellationRequested)
                {
                    ConsumeResult<string, string>? result = null;
                    try
                    {
                        result = _consumer.Consume(stoppingToken);

                        if (result?.Message.Value is null)
                        {
                            continue;
                        }

                        var evt = JsonSerializer.Deserialize<EtwRecordEntity>(result.Message.Value);

                        if (evt is null)
                        {
                            continue;
                        }

                        await Task.WhenAll(
                            _wsManager.BroadcastAsync(evt, stoppingToken),
                            _esService.IndexAsync(evt, stoppingToken)
                        );
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError(ex, "Failed to consumer on partition {Partition}", result?.Partition.Value);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unexpected error processing Kafka message");
                    }
                }

                _consumer.Close();

            }, stoppingToken);
        }

        public override void Dispose()
        {
            _consumer.Dispose();
            base.Dispose();
        }
    }
}
