using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BroadEngine.Core
{
    public static class ActivityManager
    {
        #region Fields

        static Queue<Activity> _activityList;
        static bool _loadNewActivity = true;

        #endregion

        #region Properties

        public static Activity CurrentActivity;
        public static bool IsPaused;

        #endregion

        static ActivityManager()
        {
            _activityList = new Queue<Activity>();
            CurrentActivity = new Activity();
        }

        #region Public Methods

        public static void QueueActivity(Activity activity)
        {
            _activityList.Enqueue(activity);
        }
        public static void QueueActivity<T>() where T : Activity, new() { QueueActivity(new T()); }

        public static void PrepareNextActivity()
        {
            _loadNewActivity = true;
        }

        #endregion

        #region Internal Methods

        internal static void Update(GameTime gameTime)
        {
            if (_loadNewActivity)
            {
                _loadNewActivity = false;
                CurrentActivity.Unload();
                CurrentActivity = _activityList.Dequeue();
                CurrentActivity.Load();
            }
            CurrentActivity.Update(gameTime, IsPaused);
        }

        internal static void Draw(GameTime gameTime)
        {
            CurrentActivity.Draw(gameTime, IsPaused);
        }

        #endregion

    }
}