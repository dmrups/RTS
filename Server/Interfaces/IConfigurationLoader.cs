using Server.Interfaces.Entities;

namespace Server.Interfaces
{
    internal interface IConfigurationLoader
    {
        T LoadUnit<T>() where T : IUnit;
        IMap LoadMap(string name);
        GameMode LoadGameMode(string name);
    }
}