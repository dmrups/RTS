using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Server.Communication.Messages;
using Server.Interfaces.Entities;

namespace Server.Communication
{
    internal class Client
    {
        private readonly WebSocket webSocket;
        private ManualResetEvent mrse = new ManualResetEvent(false);
        private object lockObject = new object();
        private object isSending = null;
        private object isRecieving = null;

        public ClientType ClientType { get; set; }
        public int PlayerId { get; set; }
        public string Name { get; set; }

        public Client(WebSocket webSocket)
        {
            this.webSocket = webSocket;
        }

        public async Task<T> RecieveMessage<T>() where T : Message
        {
            if (Interlocked.Exchange(ref isRecieving, lockObject) != null)
                throw new InvalidOperationException("Socket busy");

            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.CloseStatus.HasValue)
            {
                await CloseConnection(result.CloseStatus.Value, result.CloseStatusDescription);
                throw new NotImplementedException();
            }
            else
            {
                var str = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var message = JsonConvert.DeserializeObject<T>(str);
                Console.WriteLine(str);
                Interlocked.Exchange(ref isRecieving, null);
                return message;
            }
        }

        internal void WaitUntilSocketClosed()
        {
            mrse.WaitOne();
        }

        public async Task CloseConnection(WebSocketCloseStatus status = WebSocketCloseStatus.NormalClosure, string description = null)
        {
            await webSocket.CloseAsync(status, description, CancellationToken.None);
            mrse.Set();
        }

        public async Task SendMessage(Message message)
        {
            if (Interlocked.Exchange(ref isSending, lockObject) != null)
                throw new InvalidOperationException("Socket busy");

            await SendUnsafe(message);

            Interlocked.Exchange(ref isSending, null);
        }

        public async Task<bool> TrySendMessage(Message message)
        {
            if (Interlocked.Exchange(ref isSending, lockObject) != null) return false;

            await SendUnsafe(message);

            Interlocked.Exchange(ref isSending, null);

            return true;
        }

        private async Task SendUnsafe(Message msg)
        {
            var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg,
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));

            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}