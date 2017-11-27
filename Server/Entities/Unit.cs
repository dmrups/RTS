using System.Collections.Generic;
using System.Numerics;
using Server.Interfaces.Entities;

namespace Server.Entities
{
    public class Unit : IUnit
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public float MaxHealth { get; set; }
        public float Health { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Destination { get; set; }
        public int? Target { get; set; }
        public float AttackRange { get; set; }
        public float AttackPower { get; set; }
        public float Speed { get; set; }
        public float Size { get; set; }

        public virtual void Move(IMap map)
        {
            if (Destination == null
            || (Position - Destination).Length() <= (Size / 1000))
                return;

            var direction = Destination - Position;

            if (direction.Length() <= Speed)
            {
                Position = Destination;
                return;
            }

            Position = Position + direction * Speed / direction.Length();
        }

        public virtual void Shoot(IDictionary<int, IUnit> unitsDict, IPlayer player)
        {
            if (!Target.HasValue
                    || AttackPower == 0F
                    || !unitsDict.TryGetValue(Target.Value, out var target)
                    || (target.Position - Position).Length() > AttackRange
                    || target.Health <= 0)
            {
                return;
            }

            target.Health -= AttackPower;

            if (target.Health <= 0)
            {
                player.Kills.Add(target);
            }
            else
            {
                player.Score += AttackPower;
            }
        }

        public IUnit Clone()
        {
            return (Unit)this.MemberwiseClone();
        }
    }
}