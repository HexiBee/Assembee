using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities.Tiles {
    class Apartment : Tile, IBuildable {

        static int HONEY_NEEDED = 5;
        static int WAX_NEEDED = 5;
        static string NAME = "Apartment";
        public Apartment(Game1.spr texture, Vector2 position, World world) : base(texture, position, world) {
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

        public override void Update(GameTime gameTime) {
            pauseAnim = beeInside is null;
            base.Update(gameTime);
        }
    }
}
