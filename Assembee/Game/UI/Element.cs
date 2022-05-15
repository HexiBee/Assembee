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

        public bool active = true;
        public Vector2 position;
        public Vector2 size;
        public Orientation anchor = Orientation.UpperLeft;
        public Orientation origin = Orientation.UpperLeft;
        public Color color = Color.White;

        protected Element parent;
        protected List<Element> children = new List<Element>();

        public Element(Element parent, Vector2 position, Vector2 size) {
            this.parent = parent;
            if (parent != null)
                parent.children.Add(this);
            this.position = position;
            this.size = size;
        }

        public Element(Element parent, Vector2 position, Vector2 size, Orientation anchor) {
            this.parent = parent;
            if (parent != null)
                parent.children.Add(this);
            this.position = position;
            this.size = size;
            this.anchor = anchor;
            origin = anchor;
        }

        public Element(Element parent, Vector2 position, Vector2 size, Orientation anchor, Orientation origin) {
            this.parent = parent;
            if (parent != null)
                parent.children.Add(this);
            this.position = position;
            this.size = size;
            this.anchor = anchor;
            this.origin = origin;
        }

        public void FitToElement(Element element, float margin) {
            size = element.size + Vector2.One * margin;
        }

        public void FitToChildren(float margin) {
            Rectangle rect = new Rectangle(0, 0, 1, 1);
            foreach (Element child in children.ToArray()) {
                rect = Rectangle.Union(rect, child.getBounds());
            }
            size = rect.Size.ToVector2() + Vector2.One * margin * 2;
        }

        public virtual bool IntersectingGlobalPoint(Point point) {
            if (!active) return false;

            Rectangle rect = new Rectangle(getDrawPosition().ToPoint(), size.ToPoint());

            if (rect.Contains(point)) {
                return true;
            }

            foreach (Element child in children) {
                if (child.IntersectingGlobalPoint(point)) {
                    return true;
                }
            }

            return false;
        }

        protected virtual void Draw(SpriteBatch spriteBatch) {
            foreach (Element child in children.ToArray()) {
                if (child.active)
                    child.Draw(spriteBatch);
            }
        }

        protected virtual Rectangle getBounds() {
            return new Rectangle(position.ToPoint(), size.ToPoint());
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
