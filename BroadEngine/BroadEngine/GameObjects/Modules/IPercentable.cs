using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BroadEngine.GameObjects.Modules
{
    public interface IPercentable
    {
        float PercentRemaining { get; }
        float PercentCompleted { get; }
    }
}
