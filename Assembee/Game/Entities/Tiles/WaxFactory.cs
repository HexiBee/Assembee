using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities.Tiles {
    class WaxFactory : Tile{



        int tick = 0, waxProcessTime = 60, honeyProcessed = 0, honeyNeeded = 6;
        static int HONEY_NEEDED = 15;
        static int WAX_NEEDED = 5;
        static string NAME = "Wax Factory";
        public WaxFactory(Game1.spr texture, Vector2 pos, World world) : base(texture, pos, world) {
            
    }


        public override void Update(GameTime gameTime) {
            if (!(beeInside is null) && beeInside.waxAmt < Bee.WAX_LIMIT) {
                tick++;
                GenerateWax();
            } else {
                pauseAnim = true;
            }
        }
        public void GenerateWax() {
            if (beeInside.honeyAmt > 0) {
                pauseAnim = false;
                if (tick % waxProcessTime == 0) {
                    honeyProcessed++;
                    beeInside.honeyAmt--;
                }
                if (honeyProcessed >= honeyNeeded) {
                    beeInside.waxAmt++;
                    honeyProcessed = 0;
                }
            } else {
                pauseAnim = true;
            }
        }

        public override void drawInfoUI(SpriteBatch spriteBatch, World world) {
            Vector2 orig = new Vector2(Game1.ScreenWidth - 250, 0);
            spriteBatch.DrawString(Game1.font1, "Wax Factory:", orig + new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(Game1.font1, "Honey: " + honeyProcessed.ToString() + " / " + honeyNeeded.ToString(), orig + new Vector2(0, 40), Color.Black);
        }

        public override bool BuildingReqs(out int honeyRequired, out int waxRequired) {
            honeyRequired = HONEY_NEEDED;
            waxRequired = WAX_NEEDED;
            return true;
        }

        public static void BuildUI(SpriteBatch spriteBatch, World world) {
            IBuildable.BuildUI(spriteBatch, world, HONEY_NEEDED, WAX_NEEDED, NAME);
        }

    }
}
