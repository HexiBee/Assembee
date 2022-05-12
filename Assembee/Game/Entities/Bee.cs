using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities {
    class Bee : Actor {
        //Texture2D sBee;

        public const int NECTAR_LIMIT = 100, HONEY_LIMIT = 20, WAX_LIMIT = 3;

        public int nectarAmt = 0, honeyAmt = 0, waxAmt = 0;

        private Tile target;
        private Tile start;
        private float t;

        public Bee(Game1.spr textureSpr, Vector2 pos, World world) : base(textureSpr, pos, world) {
            start = world.GetTile(pos);
            position = start.position;
            start.beeInside = this;
        }

        public void SetTarget(Tile target) {
            foreach (Bee bee in world.bees.ToArray()) {
                if (bee == this) continue;
                if (bee.target == target) return;
            }
            if (target.position != position && target.beeInside is null) {
                this.target = target;
                t = 0;
                start.beeInside = null;

                sprite.rotation = (float)Math.Atan2(target.position.Y - start.position.Y, target.position.X - start.position.X) + (float)Math.PI / 2.0f;
            }
        }

        public override void Update(GameTime gameTime) {
            if (target != null) {

                position = Vector2.Lerp(start.position, target.position, t);
                t += 4.0f / Vector2.Distance(start.position, target.position);

                if (t >= 1 || position == target.position) {
                    position = target.position;
                    t = 0;
                    target.beeInside = this;
                    start = target;
                    target = null;
                    world.audio.StopSound(Audio.sfx.bee);
                }
            } 
        }
    }
}
