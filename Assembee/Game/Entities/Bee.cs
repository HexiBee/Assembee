using Assembee.Game.GameMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
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

        [JsonProperty] public bool Flying { get; private set; } = false;

        [JsonProperty] private Vector2 target;
        [JsonProperty] private Vector2 start;
        [JsonProperty] private float t;
        [JsonProperty] private int init = 0;

        public Bee(ContentRegistry.spr textureSpr, Vector2 pos) : base(textureSpr, pos) {
        }
        
        public void SetTarget(Vector2 target, World world) {
            start = position;

            Tile startTile = world.GetTile(HexPosition.PositionToHexPosition(start));
            Tile targetTile = world.GetTile(HexPosition.PositionToHexPosition(target));

            foreach (Bee bee in world.Bees.ToArray()) {
                if (bee == this) continue;
                if (bee.target == target) return;
            }
            if (target != position && targetTile.BeeInside is null) {
                this.target = target;
                t = 0;
                startTile.BeeInside = null;

                sprite.rotation = (float)Math.Atan2(target.Y - start.Y, target.X - start.X) + (float)Math.PI / 2.0f;

                Flying = true;
            }
        }

        public override void Update(GameTime gameTime, World world) {
            if (init == 0) {
                Tile startTile = world.GetTile(HexPosition.PositionToHexPosition(start));
                startTile.BeeInside = this;
                start = position;
                init++;
            }
            
            if (Flying) {
                position = Vector2.Lerp(start, target, t);
                t += 4.0f / Vector2.Distance(start, target);

                if (t >= 1) {
                    position = target;

                    world.GetTile(HexPosition.PositionToHexPosition(position)).BeeInside = this;

                    Flying = false;
                    world.GameAudio.StopSound(Audio.sfx.bee);
                }
            }
        }
    }
}
