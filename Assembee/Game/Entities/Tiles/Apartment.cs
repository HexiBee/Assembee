using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Assembee.Game.GameMath;

namespace Assembee.Game.Entities.Tiles {
    public class Apartment : Tile, IBuildable {

        private static int HONEY_NEEDED = 5;
        private static int WAX_NEEDED = 5;
        private static string NAME = "Apartment";
        public Apartment(ContentRegistry.spr texture, HexPosition hexPosition) : base(texture, hexPosition) {
        }

        public override string GetInfoString() {
            return "Apartment: \nBees: +1";
        }

        public override bool BuildingReqs(out int honeyRequired, out int waxRequired) {
            honeyRequired = HONEY_NEEDED;
            waxRequired = WAX_NEEDED;
            return true;
        }

        public static string BuildUI(World world) {
            return IBuildable.BuildUI(world, HONEY_NEEDED, WAX_NEEDED, NAME);
        }

        public override void Update(GameTime gameTime, World world) {
            pauseAnim = BeeInside is null;
            base.Update(gameTime, world);
        }
    }
}
