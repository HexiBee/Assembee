using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game {
    class Sprite {

        private const Game1.spr MISSING_TEXTURE = Game1.spr.t_hex;

        public float scale = 1.0f;
        public float rotation = 0.0f;
        public Color color = Color.White;

        private Texture2D _texture;
        private int _frameHeight;
        private int _animFrames;

        public Sprite(Game1.spr registryEnum) {

            if (!Game1.TextureRegistry.TryGetValue(registryEnum, out _texture)) 
                Game1.TextureRegistry.TryGetValue(MISSING_TEXTURE, out _texture);

            if (!Game1.AnimationRegistry.TryGetValue(registryEnum, out _frameHeight))
                _frameHeight = _texture.Height;

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
