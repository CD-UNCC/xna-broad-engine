using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BroadEngine.Core
{
    public class Activity
    {
        public virtual void Load() { }
        public virtual void Update() { }
        public virtual void Draw() { }
        public virtual void Unload() { }
    }
}
