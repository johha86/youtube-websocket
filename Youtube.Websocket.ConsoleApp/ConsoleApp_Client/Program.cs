using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Count() < 2)
            {
                Process.Start("ConsoleApp_Client.exe");
            }

            StartWebSocket().GetAwaiter().GetResult();
        }

        public static async Task StartWebSocket()
        {
            var client = new ClientWebSocket();

            //  if the web service was executed using http protocol from Visual Studio
            await client.ConnectAsync(new Uri("ws://localhost:57835/ws"), CancellationToken.None);
            //   uncomment the following lines if the web service was executed from consulo using 'dotnet run' command
            //await client.ConnectAsync(new Uri("ws://localhost:50000/ws"), CancellationToken.None);

            Console.WriteLine($"Connection web socket established @ {DateTime.UtcNow:F}");
            var send = Task.Run(
                async () =>
                {
                    string message;
                    while ((message = Console.ReadLine()) != null && message != string.Empty)
                    {
                        var bytes = Encoding.UTF8.GetBytes(message);
                        await client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                    }

                    await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }
            );

            var receive = ReceiveAsync(client);
            await Task.WhenAll(send, receive);
        }

        public static async Task ReceiveAsync(ClientWebSocket client)
        {
            var buffer = new byte[1024 * 4];
            while (true)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, result.Count));
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    break;
                }
            }
        }
    }
}
