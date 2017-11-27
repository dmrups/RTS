using System.Collections.Generic;
using System.Numerics;

namespace Server.Interfaces.Entities
{
    public interface IUnit
    {
        int Id { get; set; }
        int PlayerId { get; set; }
        float MaxHealth { get; set; }
        float Health { get; set; }
        Vector2 Position { get; set; }
        Vector2 Destination { get; set; }
        int? Target { get; set; }
        float AttackRange { get; set; }
        float AttackPower { get; set; }
        float Speed { get; set; }
        float Size { get; set; }

        void Move(IMap map);
        void Shoot(IDictionary<int, IUnit> unitsDict, IPlayer player);
        IUnit Clone();
    }
}