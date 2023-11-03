using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Assembee.Game.GameMath;
using Assembee.Game.Materials;
using System.Collections.Immutable;

namespace Assembee.Game.Entities {
    public class Apartment : Tile, IBuildable {

        public static ImmutableDictionary<Material, float> StaticBuildingRequirements { get; } = new Dictionary<Material, float>() {
                { GameRegistry.MaterialRegistry[GameRegistry.mat.honey], 5 },
                { GameRegistry.MaterialRegistry[GameRegistry.mat.wax], 5 }
        }.ToImmutableDictionary();

        public ImmutableDictionary<Material, float> BuildingRequirements { get { return StaticBuildingRequirements; } }
        public string Name { get; } = "Apartment";

        public Apartment(ContentRegistry.spr texture, HexPosition hexPosition) : base(texture, hexPosition) {
            PauseAnimation = true;
        }

        public override string GetInfoString() {
            return "Apartment: \nBees: +1";
        }

        public override void Update(GameTime gameTime, World world) {
            base.Update(gameTime, world);
        }
    }
}
