using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BroadEngine
{
    public static class Helper
    {
        #region Clamping

        public static T ClampValueMin<T>(T cur, T min) where T : IComparable
        {
            if (cur.CompareTo(min) == -1)
                cur = min;
            return cur;
        }

        public static T ClampValueMax<T>(T cur, T max) where T : IComparable
        {
            if (cur.CompareTo(max) == 1)
                cur = max;
            return cur;
        }

        public static T ClampValueBetween<T>(T cur, T min, T max) where T : IComparable
        {
            cur = ClampValueMin<T>(cur, min);
            cur = ClampValueMax<T>(cur, max);
            return cur;
        }

        #endregion
    }
}
