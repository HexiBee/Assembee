using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities.Tiles {
    class Flowers : Tile {
        //public float nectarAmt = 150;
        public int nectarAmt = 150, nectarTime = 32;
        int tick = 0;
        public Flowers(Game1.spr texture, Vector2 pos, World world): base(texture, pos, world) {

        }

        public Flowers(int nectarAmt, Game1.spr texture, Vector2 pos, World world) : base(texture, pos, world) {
            this.nectarAmt = nectarAmt;
        }


        public override void Update(GameTime gameTime) {
            if (beeInside != null) {
                tick++;
                TakeNectar(beeInside);
            }
        }

        public void TakeNectar(Bee bee) {
            if (nectarAmt > 0) {
                if (tick % nectarTime == 0 && bee.nectarAmt < Bee.NECTAR_LIMIT) {
                    nectarAmt--;
                    bee.nectarAmt++;
                }
            } else {
                sprite.color = Color.DarkSlateGray;
            }

            //float amt = 0.01f;
            //if (nectarAmt >= 0.01f) { 
            //    nectarAmt -= amt;
            //    bee.nectarAmt += amt;
            //}
        }

        public override void drawInfoUI(SpriteBatch spriteBatch, World world) {
            Vector2 orig = new Vector2(Game1.ScreenWidth - 250, 0);
            spriteBatch.DrawString(Game1.font1, "Flowers:", orig + new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(Game1.font1, "Nectar: " + nectarAmt.ToString(), orig + new Vector2(0, 40), Color.Black);
        }

    }
}
