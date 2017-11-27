using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Communication;
using Server.Communication.Messages;

namespace Server
{
    static class Extensions
    {
        public static void SendMessages(this IEnumerable<Client> clients, Message msg)
        {
            Task.WaitAll(clients.Select(async client => await client.SendMessage(msg)).ToArray());
        }

        public static void TrySendMessages(this IEnumerable<Client> clients, Message msg)
        {
            foreach (var client in clients)
            {
                client.TrySendMessage(msg);
            }
        }

        public static IDictionary<int, T> RecieveMessages<T>(this IEnumerable<Client> clients) where T : Message
        {
            var messages = new ConcurrentDictionary<int, T>();
            Task.WaitAll(
                clients.Select(async client =>
                    messages.GetOrAdd(client.PlayerId, await client.RecieveMessage<T>())
                ).ToArray());

            return messages;
        }
    }
}