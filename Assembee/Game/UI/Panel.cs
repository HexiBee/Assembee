using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.UI {
    class Panel : Element {

        private const Game1.spr DEFAULT_SPRITE = Game1.spr.u_panel;

        private Texture2D texture;

        public Panel(Element parent, Vector2 position, Vector2 size, Orientation anchor, Orientation origin) : base(parent, position, size, anchor, origin) {
            Game1.TextureRegistry.TryGetValue(DEFAULT_SPRITE, out texture);
        }

        public Panel(Element parent, Vector2 position, Vector2 size, Orientation anchor) : base(parent, position, size, anchor) {
            Game1.TextureRegistry.TryGetValue(DEFAULT_SPRITE, out texture);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, new Rectangle(getDrawPosition().ToPoint(), size.ToPoint()), Color.White);
            base.Draw(spriteBatch);
        }

    }
}
