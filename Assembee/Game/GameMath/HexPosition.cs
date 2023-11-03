using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Newtonsoft.Json;

namespace Assembee.Game.GameMath
{
    public class HexPosition {

        public static readonly float HexSize = 127.0f;
        private static readonly float SQRT3 = 1.73205080757f;
        private static readonly float FAC = 2.0f / HexSize / 3.0f;
        private static readonly Matrix2 hexMatrix = new Matrix2(MathF.Sqrt(3), MathF.Sqrt(3) / 2.0f, 0.0f, 3.0f / 2.0f);

        [JsonProperty] public int X { get; private set; }
        [JsonProperty] public int Y { get; private set; }

        public HexPosition(int x, int y) {
            X = x; 
            Y = y;
        }

        public HexPosition() {
        }

        public static HexPosition PositionToHexPosition(Vector2 position) {
            float x = 0.5f * (SQRT3 * position.X - position.Y) * FAC;
            float y = position.Y * FAC;
            float z = -x - y;

            float q = (int)MathF.Round(x);
            float r = (int)MathF.Round(y);
            float s = (int)MathF.Round(z);

            float q_diff = MathF.Abs(q - x);
            float r_diff = MathF.Abs(r - y);
            float s_diff = MathF.Abs(s - z);

            if (q_diff > r_diff && q_diff > s_diff) {
                q = -r - s;
            } else if (r_diff > s_diff) {
                r = -q - s;
            } else {
                s = -q - r;
            }
            return new HexPosition((int)q, (int)r);
        }

        public static Vector2 HexPositionToPosition(HexPosition hexPosition) {
            return hexMatrix * new Vector2(hexPosition.X * 127, hexPosition.Y * 127);
        }
    }
}
