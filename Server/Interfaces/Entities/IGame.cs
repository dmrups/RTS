using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Server.Units;

namespace Server.Interfaces.Entities
{
    public interface IGame
    {
        int Id { get; set; }
        int Tick { get; set; }
        IMap Map { get; set; }
        List<IUnit> Units { get; set; }
        IEnumerable<IPlayer> Players { get; set; }
        int TicksPerSecond { get; set; }

        Dictionary<int, float> CalculateScore();
    }
}