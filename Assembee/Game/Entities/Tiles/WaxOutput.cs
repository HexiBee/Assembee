using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities.Tiles
{
    class WaxOutput : Tile{

        int tick = 0, waxOutputTime = 5;

        public WaxOutput(Game1.spr texture, Vector2 pos, World world) : base(texture, pos, world) {
            
        }

        public override void Update(GameTime gameTime) {

            if (!(beeInside is null) && beeInside.waxAmt < Bee.WAX_LIMIT) {

                if (tick % waxOutputTime == 0 && world.hive.waxAmt > 0) {
                    world.hive.waxAmt--;
                    beeInside.waxAmt++;
                }

                tick++;
            } else {
                tick = 0;
            }
            

            base.Update(gameTime);
        }

        public override string GetInfoString() {
            return "Wax Output:\nWax: " + world.hive.waxAmt.ToString();
        }

    }
}
