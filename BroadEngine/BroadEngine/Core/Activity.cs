﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BroadEngine.GameObjects;
using IUpdateable = BroadEngine.GameObjects.IUpdateable;
using IDrawable = BroadEngine.GameObjects.IDrawable;

namespace BroadEngine.Core
{
    public class Activity
    {
        protected List<GameObject> _activityObjects = new List<GameObject>();

        #region Public Methods

        public virtual void Load() { }
        public virtual void Unload() { }
        public virtual void Update(GameTime gameTime, bool isPaused)
        {
            var toUpdate = GetObjectsByType<IUpdateable>();
            foreach (IUpdateable child in toUpdate)
                child.Update(gameTime, isPaused);
        }
        public virtual void Draw(GameTime gameTime, bool isPaused) 
        {
            var toDraw = GetObjectsByType<IDrawable>();
            foreach (IDrawable child in toDraw)
                child.Draw(gameTime, isPaused);
        }

        public IEnumerable<T> GetObjectsByType<T>()
        {
            return _activityObjects.OfType<T>();
        }

        #endregion

        #region Protected Methods

        #endregion
    }
}
