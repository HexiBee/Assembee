using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Assembee.Game.Entities {
    public class Entity {

        public Vector2 position;

        protected bool PauseAnimation = false;

        public Sprite Sprite { get; private set; }

        public ContentRegistry.spr texture { get; set; }

        public Entity(ContentRegistry.spr texture, Vector2 position) {
            this.position = position;
            this.texture = texture;
            Sprite = new Sprite(this.texture);
        }

        public virtual void Draw(SpriteBatch spriteBatch, int animTick) {
            Sprite.Draw(spriteBatch, position, PauseAnimation ? 0 : AnimationFrameFromTick(animTick));
        }

        public virtual void Update(GameTime gameTime, World world) {
        }

        protected virtual int AnimationFrameFromTick(int animTick) {
            return animTick / 5;
        }

    }
}
