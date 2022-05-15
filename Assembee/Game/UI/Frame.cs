using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.UI {
    class Frame : Element {

        public Frame(Element parent) : base(parent, parent.position, parent.size) {

        }

        public override bool IntersectingGlobalPoint(Point point) {
            if (!active) return false;

            foreach (Element child in children) {
                if (child.IntersectingGlobalPoint(point)) {
                    return true;
                }
            }

            return false;
        }

        protected override void Draw(SpriteBatch spriteBatch) {
            size = parent.size;
            position = parent.position;
            base.Draw(spriteBatch);
        }

    }
}
