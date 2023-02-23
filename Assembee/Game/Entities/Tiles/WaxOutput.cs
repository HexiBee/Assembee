using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities.Tiles
{
    class WaxOutput : Tile{

        private int tick = 0;
        private int waxOutputTime = 5;
        private int availableWax = 0;

        public WaxOutput(ContentRegistry.spr texture, Vector2 pos) : base(texture, pos) {
            
        }

        public override void Update(GameTime gameTime, World world) {
            availableWax = world.MainHive.waxAmt;

            if (!(BeeInside is null) && BeeInside.waxAmt < Bee.WAX_LIMIT) {

                if (tick % waxOutputTime == 0 && world.MainHive.waxAmt > 0) {
                    world.MainHive.waxAmt--;
                    BeeInside.waxAmt++;
                }

                tick++;
            } else {
                tick = 0;
            }

            base.Update(gameTime, world);
        }

        public override string GetInfoString() {
            return "Wax Output:\nWax: " + availableWax.ToString();
        }

    }
}
