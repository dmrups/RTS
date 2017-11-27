using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Communication;
using Server.Interfaces;

namespace Server
{
    internal class GameService : IGameService
    {
        private readonly IHub hub;

        public HashSet<GameProcessor> ActiveGames { get; set; } = new HashSet<GameProcessor>();

        public GameService(IHub connectionHub)
        {
            hub = connectionHub;
            hub.OnNewClientRegistered += TryFormNewGame;
        }

        private void TryFormNewGame(object sender, EventArgs e)
        {
            if (hub.TryGetClients(ClientType.Strategy, 2, out var clients))
            {
                StartNewGame(clients);
            }
        }

        private void StartNewGame(IEnumerable<Client> clients)
        {
            var processor = new GameProcessor(clients);

            processor.OnTick += hub.NotifyGuiClients;
            processor.OnGameFinished += hub.NotifyGuiClients;
            ActiveGames.Add(processor);
            new Task(() => processor.Start()).Start();
        }
    }
}