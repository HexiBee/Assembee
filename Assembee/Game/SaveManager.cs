﻿using Assembee.Game.Entities;
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
using Assembee.Game.GameMath;

namespace Assembee.Game {

    public class SaveData {
        public List<Tile> tiles { get; set; }

        public SaveData(List<Tile> tiles) {
            this.tiles = tiles;
        }
        public SaveData() {

        }
    }

    class SaveManager {
        private static string fileName = "save.bee";

        public SaveData saveData;

        static JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

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
            return new SaveData(world.Tiles);
        }

        public static bool Load(World world) {
            if (!File.Exists(fileName)) {
                return false;
            }
            //File.Decrypt(fileName);
            var fileContents = File.ReadAllText(fileName);
            
            SaveData readData = JsonConvert.DeserializeObject<SaveData>(fileContents, settings);

            LoadData(readData, world);

            return true;
        }

        private static void LoadData(SaveData saveData, World world) {
            foreach (Tile t in saveData.tiles) {
                world.AddTile(t);
            }
        }


    }
}
