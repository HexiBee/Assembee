using Assembee.Game.GameMath;
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
        
        public Hive(ContentRegistry.spr texture, HexPosition hexPosition) : base(texture, hexPosition) {
        }

        public override void Update(GameTime gameTime, World world) {
            if (BeeInside != null) {
                tick++;
                DepositResources();
            }
        }

        public void DepositResources() {
            if (tick % moveTime == 0) {
                if (BeeInside.honeyAmt > 0) {
                    BeeInside.honeyAmt--;
                    honeyAmt++;
                }
                if (BeeInside.waxAmt > 0) {
                    BeeInside.waxAmt--;
                    waxAmt++;
                }
            }
        }

        public override string GetInfoString() {
            return "Hive:\nHoney: " + honeyAmt.ToString() + "\nWax: " + waxAmt.ToString();
        }

    }
}
