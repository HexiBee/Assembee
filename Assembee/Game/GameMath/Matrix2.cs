using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.GameMath {
    class Matrix2 {

        public float M11, M12, M21, M22;

        public Matrix2(float M11, float M12, float M21, float M22) {
            this.M11 = M11;
            this.M12 = M12;
            this.M21 = M21;
            this.M22 = M22;
        }

        public static Matrix2 operator*(Matrix2 a, Matrix2 b) {
            return new Matrix2(
                a.M11 * b.M11 + a.M12 * b.M21, a.M11 * b.M12 + a.M12 * b.M22,
                a.M21 * b.M11 + a.M22 * b.M21, a.M21 * b.M12 + a.M22 * b.M22
            );
        }

        public static Vector2 operator*(Matrix2 a, Vector2 b) {
            return new Vector2(a.M11 * b.X + a.M12 * b.Y, a.M21 * b.X + a.M22 * b.Y);
        }

    }
}
