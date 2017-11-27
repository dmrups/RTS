using System;
using System.Collections.Generic;
using System.Linq;
using Server.Entities;
using Server.Interfaces;
using Server.Interfaces.Entities;
using Server.Units;

namespace Server
{
    internal class EntityFactory : IEntityFactory
    {
        private readonly IConfigurationLoader entitiesConfiguration;
        public EntityFactory(IConfigurationLoader entitiesConfiguration)
        {
            this.entitiesConfiguration = entitiesConfiguration;
        }

        public IEnumerable<T> BuildUnits<T>() where T : IUnit
        {
            var loadedEntity = entitiesConfiguration.LoadUnit<T>();
            yield return loadedEntity;
            while (true)
            {
                yield return (T)loadedEntity.Clone();
            }
        }

        public IGame BuildGame(string name)
        {
            var map = entitiesConfiguration.LoadMap(name);
            var mode = entitiesConfiguration.LoadGameMode(name);

            var result = new Game();
            result.Map = map;
            result.TicksPerSecond = mode.TicksPerSecond;
            result.NumberOfPlayers = mode.NumberOfPlayers;
            var players = new List<IPlayer>();
            var units = new List<IUnit>();
            var unitIndex = 0;

            for (int i = 0; i < result.NumberOfPlayers; i++)
            {
                var player = new Player { Id = i };
                players.Add(player);
                foreach (var unitType in mode.Units)
                {
                    switch (unitType.Key)
                    {
                        case nameof(Mariner):
                            foreach (var unit in BuildUnits<Mariner>().Take(unitType.Value))
                            {
                                unit.Id = unitIndex++;
                                unit.PlayerId = player.Id;
                                unit.Position = map.GetInitialPosition(result.NumberOfPlayers, i);
                                unit.Health = unit.MaxHealth;
                                units.Add(unit);
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Unit type is unknown");
                    }
                }
            }

            result.Players = players;
            result.Units = units;

            return result;
        }
    }
}