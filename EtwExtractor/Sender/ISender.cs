
namespace EtwExtractor.Sender
{
    public interface ISender<T> : IDisposable
    {
        void Send(T input);

        Task SendAsync(T input);

        Task RunAsync(CancellationToken ct );
    }
}
