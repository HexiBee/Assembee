using Assembee.Game.GameMath;
using Assembee.Game.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities {
    public class Hive : Tile, IInventoryTile {

        public Dictionary<Material, float> Inventory { get; private set; }

        public Hive(ContentRegistry.spr texture, HexPosition hexPosition) : base(texture, hexPosition){
            Inventory = new Dictionary<Material, float>();
            Inventory.Add(GameRegistry.MaterialRegistry[GameRegistry.mat.honey], 100);
            Inventory.Add(GameRegistry.MaterialRegistry[GameRegistry.mat.wax], 100);
        }

        public override void Update(GameTime gameTime, World world) {
        }

        public override string GetInfoString() {
            string infoString = "[Hive]";
            foreach(KeyValuePair<Material, float> kvp in Inventory) {
                infoString += $"\n{kvp.Key.Name}: {kvp.Value}";
            }
            return infoString;
        }

        public bool CanAcceptMaterial(Material material) {
            return true;
        }

        public bool CanProvideMaterial(Material material) {
            return true;
        }

        public float InputMaterial(Material material, float amount) {
            if (amount < 0) throw new ArgumentOutOfRangeException("amount", "Input amount must be nonnegative.");

            if (Inventory.ContainsKey(material)) {
                Inventory[material] += amount;
            } else {
                Inventory[material] = amount;
            }

            return 0.0f;
        }

        public float OutputMaterial(Material material, float amount) {
            if (amount < 0) throw new ArgumentOutOfRangeException("amount", "Output amount must be nonnegative.");

            float currentAmount = Inventory[material];

            if (currentAmount <= amount) {
                Inventory[material] = 0.0f;
                return currentAmount;
            }

            Inventory[material] -= amount;
            return amount;
        }

    }
}
