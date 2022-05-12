using Assembee.Game.GameMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities {
    class Tile : Entity {
        Matrix2 hexMat = new Matrix2((float)Math.Sqrt(3), (float)Math.Sqrt(3) / 2.0f, 0.0f, 3.0f / 2.0f);

        //public static int HONEY_NEEDED = 0;
        //public static int WAX_NEEDED = 0;

        public Bee beeInside = null;
        public Vector2 gridPos;

        public Tile(Game1.spr texture, Vector2 gridPos, World world) : base(texture, gridPos, world) {
            this.gridPos = gridPos;
            position = hexMat * new Vector2(gridPos.X * 127, gridPos.Y * 127);
        }

        public override void Update(GameTime gameTime) {

        }

        public virtual void drawInfoUI(SpriteBatch spriteBatch, World world) {

        }
        public virtual bool BuildingReqs(out int honeyRequired, out int waxRequired) {
            honeyRequired = 0;
            waxRequired = 0;
            return false;
        }
        //public static void BuildUI(SpriteBatch spriteBatch, World world) {
        //    Vector2 orig = new Vector2(0, Game1.ScreenHeight - 250);
        //    spriteBatch.DrawString(Game1.font1, "Build Honey Factory:", orig, Color.Black);
        //    spriteBatch.DrawString(Game1.font1, "Honey Needed: " + world.hive.honeyAmt.ToString() + " / " + HONEY_NEEDED.ToString(), orig + new Vector2(0, 40), Color.Black);
        //    spriteBatch.DrawString(Game1.font1, "Wax Needed: " + world.hive.waxAmt.ToString() + " / " + WAX_NEEDED.ToString(), orig + new Vector2(0, 80), Color.Black);
        //}


    }
}
