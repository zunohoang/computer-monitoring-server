using System.Net.WebSockets;
using System.Text;

namespace ComputerMonitoringServerAPI.Services
{
    public static class WebSocketHandler
    {
        public static async Task Handle(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];

            var result = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer),
                CancellationToken.None
            );

            while (!result.CloseStatus.HasValue)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                Console.WriteLine($"[WebSocket] Received: {message}");

                // Echo lại hoặc xử lý message
                var response = Encoding.UTF8.GetBytes($"Server received: {message}");
                await webSocket.SendAsync(
                    new ArraySegment<byte>(response),
                    result.MessageType,
                    result.EndOfMessage,
                    CancellationToken.None
                );

                result = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    CancellationToken.None
                );
            }

            await webSocket.CloseAsync(
                result.CloseStatus.Value,
                result.CloseStatusDescription,
                CancellationToken.None
            );
        }
    }
}
