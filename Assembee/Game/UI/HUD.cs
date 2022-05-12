using Assembee.Game.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities {
    static class HUD {
        static Color col = Color.Black;

        private static Root root = new Root();
        private static Panel beePanel = new Panel(root, new Vector2(0, 0), new Vector2(230, 165), Element.Orientation.UpperLeft, Element.Orientation.UpperLeft);
        private static Panel tilePanel = new Panel(root, new Vector2(0, 0), new Vector2(290, 130), Element.Orientation.UpperRight, Element.Orientation.UpperRight);

        public static void DrawHud(SpriteBatch spriteBatch, World world) {

            root.updateSize();

            if (!(world.selectedTile is null)) {
                tilePanel.Draw(spriteBatch);
                world.selectedTile.drawInfoUI(spriteBatch, world);
            }
            if (world.selectedBee != null) {
                beePanel.Draw(spriteBatch);
                spriteBatch.DrawString(Game1.font1, "Bee:", new Vector2(10, 5), col);
                spriteBatch.DrawString(Game1.font1, "Nectar: " + ((int)world.selectedBee.nectarAmt).ToString() + " / " + Bee.NECTAR_LIMIT.ToString(), new Vector2(10, 45), col);
                spriteBatch.DrawString(Game1.font1, "Honey: " + ((int)world.selectedBee.honeyAmt).ToString() + " / " + Bee.HONEY_LIMIT.ToString(), new Vector2(10, 85), col);
                spriteBatch.DrawString(Game1.font1, "Wax: " + ((int)world.selectedBee.waxAmt).ToString() + " / " + Bee.WAX_LIMIT.ToString(), new Vector2(10, 125), col);
            }

        }
    }
}
