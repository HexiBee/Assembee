using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.UI {
    class NineSliceTexture {

        private Texture2D texture;
        private Point slice1;
        private Point slice2;

        private Rectangle r1;
        private Rectangle r2;
        private Rectangle r3;
        private Rectangle r4;
        private Rectangle r5;
        private Rectangle r6;
        private Rectangle r7;
        private Rectangle r8;
        private Rectangle r9;

        //  r1 | r2 | r3
        // ----+----+----
        //  r4 | r5 | r6
        // ----+----+----
        //  r7 | r8 | r9

        public NineSliceTexture(Texture2D texture, float margin) {

            this.texture = texture;
            slice1 = (Vector2.One * margin).ToPoint();
            slice2 = (new Vector2(texture.Width - margin, texture.Height - margin)).ToPoint();

            r1 = new Rectangle(Point.Zero, slice1);
            r2 = new Rectangle(slice1.X, 0, slice2.X - slice1.X, slice1.Y);
            r3 = new Rectangle(slice2.X, 0, texture.Width - slice2.X, slice1.Y);

            r4 = new Rectangle(0, slice1.Y, slice1.X, slice2.Y - slice1.Y);
            r5 = new Rectangle(slice1.X, slice1.Y, slice2.X - slice1.X, slice2.Y - slice1.Y);
            r6 = new Rectangle(slice2.X, slice1.Y, texture.Width - slice2.X, slice2.Y - slice1.Y);

            r7 = new Rectangle(0, slice2.Y, slice1.X, texture.Height - slice2.Y);
            r8 = new Rectangle(slice1.X, slice2.Y, slice2.X - slice1.X, texture.Height - slice2.Y);
            r9 = new Rectangle(slice2.X, slice2.Y, texture.Width - slice2.X, texture.Height - slice2.Y);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle rect, Color color) {

            int x = rect.X;
            int y = rect.Y;
            int w = rect.Width;
            int h = rect.Height;

            int cw = rect.Width - slice1.X - (texture.Width - slice2.X);
            int ch = rect.Height - slice1.Y - (texture.Height - slice2.Y);

            // Draw corners
            spriteBatch.Draw(texture, new Rectangle(rect.Location, slice1), r1, color);
            spriteBatch.Draw(texture, new Rectangle(x + slice1.X + cw, y, texture.Width - slice2.X, slice1.Y), r3, color);
            spriteBatch.Draw(texture, new Rectangle(x, y + slice1.Y + ch, slice1.X, texture.Height - slice2.Y), r7, color);
            spriteBatch.Draw(texture, new Rectangle(x + slice1.X + cw, y + slice1.Y + ch, texture.Width - slice2.X, texture.Height - slice2.Y), r9, color);

            // Draw edges
            spriteBatch.Draw(texture, new Rectangle(x + slice1.X, y, cw, slice1.Y), r2, color);
            spriteBatch.Draw(texture, new Rectangle(x, y + slice1.Y, slice1.X, ch), r4, color);
            spriteBatch.Draw(texture, new Rectangle(x + slice1.X + cw, y + texture.Width - slice2.X, texture.Width - slice2.X, ch), r6, color);
            spriteBatch.Draw(texture, new Rectangle(x + slice1.X, y + slice1.Y + ch, cw, texture.Height - slice2.Y), r8, color);

            // Draw center
            spriteBatch.Draw(texture, new Rectangle(x + slice1.X, y + slice1.X, cw, ch), r5, color);

        }

    }
}
