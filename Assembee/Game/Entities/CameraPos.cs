using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities {
    class CameraPos : Actor {

        public float moveSpeed = 4.0f;
        public float baseMoveSpeed = 4.0f;

        public CameraPos(Game1.spr texture, Vector2 pos, World world) : base(texture, pos, world) {

        }

        public override void Draw(SpriteBatch spriteBatch, int animTick) {
        }

        public override void Update(GameTime gameTime) {
            if (Input.keyDown(Input.Left)) {
                position.X-=moveSpeed;
            }
            if (Input.keyDown(Input.Right)) {
                position.X+=moveSpeed;
            }
            if (Input.keyDown(Input.Up)) {
                position.Y-=moveSpeed;
            }
            if (Input.keyDown(Input.Down)) {
                position.Y+=moveSpeed;
            }

        }
    }
}
