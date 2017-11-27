using System.Collections.Generic;

namespace Server.Interfaces.Entities
{
    public interface IPlayer
    {
        int Id { get; set; }
        string Name { get; set; }
        List<IUnit> Kills { get; set; }
        float Score { get; set; }
    }
}