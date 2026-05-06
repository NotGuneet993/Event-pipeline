using Confluent.Kafka;
using EtwExtractor.Reader;
using System.Reflection.PortableExecutable;
using System.Text.Json;

namespace EtwExtractor.Sender
{
    public class KafkaSender : ISender<EtwRecordEntity>
    {
        private const string Topic = "etw-events";
        private const int MaxRetries = 3;

        private readonly IProducer<string, string> producer;
        private IReader<EtwRecordEntity> reader;
        private bool disposed;

        public KafkaSender(IReader<EtwRecordEntity> reader)
        {
            this.reader = reader;

            var config = new ProducerConfig
            {
                BootstrapServers = "172.19.180.166:9092",
                Acks = Acks.Leader
            };
            producer = new ProducerBuilder<string, string>(config).Build();
        }

        public void Send(EtwRecordEntity record)
        {
            SendAsync(record).GetAwaiter().GetResult();
        }

        public async Task SendAsync(EtwRecordEntity record)
        {
            var message = new Message<string, string>
            {
                Key = record.EventName,
                Value = JsonSerializer.Serialize(record)
            };

            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    await producer.ProduceAsync(Topic, message);
                    return;
                }
                catch (ProduceException<string, string>)
                {
                    Console.Error.WriteLine("Failed to send data to Kafka.");
                    if (attempt == MaxRetries)
                        return;
                }
            }
        }

        public async Task RunAsync(CancellationToken ct = default)
        {
            await foreach (var batch in reader.ReadAsync(ct))
            {
                foreach (var entity in batch)
                {
                    Send(entity);
                }
            }
        }

        public void Dispose()
        {
            if (disposed) return;
            producer.Flush(TimeSpan.FromSeconds(5));
            producer.Dispose();
            disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
