using System.Collections.Generic;
using Server.Interfaces.Entities;

namespace Server.Interfaces
{
    internal interface IEntityFactory
    {
        IGame BuildGame(string name);
        IEnumerable<T> BuildUnits<T>() where T : IUnit;
    }
}