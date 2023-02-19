using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities {
    public class Bee : Actor {

        public const int NECTAR_LIMIT = 100;
        public const int HONEY_LIMIT = 20;
        public const int WAX_LIMIT = 3;

        public int nectarAmt = 0;
        public int honeyAmt = 0;
        public int waxAmt = 0;

        private Tile target;
        private Tile start;
        private float t;
        private int init = 0;

        public Bee(ContentRegistry.spr textureSpr, Vector2 pos, World world) : base(textureSpr, pos, world) {
        }
        
        public void SetTarget(Tile target) {
            if (start == null) {
                start = world.GetTile(this.position);
            }
            foreach (Bee bee in world.Bees.ToArray()) {
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

            // THIS STUFF DIDNT WANT TO WORK IN THE CONSTRUCTOR BECAUSE OF LOADING
            if (init == 0) {
                Tile s = world.GetTile(Input.hexRound(position / (127.0f * (float)Math.Sqrt(3))));
                Util.Log("bee tile: " + world.GetTile(Input.hexRound(position / (127.0f * (float)Math.Sqrt(3)))).ToString());
                start = world.GetTile(Input.hexRound(position / (127.0f * (float)Math.Sqrt(3))));
                position = start.position;
                start.beeInside = this;
                init++;
            }
            
            if (target != null) {
                position = Vector2.Lerp(start.position, target.position, t);
                t += 4.0f / Vector2.Distance(start.position, target.position);

                if (t >= 1 || position == target.position) {
                    position = target.position;
                    t = 0;
                    target.beeInside = this;
                    start = target;
                    target = null;
                    world.GameAudio.StopSound(Audio.sfx.bee);
                }
            }
        }
    }
}
