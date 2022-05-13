using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.UI {
    class Text : Element{

        private string textString;
        private float scale;

        public Text(Element parent, string textString, Vector2 position, float scale, Orientation anchor, Orientation origin) : base(parent, position, Game1.font1.MeasureString(textString) * scale, anchor, origin) {
            this.textString = textString;
            this.scale = scale;
        }

        public Text(Element parent, string textString, Vector2 position, float scale, Orientation anchor) : base(parent, position, Game1.font1.MeasureString(textString) * scale, anchor) {
            this.textString = textString;
            this.scale = scale;
        }

        protected override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(Game1.font1, textString, getDrawPosition(), Color.DarkOrange, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 1.0f);
            base.Draw(spriteBatch);
        }

        public void SetString(string textString) {
            this.textString = textString;
            size = findSize();
        }

        private Vector2 findSize() {
            return Game1.font1.MeasureString(textString) * scale;
        }


    }
}
