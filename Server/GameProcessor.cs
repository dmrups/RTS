using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using Server.Communication;
using Server.Communication.Messages;
using Server.Interfaces.Entities;
using Server.Interfaces;
//using Server.Entities;

namespace Server
{
    internal class GameProcessor : IGameProcessor
    {
        private IGame currentGame;
        private IEnumerable<Client> clients;
        private readonly IEntityFactory entityFactory;

        public event EventHandler<MsgGameState> OnTick;
        public event EventHandler<MsgGameFinish> OnGameFinished;

        public GameProcessor(IEntityFactory entityFactory)
        {
            this.entityFactory = entityFactory;
        }

        public GameProcessor(IEnumerable<Client> clients)
        {
            currentGame = entityFactory.BuildGame("Test");
            foreach (var client in clients)
            {
                client.PlayerId = player.Id;
                player.Name = client.Name;
                await client.SendMessage(new MsgRegisterComplete(client.Player.Id);
            }
            clients.Zip(currentGame.Players, (client, player) => );//.ToArray();
            clients.Zip(currentGame.Players, (client, player) => );//.ToArray();
            );

            this.clients = clients;
        }

        public void Start()
        {
            for (var i = 0; i < int.MaxValue; i++)
            {
                if (OnTick != null) OnTick(this, new MsgGameState(currentGame));

                clients.SendMessages(new MsgGameState(currentGame));

                var responses = clients.RecieveMessages<MsgGameState>();
                if (Process(responses.ToDictionary(x => x.Key, x => x.Value.Game)) == GameState.Finished)
                {
                    break;
                }
            }

            var lastMessage = new MsgGameFinish(currentGame.CalculateScore());
            if (OnGameFinished != null) OnGameFinished(this, lastMessage);
            clients.SendMessages(lastMessage);
        }

        private GameState Process(IDictionary<int, IGame> gamesDict)
        {
            MergeGames(gamesDict);

            var unitsDict = currentGame.Units.ToDictionary(x => x.Id);
            var playersDict = currentGame.Players.ToDictionary(x => x.Id);

            foreach (var unit in currentGame.Units)
            {
                unit.Shoot(unitsDict, playersDict[unit.PlayerId]);
            }

            currentGame.Units.RemoveAll(x => x.Health <= 0);

            if (currentGame.Units.GroupBy(x => x.PlayerId).Count() < gamesDict.Count)
            {
                return GameState.Finished;
            }

            foreach (var unit in currentGame.Units)
            {
                unit.Move(currentGame.Map);
            }

            currentGame.Tick++;

            return GameState.Started;
        }

        private void MergeGames(IDictionary<int, IGame> gamesDict)
        {
            IEnumerable<IUnit> units = Enumerable.Empty<IUnit>();
            foreach (var keyValue in gamesDict)
            {
                units = units.Union(keyValue.Value.Units.Where(x => x.PlayerId == keyValue.Key));
            }

            var query = from currentGameUnit in currentGame.Units
                        join processedUnit in units on currentGameUnit.Id equals processedUnit.Id
                        select new { currentGameUnit, processedUnit };

            foreach (var tuple in query)
            {
                tuple.currentGameUnit.Destination = tuple.processedUnit.Destination;
                tuple.currentGameUnit.Target = tuple.processedUnit.Target;
            }
        }
    }
}