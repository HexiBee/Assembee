using Assembee.Game.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Assembee.Game {

    public class Camera {

        private const float SPD_MOVE = 0.1f;
        private const float SPD_MOVE_MOUSE = 0.7f;

        private float moveSpeed = 0.0f;
        private float baseMoveSpeed = 0.0f;
        private Vector2 mPosStart;
        private Vector2 mOffset;

        private float spdMove = 0.1f;
        private float spdScale = 0.1f;

        private Vector2 desiredPosition;
        private float desiredScale = 1f;

        private Vector2 _position;

        public Vector2 Position { get { return _position; } }
        public float Scale { get; private set; }

        public Matrix Transform { get; private set; }

        public Camera() {
            desiredPosition = new Vector2(0, 0);
            _position = new Vector2(0, 0);
        }

        public void Update() {

            if (Input.MouseHold(0)) {
                mOffset = (mPosStart - Input.getMousePosition()) * desiredScale;
                desiredPosition += mOffset;
                spdMove = SPD_MOVE_MOUSE;
            } else {
                spdMove = SPD_MOVE;
            }

            if (Input.keyDown(Input.Left)) {
                desiredPosition += new Vector2(-moveSpeed, 0);
            }
            if (Input.keyDown(Input.Right)) {
                desiredPosition += new Vector2(moveSpeed, 0);
            }
            if (Input.keyDown(Input.Up)) {
                desiredPosition += new Vector2(0, -moveSpeed);
            }
            if (Input.keyDown(Input.Down)) {
                desiredPosition += new Vector2(0, moveSpeed);
            }

            mPosStart = Input.getMousePosition();

            // -- scrolling
            if (Input.scrollPressed() != 0) {
                desiredScale += 0.2f * Input.scrollPressed();
            }

            Scale = Util.Lerp(Scale, desiredScale, spdScale);
            moveSpeed = baseMoveSpeed + Scale * 3.0f;

            if (Scale < 1.0f) {
                Scale = 1.0f;
                desiredScale = 1.0f;
            } else if (Scale > 10.0f) {
                Scale = 10.0f;
                desiredScale = 10.0f;
            }
            // -- end scrolling

            // Center on target
            _position.X = Util.Lerp(Position.X, desiredPosition.X, spdMove);
            _position.Y = Util.Lerp(Position.Y, desiredPosition.Y, spdMove);
            
            var positionMatrix = Matrix.CreateTranslation(
              -_position.X,
              -_position.Y,
              0);

            var offset = Matrix.CreateTranslation(
                (Game1.windowHandler.windowWidth / 2)  / (1/Scale),
                (Game1.windowHandler.windowHeight / 2) / (1/Scale),
                0);

            Transform = positionMatrix * offset * Matrix.CreateScale(1 / Scale);
        }

        public bool InBounds(Vector2 position) {
            return !((position.X < Position.X - Scale * Game1.windowHandler.windowWidth / 1.2) ||
                   (position.X > Position.X + Scale * Game1.windowHandler.windowWidth / 1.2) ||
                   (position.Y < Position.Y - Scale * Game1.windowHandler.windowHeight / 1.2) ||
                   (position.Y > Position.Y + Scale * Game1.windowHandler.windowHeight / 1.2));

        }
    }
}
