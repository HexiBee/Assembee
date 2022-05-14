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
        public float spdMove = 0.1f, spdScale = 0.1f;
        
        public Entity target;

        public Camera(Entity target) {
            this.target = target;

        }

        public void Follow() {
            if (target == null) return;

            // Center on target
            approach.X = Util.Lerp(approach.X, (-target.position.X), spdMove);
            approach.Y = Util.Lerp(approach.Y, -target.position.Y, spdMove);
            
            var position = Matrix.CreateTranslation(
              approach.X,
              approach.Y,
              0);

            var offset = Matrix.CreateTranslation(
                (Game1.ScreenWidth - Game1.ScreenWidth / 2)/(1/scale),
                (Game1.ScreenHeight - Game1.ScreenHeight / 2) / (1/scale),
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
