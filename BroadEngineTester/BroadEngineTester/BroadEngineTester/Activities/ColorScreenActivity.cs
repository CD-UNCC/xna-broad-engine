using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BroadEngine.Core;
using Microsoft.Xna.Framework;
using BroadEngine.GameObjects.Modules;

namespace BroadEngineTester.Activities
{
    public class ColorScreenActivity : Activity
    {
        ReversibleTransition<Color> cTrans;
        int count = 0;
        public override void Load()
        {
            cTrans = new ReversibleTransition<Color>(Color.BlanchedAlmond, Color.Coral, 1);
            cTrans.OnValueChanged = new Action<Color>(ChangeColor);
            cTrans.Loops = true;
            cTrans.OnLoopReverse = new Action(Reversed);
            cTrans.Running = true;
            _activityObjects.Add(cTrans);
        }

        public void Reversed()
        {
            if (++count >= 5)
                cTrans.Loops = false;
        }

        public void ChangeColor(Color color)
        {
            Screen.ScreenClearColor = color;
        }
    }
}
