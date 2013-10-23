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
        protected class DelayedAction
        {
            float _delay;
            Action _action;

            public DelayedAction(float delay, Action action)
            {
                _delay = delay;
                _action = action;
            }
            public bool Update(GameTime gameTime)
            {
                _delay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_delay <= 0)
                {
                    _action();
                    return true;
                }
                return false;
            }
        }

        protected List<DelayedAction> _delayedActions = new List<DelayedAction>();
        protected List<GameObject> _activityObjects = new List<GameObject>();
        protected Queue<GameObject> _toRemove = new Queue<GameObject>();
        protected Queue<GameObject> _toAdd = new Queue<GameObject>();
        protected float SecondsElapsed;

        #region Public Methods

        public virtual void Load() { }
        public virtual void Unload() { }
        public virtual void Update(GameTime gameTime, bool isPaused)
        {
            while (_toRemove.Count > 0)
                _activityObjects.Remove(_toRemove.Dequeue());
            while (_toAdd.Count > 0)
                _activityObjects.Add(_toAdd.Dequeue());
            if (!isPaused)
                for (int i = 0; i < _delayedActions.Count; i++)
                    if (_delayedActions[i].Update(gameTime))
                    {
                        _delayedActions.RemoveAt(i);
                        i--;
                    }

            UpdateBeforeObjects(gameTime, isPaused);

            var toUpdate = GetObjectsByType<IUpdateable>();
            foreach (IUpdateable child in toUpdate)
                child.Update(gameTime, isPaused);

            UpdateAfterObjects(gameTime, isPaused);
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

        public void AddDelayedAction(float delayInSeconds, Action action)
        {
            _delayedActions.Add(new DelayedAction(delayInSeconds, action));
        }

        #endregion

        #region Protected Methods

        protected void ReplaceObject<T>(ref T curObj, T newObj) where T : GameObject
        {
            RemoveObject(curObj);
            curObj = newObj;
            AddObject(curObj);
        }

        protected virtual void UpdateBeforeObjects(GameTime gameTime, bool isPaused) { }
        protected virtual void UpdateAfterObjects(GameTime gameTime, bool isPaused) { }

        #endregion
    }
}
