using Assembee.Game.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game {

    public class Camera {

        private const float SPD_MOVE = 0.1f;
        private const float SPD_MOVE_MOUSE = 0.7f;

        public float moveSpeed = 0.0f;
        public float baseMoveSpeed = 0.0f;
        public Vector2 mPosStart;
        public Vector2 mOffset;
        public Vector2 position = new Vector2(0, 0);

        public static Vector2 pos1;
        public Matrix Transform;
        public Matrix Scale;
        public float scale = 1f;
        public Vector2 approach = new Vector2(0, 0);
        public float scaleLerp;

        public float spdMove = 0.1f, spdScale = 0.1f;

        public Camera() {
        }

        public void Update() {

            if (Input.MouseHold(0)) {
                mOffset = (mPosStart - Input.getMousePos()) * scale;
                position += mOffset;
                spdMove = SPD_MOVE_MOUSE;
            } else {
                spdMove = SPD_MOVE;
            }

            if (Input.keyDown(Input.Left)) {
                position = new Vector2(position.X - moveSpeed, position.Y);
            }
            if (Input.keyDown(Input.Right)) {
                position = new Vector2(position.X + moveSpeed, position.Y);
            }
            if (Input.keyDown(Input.Up)) {
                position = new Vector2(position.X, position.Y - moveSpeed);
            }
            if (Input.keyDown(Input.Down)) {
                position = new Vector2(position.X, position.Y + moveSpeed);
            }


            mPosStart = Input.getMousePos();

            // -- scrolling
            if (Input.scrollPressed() != 0) {
                scaleLerp += 0.2f * Input.scrollPressed();
            }

            scale = Util.Lerp(scale, scaleLerp, spdScale);
            moveSpeed = baseMoveSpeed + scale * 3.0f;

            if (scale < 1.0f) {
                scale = 1.0f;
                scaleLerp = 1.0f;
            } else if (scale > 10.0f) {
                scale = 10.0f;
                scaleLerp = 10.0f;
            }
            // -- end scrolling

            // Center on target
            approach.X = Util.Lerp(approach.X, -position.X, spdMove);
            approach.Y = Util.Lerp(approach.Y, -position.Y, spdMove);
            
            var positionMatrix = Matrix.CreateTranslation(
              approach.X,
              approach.Y,
              0);

            var offset = Matrix.CreateTranslation(
                (Game1.windowHandler.windowWidth / 2)/(1/scale),
                (Game1.windowHandler.windowHeight / 2) / (1/scale),
                0);

            pos1.X = positionMatrix.M41;
            pos1.Y = positionMatrix.M42;

            Transform = positionMatrix * offset;
            Scale = Matrix.CreateScale((1/scale));
            Transform = Matrix.Multiply(Transform, Scale);
        }
    }
}
