using System;
using System.Collections.Generic;
using System.Numerics;

namespace Server.Interfaces.Entities
{
    public interface IMap
    {
        Vector2 Size { get; set; }

        Vector2 GetInitialPosition(int numberOfPlayers, int playerNumber);
    }
}