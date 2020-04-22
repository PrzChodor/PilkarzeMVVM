using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Windows;

namespace PilkarzeMVVM.Model
{
    class PlayersData
    {
        public List<Player> PlayersList { get; set; }

        public PlayersData()
        {
            PlayersList = new List<Player>();
            LoadData("players.txt");
        }

        public void LoadData(string path)
        {
            if (File.Exists(path))
            {
                string jsonString = File.ReadAllText(path);
                PlayersList = JsonSerializer.Deserialize<List<Player>>(jsonString);
            }
        }

        public void SaveData(string path)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            string jsonString = JsonSerializer.Serialize(PlayersList,options);
            File.WriteAllText(path, jsonString);
        }
    }
}
