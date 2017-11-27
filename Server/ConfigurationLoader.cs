using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Server.Entities;
using Server.Interfaces;
using Server.Interfaces.Entities;

namespace Server
{
    internal class ConfigurationLoader : IConfigurationLoader
    {
        private const string mapsSelector = @"\Configuration\Maps";
        private const string gamesSelector = @"\Configuration\Games";
        private const string unitsSelector = @"\Configuration\Units";
        private Dictionary<string, string> mapFiles;
        private Dictionary<string, string> unitFiles;
        private Dictionary<string, string> gameFiles;


        public ConfigurationLoader()
        {
            mapFiles = Directory.EnumerateFiles(mapsSelector)
                .ToDictionary(path => path.Split(@"/").Last().Split(".").First(), path => File.ReadAllText(path));

            unitFiles = Directory.EnumerateFiles(gamesSelector)
                .ToDictionary(path => path.Split(@"/").Last().Split(".").First(), path => File.ReadAllText(path));

            gameFiles = Directory.EnumerateFiles(unitsSelector)
                .ToDictionary(path => path.Split(@"/").Last().Split(".").First(), path => File.ReadAllText(path));
        }

        public T LoadUnit<T>() where T : IUnit
        {
            return JsonConvert.DeserializeObject<T>(unitFiles[nameof(T)]);
        }

        public IMap LoadMap(string name)
        {
            return JsonConvert.DeserializeObject<Map>(unitFiles[name]);
        }

        public GameMode LoadGameMode(string name)
        {
            return JsonConvert.DeserializeObject<GameMode>(unitFiles[name]);
        }
    }
}