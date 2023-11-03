using Assembee.Game.Materials;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Assembee.Game {
    public class FactoryType {

        public ContentRegistry.spr Texture { get; private set; }
        public ImmutableDictionary<Material, float> BuildingRequirement { get; private set; }
        public string Name { get; private set; }
        public Recipe Recipe { get; private set; }

        public FactoryType(string name, ContentRegistry.spr texture, ImmutableDictionary<Material, float> buildingRequirement, Recipe recipe) {
            Texture = texture;
            BuildingRequirement = buildingRequirement;
            Name = name;
            Recipe = recipe;
        }

        public FactoryType(string name, ContentRegistry.spr texture, Dictionary<Material, float> buildingRequirement, Recipe recipe) : this(name, texture, buildingRequirement.ToImmutableDictionary(), recipe) {
        }

    }
}
