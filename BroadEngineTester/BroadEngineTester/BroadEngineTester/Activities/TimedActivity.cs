using BroadEngine.Core;
using BroadEngine.GameObjects.Modules;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BroadEngineTester.Activities
{
    class TimedActivity : Activity
    {
        public override void Load()
        {
            Screen.ScreenClearColor = Color.BurlyWood;
            _activityObjects.Add(new Timer(2, new Action(TimerFinished)));
        }

        public void TimerFinished()
        {
            Screen.ScreenClearColor = Color.BlanchedAlmond;
        }
    }
}
