using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Assembee.Game.Entities {
    public class Entity {

        [JsonIgnore]
        public World world;
        public Vector2 position { get; set; }


        protected bool pauseAnim = false;
        //[JsonIgnore]
        public Sprite sprite { get; set; }

        public ContentRegistry.spr texture { get; set; }

        public Entity(ContentRegistry.spr texture, Vector2 position, World world) {
            
            this.position = position;
            this.world = Game1.world;
            this.texture = texture;
            sprite = new Sprite(this.texture);
            
        }


        public virtual void Draw(SpriteBatch spriteBatch, int animTick) {

            sprite.Draw(spriteBatch, position, pauseAnim ? 0 : animFrameFromTick(animTick));
            
        }

        public virtual void Update(GameTime gameTime) {
        }

        protected virtual int animFrameFromTick(int animTick) {
            return animTick / 5;
        }
        public bool ShouldSerializesprite() {
            if (sprite.color == Color.White && 
                sprite.rotation == 0.0f &&
                sprite.scale == 1.0f) {
                return false;
            }
            return true;
        }

    }
}
