using Assembee.Game.GameMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.UI {
    class Text : Element{

        private static readonly Matrix2 CORRECT_MATRIX = new Matrix2(1, 0, 0, 0.95f);

        private string textString;
        private float scale;
        private SpriteFont font;

        public Text(Element parent, string textString, Vector2 position, float scale, Orientation anchor) : base(parent, position, CORRECT_MATRIX * Game1.font24.MeasureString(textString) * scale, anchor) {
            this.textString = textString;
            this.scale = scale;
            font = Game1.font24;
        }

        public Text(Element parent, string textString, Vector2 position, float scale, Orientation anchor, SpriteFont font) : base(parent, position, CORRECT_MATRIX * font.MeasureString(textString) * scale, anchor) {
            this.textString = textString;
            this.scale = scale;
            this.font = font;
        }

        protected override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(font, textString, getDrawPosition(), Color.DarkOrange, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 1.0f);
            base.Draw(spriteBatch);
        }

        public void SetString(string textString) {
            this.textString = textString;
            size = findSize();
        }

        private Vector2 findSize() {
            return CORRECT_MATRIX * font.MeasureString(textString) * scale;
        }


    }
}
