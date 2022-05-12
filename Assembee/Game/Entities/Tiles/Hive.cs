using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities.Tiles {
    class Hive : Tile {


        public int honeyAmt = 0, waxAmt = 0, moveTime = 5;
        int tick = 0;
        
        public Hive(Game1.spr texture, Vector2 pos, World world) : base(texture, pos, world) {

        }

        //public override void Update(GameTime gameTime) {
        //    //Util.Log("!!");
        //    if (world.selectedTile == this) {
        //        if (beeInside != null) {

        //            world.activeBee = beeInside;
        //        }

        //    }
        //}

        public override void Update(GameTime gameTime) {
            if (beeInside != null) {
                tick++;
                DepositResources();
                

                //DepositResources();
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

        public override void drawInfoUI(SpriteBatch spriteBatch, World world) {
            Vector2 orig = new Vector2(Game1.ScreenWidth - 250, 0);
            spriteBatch.DrawString(Game1.font1, "Hive:", orig + new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(Game1.font1, "Honey: " + honeyAmt.ToString(), orig + new Vector2(0, 40), Color.Black);
            spriteBatch.DrawString(Game1.font1, "Wax: " + waxAmt.ToString(), orig + new Vector2(0, 80), Color.Black);

            //orig = new Vector2(Game1.);
        }



    }
}
