using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.UI {
    class ButtonBase : Element{

        private const ContentRegistry.spr DEFAULT_SPRITE = ContentRegistry.spr.u_panel;
        private static Color HOVER_COLOR = new Color(0.9f, 0.9f, 0.9f);
        private static Color CLICK_COLOR = new Color(0.8f, 0.8f, 0.8f);

        private NineSliceTexture backgroundTexture;

        private Action function;
        
        public ButtonBase(Element parent, Vector2 position, Vector2 size, Orientation anchor) : base(parent, position, size, anchor) {
            backgroundTexture = new NineSliceTexture(ContentRegistry.GetTexture(DEFAULT_SPRITE), 10.0f);
            function = () => { };
        }

        public void AssignFunction(Action function) {
            this.function = function;
        }

        protected override void Draw(SpriteBatch spriteBatch) {

            if (IntersectingGlobalPoint(Input.getMousePosition().ToPoint())) {
                if (Input.MouseHold(0)) {
                    color = CLICK_COLOR;
                } else {
                    color = HOVER_COLOR;
                }

                if (Input.Click(0)) {
                    function.Invoke();
                }

            } else {
                color = Color.White;
            }

            backgroundTexture.Draw(spriteBatch, new Rectangle(getDrawPosition().ToPoint(), size.ToPoint()), color);
            base.Draw(spriteBatch);
        }

    }
}
