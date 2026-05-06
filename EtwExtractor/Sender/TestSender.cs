using EtwExtractor.Reader;
using System.Reflection.PortableExecutable;

namespace EtwExtractor.Sender
{
    public class TestSender : ISender<EtwRecordEntity>
    {
        private IReader<EtwRecordEntity> reader;
        private bool disposed;

        public TestSender(IReader<EtwRecordEntity> reader)
        {
            this.reader = reader;
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

        public void Send(EtwRecordEntity input)
        {
            Console.WriteLine($"{input.Id}\t{input.EventName}\t{input.ProcessName}\t{input.Time}");
        }

        public Task SendAsync(EtwRecordEntity input)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            reader.Dispose();
            disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
