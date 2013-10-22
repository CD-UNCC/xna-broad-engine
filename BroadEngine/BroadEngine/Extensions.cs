using Microsoft.Xna.Framework;
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
    }
}
