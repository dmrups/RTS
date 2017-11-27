using System.Collections.Generic;

namespace Server
{
    internal class GameMode
    {
        public int TicksPerSecond { get; set; }

        public int NumberOfPlayers { get; set; }

        public Dictionary<string, int> Units { get; set; }

    }
}