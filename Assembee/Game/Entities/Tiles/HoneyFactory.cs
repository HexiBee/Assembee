using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities.Tiles {
    class HoneyFactory : Tile {

        static int HONEY_NEEDED = 5;
        static int WAX_NEEDED = 5;
        static string NAME = "Honey Factory";

        public int honeyLimit = 150, nectarLimit = 150;
        //public int honeyAmt = 0;//, nectarAmt = 0;
        int tick = 0, nectarProcessTime = 38, nectarProcessed = 0, nectarNeeded = 4;

        public HoneyFactory(ContentRegistry.spr texture, Vector2 pos, World world) : base(texture, pos, world) {

        }


        public override void Update(GameTime gameTime) {
            if (beeInside != null && beeInside.honeyAmt < Bee.HONEY_LIMIT) {
                tick++;
                GenerateHoney();
            } else {
                pauseAnim = true;
            }
        }
        public void GenerateHoney() {

            if (beeInside.nectarAmt > 0) {
                pauseAnim = false;
                if (tick % nectarProcessTime == 0) {
                    nectarProcessed++;
                    beeInside.nectarAmt--;
                }
                if (nectarProcessed >= nectarNeeded) {
                    beeInside.honeyAmt++;
                    nectarProcessed = 0;
                }
            } else {
                pauseAnim = true;
            }
        }

        public override string GetInfoString() {
            return "Honey Factory:\nNectar: " + nectarProcessed.ToString() + " / " + nectarNeeded.ToString();
        }

        public static string BuildUI(World world) {
            return IBuildable.BuildUI(world, HONEY_NEEDED, WAX_NEEDED, NAME);
        }

        public override bool BuildingReqs(out int honeyRequired, out int waxRequired) {
            honeyRequired = HONEY_NEEDED;
            waxRequired = WAX_NEEDED;
            return true;
        }
    }
}
