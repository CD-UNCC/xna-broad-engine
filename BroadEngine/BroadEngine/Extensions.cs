using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BroadEngine
{
    public static class Extensions
    {
        #region Colors

        public static Vector4 ToVector4(this Color color)
        {
            return new Vector4(color.R, color.G, color.B, color.A);
        }

        #endregion

        #region Vector4

        public static Color ToColor(this Vector4 vector)
        {
            return new Color(vector.X, vector.Y, vector.Z, vector.W);
        }

        #endregion

        #region Texture2D

        public static Vector2 Center(this Texture2D tex)
        {
            return new Vector2(tex.Width / 2, tex.Height / 2);
        }

        #endregion

        #region Point

        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        #endregion
    }
}
