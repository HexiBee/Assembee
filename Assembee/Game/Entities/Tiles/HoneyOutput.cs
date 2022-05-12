using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities.Tiles
{
    class HoneyOutput : Tile{

        int tick = 0, honeyOutputTime = 5;

        public HoneyOutput(Game1.spr texture, Vector2 pos, World world) : base(texture, pos, world) {
            
        }

        public override void Update(GameTime gameTime) {

            if (!(beeInside is null) && beeInside.honeyAmt < Bee.HONEY_LIMIT) {

                if (tick % honeyOutputTime == 0 && world.hive.honeyAmt > 0) {
                    world.hive.honeyAmt--;
                    beeInside.honeyAmt++;
                }

                tick++;
            } else {
                tick = 0;
            }
            

            base.Update(gameTime);
        }

        public override void drawInfoUI(SpriteBatch spriteBatch, World world) {
            Vector2 orig = new Vector2(Game1.ScreenWidth - 250, 0);
            spriteBatch.DrawString(Game1.font1, "Honey Output:", orig, Color.Black);
            spriteBatch.DrawString(Game1.font1, "Honey: " + world.hive.honeyAmt.ToString(), orig + new Vector2(0, 40), Color.Black);

            base.drawInfoUI(spriteBatch, world);
        }

    }
}
