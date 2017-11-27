using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Server.Communication.Messages;
using Server.Interfaces.Entities;
using Server.Interfaces;
using Server.Entities;

namespace Server.Communication
{
    internal class Hub : IHub
    {
        private object lockObject = new object();
        private HashSet<Client> newClients { get; set; } = new HashSet<Client>();
        private HashSet<Client> processedClients { get; set; } = new HashSet<Client>();
        private HashSet<Client> guiClients { get; set; } = new HashSet<Client>();

        public event EventHandler OnNewClientRegistered;

        public async Task RegisterClientAsync(WebSocket webSocket)
        {
            var client = new Client(webSocket);

            var msgReg = await client.RecieveMessage<MsgRegister>();
            if (!msgReg.Type.HasValue) throw new ArgumentNullException(nameof(msgReg.Type));
            client.ClientType = msgReg.Type.Value;
            if (msgReg.Type == ClientType.Strategy)
            {
                client.Name = msgReg.Name;
                newClients.Add(client);
                if (OnNewClientRegistered != null) OnNewClientRegistered(this, EventArgs.Empty);
            }
            else
            {
                guiClients.Add(client);
            }

            client.WaitUntilSocketClosed();
        }

        public bool TryGetClients(ClientType type, int count, out IEnumerable<Client> clients)
        {
            clients = null;

            lock (lockObject)
            {
                var clientsQuery = newClients.Where(c => c.ClientType == type);
                if (count > clientsQuery.Count()) return false;

                var result = new List<Client>();
                foreach (var client in clientsQuery.Take(count).ToArray())
                {
                    newClients.Remove(client);
                    result.Add(client);
                    processedClients.Add(client);
                }

                clients = result;
                return true;
            }
        }

        public void NotifyGuiClients(object sender, Message e)
        {
            guiClients.TrySendMessages(e);
        }
    }
}