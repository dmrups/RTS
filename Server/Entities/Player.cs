using System.Collections.Generic;
using Server.Interfaces.Entities;

namespace Server.Entities
{
    public class Player : IPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<IUnit> Kills { get; set; } = new List<IUnit>();
        public float Score { get; set; }
    }
}