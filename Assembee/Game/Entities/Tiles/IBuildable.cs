using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities.Tiles {
    interface IBuildable {
        static int HONEY_NEEDED { get; set; }
        static int WAX_NEEDED { get; set; }
        static string NAME { get; set; }

        public static void BuildUI(SpriteBatch spriteBatch, World world, int HONEY_NEEDED, int WAX_NEEDED, string NAME) {
            Vector2 orig = new Vector2(0, Game1.ScreenHeight - 250 + 130);
            spriteBatch.DrawString(Game1.font1, "Build " + NAME + ":", orig, Color.Black);
            spriteBatch.DrawString(Game1.font1, "Honey Needed: " + world.hive.honeyAmt.ToString() + " / " + HONEY_NEEDED.ToString(), orig + new Vector2(0, 40), Color.Black);
            spriteBatch.DrawString(Game1.font1, "Wax Needed: " + world.hive.waxAmt.ToString() + " / " + WAX_NEEDED.ToString(), orig + new Vector2(0, 80), Color.Black);
        }
    }
}
