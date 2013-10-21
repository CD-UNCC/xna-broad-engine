using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BroadEngine.Core;
using Microsoft.Xna.Framework;

namespace BroadEngineTester.Activities
{
    public class ColorScreenActivity : Activity
    {
        public override void  Load()
        {
            Screen.ScreenClearColor = Color.Aquamarine;
            Screen.ToggleFullScreen();
        }
    }
}
