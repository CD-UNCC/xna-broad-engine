using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BroadEngine.Core
{
    public static class ActivityManager
    {
        #region Fields

        static Queue<Activity> _activityList;

        #endregion

        #region Properties

        public static Activity CurrentActivity;

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

        #endregion

        #region Internal Methods

        internal static void LoadNextActivity()
        {
            if (_activityList.Count == 0)
                throw new InvalidOperationException("Attempted to load the next activity when none were present");
            CurrentActivity.Unload();
            CurrentActivity = _activityList.Dequeue();
            CurrentActivity.Load();
        }

        #endregion
    }
}