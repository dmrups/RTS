using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Server.Interfaces.Entities;
using Server.Units;

namespace Server.Entities
{
    public class Game : IGame
    {
        public int Id { get; set; }
        public int Tick { get; set; }
        public IMap Map { get; set; }
        public List<IUnit> Units { get; set; }
        public IEnumerable<IPlayer> Players { get; set; }
        public int TicksPerSecond { get; set; }
        public int NumberOfPlayers { get; set; }

        public Dictionary<int, float> CalculateScore()
        {
            var result = new Dictionary<int, float>();

            foreach (var player in Players)
            {
                player.Score += player.Kills.Sum(x => x.MaxHealth);
                result.Add(player.Id, player.Score);
            }

            return result;
        }
    }
}