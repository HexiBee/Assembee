using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.UI {
    class Root : Element {

        public Root() : base(null, Vector2.Zero, new Vector2(Game1.windowHandler.windowWidth, Game1.windowHandler.windowHeight)) {
        }

        protected override Vector2 getDrawPosition() {
            return Vector2.Zero;
        }

        public void DrawAll(SpriteBatch spriteBatch) {
            Draw(spriteBatch);
        }

        public void updateSize() {
            size.X = Game1.windowHandler.windowWidth;
            size.Y = Game1.windowHandler.windowHeight;
        }

        public override bool IntersectingGlobalPoint(Point point) {
            foreach (Element child in children) {
                if (child.IntersectingGlobalPoint(point)) {
                    return true;
                }
            }

            return false;
        }

    }
}
