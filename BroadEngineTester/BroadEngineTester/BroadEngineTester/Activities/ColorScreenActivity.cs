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
        ColorTransition cTrans;
        public override void Load()
        {
            cTrans = new ColorTransition(Color.CadetBlue, Color.IndianRed, 3);
            cTrans.OnValueChanged = new Action<Color>(ChangeColor);
            cTrans.OnFinish = new Action(TransitionFinished);
            cTrans.Running = true;
            _activityObjects.Add(cTrans);
        }

        public void ChangeColor(Color color)
        {
            Screen.ScreenClearColor = color;
        }

        public void TransitionFinished()
        {
            //_activityObjects.Remove(cTrans);
            cTrans = new ColorTransition(Color.HotPink, Color.Honeydew, 5);
            cTrans.OnValueChanged = new Action<Color>(ChangeColor);
            cTrans.OnFinish = new Action(TransitionFinished);
            cTrans.Running = true;
            _activityObjects.Add(cTrans);
        }
    }
}
