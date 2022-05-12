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

        public override void drawInfoUI(SpriteBatch spriteBatch, World world) {
            Vector2 orig = new Vector2(Game1.ScreenWidth - 250, 0);
            spriteBatch.DrawString(Game1.font1, "Apartment:", orig + new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(Game1.font1, "Bees: +1", orig + new Vector2(0, 40), Color.Black);
        }

        public override bool BuildingReqs(out int honeyRequired, out int waxRequired) {
            honeyRequired = HONEY_NEEDED;
            waxRequired = WAX_NEEDED;
            return true;
        }

        public static void BuildUI(SpriteBatch spriteBatch, World world) {
            IBuildable.BuildUI(spriteBatch, world, HONEY_NEEDED, WAX_NEEDED, NAME);
        }

        public override void Update(GameTime gameTime) {
            pauseAnim = beeInside is null;
            base.Update(gameTime);
        }
    }
}
