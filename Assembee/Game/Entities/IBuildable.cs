using Assembee.Game.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Assembee.Game.Entities {
    interface IBuildable {

        public virtual ImmutableDictionary<Material, float> BuildingRequirement { get { return ImmutableDictionary.Create<Material, float>(); } }
        public virtual string Name { get { return ""; } }

        public static string CreateUIString(World world, string name, ImmutableDictionary<Material, float> buildingRequirements) {
            string uiString = $"Build {name}:";
            float resourceAmount;
            foreach (var kvp in buildingRequirements) {
                resourceAmount = world.MainHive.Inventory.GetValueOrDefault(kvp.Key, 0.0f);
                uiString += $"\n{kvp.Key} Needed: {resourceAmount} / {kvp.Value}";
            }
            return uiString;
        }

        public static string CreateUIString(World world, FactoryType type) {
            return CreateUIString(world, type.Name, type.BuildingRequirement);
        }

        public static bool CanBuild(IBuildable building, Dictionary<Material, float> inventory) {
            return building.BuildingRequirement.All(kvp =>
                            inventory.GetValueOrDefault(kvp.Key, 0.0f) >= kvp.Value);

        }
    }
}
