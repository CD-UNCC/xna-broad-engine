using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BroadEngine.Core
{
    public class Activity
    {
        public Activity()
        {
            
        }

        public void Invoke()
        {
            Screen.CurrentScreenClearColor = Color.Aquamarine;
        }
    }
}
