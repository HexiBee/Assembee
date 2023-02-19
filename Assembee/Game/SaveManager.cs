using Assembee.Game.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Xml.Serialization;

using System.Diagnostics;
using Newtonsoft.Json;
using Assembee.Game.Entities.Tiles;
using System.Security.AccessControl;


namespace Assembee.Game {

    public class SaveData {
        public List<Tile> tiles { get; set; }
        public List<Bee> bees { get; set; }

        public SaveData(List<Bee> bees, List<Tile> tiles) {
            this.tiles = tiles;
            this.bees = bees;
        }
        public SaveData() {

        }
    }

    class SaveManager {
        private static string fileName = "save.bee";

        public SaveData saveData;

        static JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        //World world;

        public SaveManager() : this(new SaveData()) {

        }
        public SaveManager(SaveData saveData) {
            this.saveData = saveData;

        }

        public static void Save(World world) {

            SaveData saveData = GetSaveData(world);


            using (StreamWriter wrt = new StreamWriter(fileName)) {
                wrt.WriteLine(JsonConvert.SerializeObject(saveData, Formatting.Indented, settings));
            }
            //File.Encrypt(fileName);

        }


        public static SaveData GetSaveData(World world) {
            return new SaveData(world.Bees, world.TileList);
        }



        public static bool Load(World world) {
            if (!File.Exists(fileName)) {
                return false; // failure, start new game
            }
            //File.Decrypt(fileName);
            var fileContents = File.ReadAllText(fileName);
            
            SaveData readData = JsonConvert.DeserializeObject<SaveData>(fileContents, settings);

            LoadData(readData, world);


            return true;
        }

        private static void LoadData(SaveData saveData, World world) {
            foreach (Tile t in saveData.tiles) {
                t.world = world;
                world.Add(t);

            }
           
            foreach (Bee b in saveData.bees) {
                b.world = world;
                world.Add(b);
            }

        }


    }
}
