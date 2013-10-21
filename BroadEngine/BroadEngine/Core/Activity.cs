using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BroadEngine.GameObjects;

namespace BroadEngine.Core
{
    public class Activity
    {
        protected List<GameObject> _activityObjects = new List<GameObject>();

        #region Public Methods

        public virtual void Load() { }
        public virtual void Update(GameTime gameTime, bool isPaused) { }
        public virtual void Draw(GameTime gameTime, bool isPaused) { }
        public virtual void Unload() { }

        #endregion

        #region Protected Methods

        protected IEnumerable<GameObject> GetObjectsByType<T>() where T : GameObject
        {
            return _activityObjects.OfType<T>();
        }

        #endregion
    }
}
