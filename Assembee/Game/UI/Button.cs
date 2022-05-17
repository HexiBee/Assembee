using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.UI {
    class Button : ButtonBase{

        private Text text;

        public Button(Element parent, string textString, float textSize, Vector2 position, Vector2 size, Orientation anchor) : base(parent, position, size, anchor) {
            text = new Text(this, textString, new Vector2(0, 0), textSize, Orientation.Center);
        }

    }
}
