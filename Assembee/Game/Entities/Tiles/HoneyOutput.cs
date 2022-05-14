using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities.Tiles
{
    class HoneyOutput : Tile{

        int tick = 0, honeyOutputTime = 5;

        public HoneyOutput(Game1.spr texture, Vector2 gridPos, World world) : base(texture, gridPos, world) {

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

        public override string GetInfoString() {
            return "Honey Output:\nHoney: " + world.hive.honeyAmt.ToString();
        }

    }
}
