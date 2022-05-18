using Assembee.Game.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game {

    public class Camera {

        public static Vector2 pos1;
        public Matrix Transform;
        public Matrix Scale;
        public float scale = 1f;
        public Vector2 approach = new Vector2(0, 0);
        public const float SPD_MOVE = 0.1f, SPD_MOVE_MOUSE = 0.7f;
        public float scaleLerp;

        public float spdMove = 0.1f, spdScale = 0.1f;
        
        public CameraPos target;

        public Camera(CameraPos target) {
            this.target = target;

        }

        public void Follow() {
            if (target == null) return;

            // -- scrolling
            if (Input.scrollPressed() != 0) {
                scaleLerp += 0.2f * Input.scrollPressed();
            }

            scale = Util.Lerp(scale, scaleLerp, spdScale);
            target.moveSpeed = target.baseMoveSpeed + scale * 3.0f;

            if (scale < 1.0f) {
                scale = 1.0f;
                scaleLerp = 1.0f;
            } else if (scale > 10.0f) {
                scale = 10.0f;
                scaleLerp = 10.0f;
            }
            // -- end scrolling

            // Center on target
            approach.X = Util.Lerp(approach.X, (-target.position.X), spdMove);
            approach.Y = Util.Lerp(approach.Y, -target.position.Y, spdMove);
            
            var position = Matrix.CreateTranslation(
              approach.X,
              approach.Y,
              0);

            var offset = Matrix.CreateTranslation(
                (Game1.windowHandler.windowWidth / 2)/(1/scale),
                (Game1.windowHandler.windowHeight / 2) / (1/scale),
                0);

            pos1.X = position.M41;
            pos1.X = position.M42;

      

            Transform = position * offset;
            Scale = Matrix.CreateScale((1/scale));
            Transform = Matrix.Multiply(Transform, Scale);
            //Transform = 

        }
    }
}
