using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities {
    class Entity {

        public World world;
        public Vector2 position;

        protected bool pauseAnim = false;
        protected Sprite sprite;

        public Entity(Game1.spr texture, Vector2 position, World world) {
            
            this.position = position;
            this.world = world;

            sprite = new Sprite(texture);
            
        }

        public virtual void Draw(SpriteBatch spriteBatch, int animTick) {

            sprite.Draw(spriteBatch, position, pauseAnim ? 0 : animFrameFromTick(animTick));
            
        }

        public virtual void Update(GameTime gameTime) {
        }

        protected virtual int animFrameFromTick(int animTick) {
            return animTick / 5;
        }
    }
}
