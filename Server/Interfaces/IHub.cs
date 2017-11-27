using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Server.Communication;
using Server.Communication.Messages;
using Server.Interfaces.Entities;

namespace Server.Interfaces
{
    internal interface IHub
    {
        event EventHandler OnNewClientRegistered;
        bool TryGetClients(ClientType type, int count, out IEnumerable<Client> clients);

        Task RegisterClientAsync(WebSocket webSocket);

        void NotifyGuiClients(object sender, Message e);
    }
}