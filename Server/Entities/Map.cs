using System;
using System.Collections.Generic;
using System.Numerics;
using Server.Interfaces.Entities;

namespace Server.Entities
{
    public class Map : IMap
    {
        public Vector2 Size { get; set; }

        public Vector2 GetInitialPosition(int numberOfPlayers, int playerNumber)
        {
            if (numberOfPlayers != 2) throw new NotImplementedException();

            return new Vector2
            {
                X = Size.X * 0.5F,
                Y = playerNumber * Size.Y * 0.9F + Size.Y * 0.05F
            };
        }
    }
}