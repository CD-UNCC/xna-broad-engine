using System;
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
        protected Queue<GameObject> _toRemove = new Queue<GameObject>();
        protected Queue<GameObject> _toAdd = new Queue<GameObject>();

        #region Public Methods

        public virtual void Load() { }
        public virtual void Unload() { }
        public virtual void Update(GameTime gameTime, bool isPaused)
        {
            while (_toRemove.Count > 0)
                _activityObjects.Remove(_toRemove.Dequeue());
            while (_toAdd.Count > 0)
                _activityObjects.Add(_toAdd.Dequeue());

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

        public void AddObject(GameObject toAdd)
        {
            _toAdd.Enqueue(toAdd);
        }

        public void RemoveObject(GameObject toRemove)
        {
            _toRemove.Enqueue(toRemove);
        }

        #endregion

        #region Protected Methods

        protected void ReplaceObject<T>(ref T curObj, T newObj) where T : GameObject
        {
            RemoveObject(curObj);
            curObj = newObj;
            AddObject(curObj);
        }

        #endregion
    }
}
