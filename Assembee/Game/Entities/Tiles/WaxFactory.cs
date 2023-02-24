using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Assembee.Game.GameMath;

namespace Assembee.Game.Entities.Tiles {
    class WaxFactory : Tile{



        int tick = 0, waxProcessTime = 60, honeyProcessed = 0, honeyNeeded = 6;
        static int HONEY_NEEDED = 15;
        static int WAX_NEEDED = 5;
        static string NAME = "Wax Factory";
        public WaxFactory(ContentRegistry.spr texture, HexPosition hexPosition) : base(texture, hexPosition) {
            
    }


        public override void Update(GameTime gameTime, World world) {
            if (!(BeeInside is null) && BeeInside.waxAmt < Bee.WAX_LIMIT) {
                tick++;
                GenerateWax();
            } else {
                pauseAnim = true;
            }
        }
        public void GenerateWax() {
            if (BeeInside.honeyAmt > 0) {
                pauseAnim = false;
                if (tick % waxProcessTime == 0) {
                    honeyProcessed++;
                    BeeInside.honeyAmt--;
                }
                if (honeyProcessed >= honeyNeeded) {
                    BeeInside.waxAmt++;
                    honeyProcessed = 0;
                }
            } else {
                pauseAnim = true;
            }
        }

        public override string GetInfoString() {
            return "Wax Factory:\nHoney " + honeyProcessed.ToString() + " / " + honeyNeeded.ToString();
        }

        public override bool BuildingReqs(out int honeyRequired, out int waxRequired) {
            honeyRequired = HONEY_NEEDED;
            waxRequired = WAX_NEEDED;
            return true;
        }

        public static string BuildUI(World world) {
            return IBuildable.BuildUI(world, HONEY_NEEDED, WAX_NEEDED, NAME);
        }

    }
}
