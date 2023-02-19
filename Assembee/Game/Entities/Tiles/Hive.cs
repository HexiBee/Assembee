using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities.Tiles {
    public class Hive : Tile {

        public int honeyAmt = 0;
        public int waxAmt = 0;
        public int moveTime = 5;

        private int tick = 0;
        
        public Hive(ContentRegistry.spr texture, Vector2 pos, World world) : base(texture, pos, world) {
        }

        public override void Update(GameTime gameTime) {
            if (beeInside != null) {
                tick++;
                DepositResources();
            }
        }

        public void DepositResources() {
            if (tick % moveTime == 0) {
                if (beeInside.honeyAmt > 0) {
                    beeInside.honeyAmt--;
                    honeyAmt++;
                }
                if (beeInside.waxAmt > 0) {
                    beeInside.waxAmt--;
                    waxAmt++;
                }
            }
        }

        public override string GetInfoString() {
            return "Hive:\nHoney: " + honeyAmt.ToString() + "\nWax: " + waxAmt.ToString();
        }



    }
}
