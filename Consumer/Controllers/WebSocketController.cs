using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using Consumer.Services;
using EtwDashboard.Services;

namespace Consumer.Controllers
{
    [ApiController]
    public class WebSocketController : Controller
    {
        private readonly WebSocketService _wsService;

        public WebSocketController(WebSocketService wsService)
        {
            _wsService = wsService;
        }

        [HttpGet("/ws")]
        public async Task Connect()
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                HttpContext.Response.StatusCode = 400;
                return;
            }

            var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var id = Guid.NewGuid();

            _wsService.Add(id, socket);

            Console.WriteLine("Connected!");

            var buffer = new byte[1024];
            while (socket.State == WebSocketState.Open)
            {
                await socket.ReceiveAsync(buffer, CancellationToken.None);
            }

            _wsService.Remove(id);
        }
    }
}
