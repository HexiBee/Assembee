using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Assembee.Game.GameMath;

namespace Assembee.Game.Entities.Tiles {
    public class Flowers : Tile {
        //public float nectarAmt = 150;
        public int nectarAmt = 150;
        private int nectarTime = 32;
        int tick = 0;
        //public Flowers(Game1.spr texture, Vector2 pos, World world): base(texture, pos, world) {

        //}

        public Flowers(int nectarAmt, ContentRegistry.spr texture, HexPosition hexPosition) : base(texture, hexPosition) {
            this.nectarAmt = nectarAmt;
        }


        public override void Update(GameTime gameTime, World world) {
            if (BeeInside != null) {
                tick++;
                TakeNectar(BeeInside);
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

        public override string GetInfoString() {
            return "Flowers:\nNectar: " + nectarAmt.ToString();
        }

    }
}
