using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities.Tiles
{
    class HoneyOutput : Tile{

        private int tick = 0;
        private int honeyOutputTime = 5;
        private int availableHoney = 0;

        public HoneyOutput(ContentRegistry.spr texture, Vector2 gridPos) : base(texture, gridPos) {

        }

        public override void Update(GameTime gameTime, World world) {
            availableHoney = world.MainHive.honeyAmt;

            if (!(BeeInside is null) && BeeInside.honeyAmt < Bee.HONEY_LIMIT) {

                if (tick % honeyOutputTime == 0 && world.MainHive.honeyAmt > 0) {
                    world.MainHive.honeyAmt--;
                    BeeInside.honeyAmt++;
                }

                tick++;
            } else {
                tick = 0;
            }
            

            base.Update(gameTime, world);
        }

        public override string GetInfoString() {
            return "Honey Output:\nHoney: " + availableHoney.ToString();
        }

    }
}
