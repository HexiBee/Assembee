using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.UI {
    class Element {

        public enum Orientation {
            UpperLeft,
            UpperMiddle,
            UpperRight,
            CenterLeft,
            Center,
            CenterRight,
            LowerLeft,
            LowerMiddle,
            LowerRight
        }

        protected Element parent;
        protected Vector2 position;
        protected Vector2 size;
        protected Orientation anchor = Orientation.UpperLeft;
        protected Orientation origin = Orientation.UpperLeft;

        public Element(Element parent, Vector2 position, Vector2 size) {
            this.parent = parent;
            this.position = position;
            this.size = size;
        }

        public Element(Element parent, Vector2 position, Vector2 size, Orientation anchor, Orientation origin) {
            this.parent = parent;
            this.position = position;
            this.size = size;
            this.anchor = anchor;
            this.origin = origin;
        }

        public virtual void Draw(SpriteBatch spriteBatch) {

        }

        protected virtual Vector2 getDrawPosition() {

            Vector2 pos = parent.getDrawPosition();
            float x = 0;
            float y = 0;

            if (anchor == Orientation.UpperMiddle || anchor == Orientation.Center || anchor == Orientation.LowerMiddle) {
                x = 0.5f;
            } else if (anchor == Orientation.UpperRight || anchor == Orientation.CenterRight || anchor == Orientation.LowerRight) {
                x = 1.0f;
            }

            if (anchor == Orientation.CenterLeft || anchor == Orientation.Center || anchor == Orientation.CenterRight) {
                y = 0.5f;
            } else if (anchor == Orientation.LowerLeft || anchor == Orientation.LowerMiddle || anchor == Orientation.LowerRight) {
                y = 1.0f;
            }

            pos.X += x * parent.size.X;
            pos.Y += y * parent.size.Y;

            if (origin == Orientation.UpperMiddle || origin == Orientation.Center || origin == Orientation.LowerMiddle) {
                x = 0.5f;
            } else if (origin == Orientation.UpperRight || origin == Orientation.CenterRight || origin == Orientation.LowerRight) {
                x = 1.0f;
            }

            if (origin == Orientation.CenterLeft || origin == Orientation.Center || origin == Orientation.CenterRight) {
                y = 0.5f;
            } else if (origin == Orientation.LowerLeft || origin == Orientation.LowerMiddle || origin == Orientation.LowerRight) {
                y = 1.0f;
            }

            pos.X -= x * size.X;
            pos.Y -= y * size.Y;

            return pos + position;
        }

    }
}
