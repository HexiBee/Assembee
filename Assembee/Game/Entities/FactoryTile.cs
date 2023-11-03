using Assembee.Game.GameMath;
using Assembee.Game.Materials;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Assembee.Game.Entities
{
    public class FactoryTile : Tile, IBuildable, IInventoryTile {

        // Number of recipes worth of material that can be stored.
        private static readonly float CAPACITY_SIZE = 2.0f;

        public ImmutableDictionary<Material, float> BuildingRequirement { get { return type.BuildingRequirement; } }

        public ImmutableHashSet<Material> OutputMaterials { get; private set; }
        public ImmutableHashSet<Material> InputMaterials { get; private set; }

        private readonly FactoryType type;

        private float productionTime = 0.0f;

        protected Dictionary<Material, float> inputInventory;
        protected Dictionary<Material, float> outputInventory;
        protected Dictionary<Material, float> inputInventoryCapacity;
        protected Dictionary<Material, float> outputInventoryCapacity;

        public FactoryTile(GameRegistry.fct factoryType, HexPosition hexPosition) : base(GameRegistry.FactoryRegistry[factoryType].Texture, hexPosition) {
            type = GameRegistry.FactoryRegistry[factoryType];
            
            inputInventory = type.Recipe.Requirements.Keys.ToDictionary(material => material, material => 0.0f);
            outputInventory = type.Recipe.Products.Keys.ToDictionary(material => material, material => 0.0f);
            inputInventoryCapacity = type.Recipe.Requirements.Keys.ToDictionary(material => material, material => type.Recipe.Requirements[material] * CAPACITY_SIZE);
            outputInventoryCapacity = type.Recipe.Products.Keys.ToDictionary(material => material, material => type.Recipe.Products[material] * CAPACITY_SIZE);

            OutputMaterials = new HashSet<Material>(outputInventory.Keys).ToImmutableHashSet();
            InputMaterials = new HashSet<Material>(inputInventory.Keys).ToImmutableHashSet();
        }

        public override string GetInfoString() {
            string infoString = $"[{type.Name}]\nInput:";
            foreach (var kvp in inputInventory) {
                infoString += $"\n{kvp.Key.Name}: {kvp.Value} / {inputInventoryCapacity[kvp.Key]}";
            }

            infoString += "\nOutput:";
            foreach (var kvp in outputInventory) {
                infoString += $"\n{kvp.Key.Name}: {kvp.Value} / {outputInventoryCapacity[kvp.Key]}";
            }
            return infoString;
        }

        // Only handles discrete crafting right now.
        public override void Update(GameTime gameTime, World world) {
            /* Don't produce if output will exceed capacity. */
            foreach (var kvp in outputInventory) {
                if (kvp.Value + type.Recipe.Products[kvp.Key] > outputInventoryCapacity[kvp.Key]) {
                    PauseAnimation = true;
                    return;
                }
            }
            /* Don't produce if not enough materials. */
            foreach (var kvp in inputInventory) {
                if (kvp.Value < type.Recipe.Requirements[kvp.Key]) {
                    PauseAnimation = true;
                    return;
                }
            }

            PauseAnimation = false;

            productionTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (productionTime >= type.Recipe.Time) {
                productionTime = 0.0f;

                foreach (var kvp in type.Recipe.Requirements) {
                    inputInventory[kvp.Key] -= kvp.Value;
                }

                foreach (var kvp in type.Recipe.Products) {
                    outputInventory[kvp.Key] += kvp.Value;
                }
            }
        }

        public bool CanAcceptMaterial(Material material) {
            return inputInventory.ContainsKey(material);
        }

        public bool CanProvideMaterial(Material material) {
            return outputInventory.ContainsKey(material);
        }

        public float InputMaterial(Material material, float amount) {
            if (amount < 0) throw new ArgumentOutOfRangeException("amount", "Input amount must be nonnegative.");

            if (!CanAcceptMaterial(material)) return amount;

            float capacity = inputInventoryCapacity[material];
            float currentAmount = inputInventory[material];

            if (currentAmount + amount >= capacity) {
                inputInventory[material] = capacity;
                return currentAmount + amount - capacity;
            }

            inputInventory[material] += amount;
            return 0.0f;
        }

        public float OutputMaterial(Material material, float amount) {
            if (amount < 0) throw new ArgumentOutOfRangeException("amount", "Output ammount must be nonnegative.");

            if (!CanProvideMaterial(material)) return 0.0f;

            float currentAmount = outputInventory[material];

            if (currentAmount <= amount) {
                outputInventory[material] = 0.0f;
                return currentAmount;
            }

            outputInventory[material] -= amount;
            return amount;
        }
    }
}
