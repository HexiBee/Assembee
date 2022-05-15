using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.UI {
    class Panel : Element {

        private const ContentRegistry.spr DEFAULT_SPRITE = ContentRegistry.spr.u_panel;

        private NineSliceTexture texture;

        public Panel(Element parent, Vector2 position, Vector2 size, Orientation anchor, Orientation origin) : base(parent, position, size, anchor, origin) {
            Texture2D temp;
            temp = ContentRegistry.GetTexture(DEFAULT_SPRITE);
            texture = new NineSliceTexture(temp, 10.0f);
        
        }

        public Panel(Element parent, Vector2 position, Vector2 size, Orientation anchor) : base(parent, position, size, anchor) {
            Texture2D temp;
            temp = ContentRegistry.GetTexture(DEFAULT_SPRITE);
            texture = new NineSliceTexture(temp, 10.0f);
        }

        public void PackVert(float space) {
            if (children.Count == 0) return;

            float offset = 0.0f;
            float height = 0.0f;

            for (int i = 0; i < children.Count; i++) {
                children[i].anchor = Orientation.Center;
                children[i].origin = Orientation.Center;
                children[i].position = new Vector2(0, offset);
                height += children[i].size.Y;

                if (i != children.Count - 1) {
                    offset += children[i].size.Y / 2.0f + children[i + 1].size.Y / 2.0f + space;
                    height += space;
                }
            }

            foreach (Element child in children) {
                child.position.Y -= height / 2.0f - children[0].size.Y / 2.0f;
            }

            FitToChildren(space);

        }

        protected override void Draw(SpriteBatch spriteBatch) {
            texture.Draw(spriteBatch, new Rectangle(getDrawPosition().ToPoint(), size.ToPoint()), color);
            base.Draw(spriteBatch);
        }

    }
}
