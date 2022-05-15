using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game {
    public class Sprite {

        public float scale = 1.0f;
        public float rotation { get; set; } = 0.0f;
        public Color color { get; set; } = Color.White;
        public ContentRegistry.spr registryEnum { get; set; }

        private Texture2D _texture;
        private int _frameHeight;
        private int _animFrames;

        public Sprite(ContentRegistry.spr registryEnum) {
            this.registryEnum = registryEnum;

            _texture = ContentRegistry.GetTexture(registryEnum);
            _frameHeight = ContentRegistry.GetFrameHeight(registryEnum);

            _animFrames = _texture.Height / _frameHeight;

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, int animFrame) {

            animFrame %= _animFrames;

            spriteBatch.Draw(
                _texture,
                position,
                new Rectangle(0, animFrame * _frameHeight, _texture.Width, _frameHeight),
                color,
                rotation,
                new Vector2(_texture.Width / 2, _frameHeight / 2),
                scale,
                SpriteEffects.None,
                1.0f
            );

        }


    }
}
