using Consumer.Models;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace EtwDashboard.Services
{

    public class WebSocketService
    {
        private readonly ConcurrentDictionary<Guid, WebSocket> _sockets = new();
        private readonly ILogger<WebSocketService> _logger;

        public WebSocketService(ILogger<WebSocketService> logger)
        {
            _logger = logger;
        }

        public void Add(Guid id, WebSocket socket)
        {
            _sockets.TryAdd(id, socket);
        }

        public void Remove(Guid id)
        {
            _sockets.TryRemove(id, out _);
        }

        public async Task BroadcastAsync(EtwRecordEntity evt, CancellationToken ct = default)
        {
            var json = JsonSerializer.Serialize(evt, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            var bytes = Encoding.UTF8.GetBytes(json);
            var segment = new ArraySegment<byte>(bytes);

            foreach (var (id, socket) in _sockets)
            {
                try
                {
                    if (socket.State != WebSocketState.Open)
                    {
                        Remove(id);
                        continue;
                    }

                    await socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send to socket {Id}", id);
                    Remove(id);
                }
            }
        }
    }
}