using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BroadEngine.GameObjects.Modules
{
    public class Timer : GameObject, IAttaches, IUpdateable
    {
        #region Fields

        float _maxTime;
        float _curTime;
        Action _onFinish;

        #endregion

        #region Public Properties

        public float PercentCompleted { get { return Helper.ClampValueBetween<float>(_curTime / _maxTime, 0, 1); } }
        public float PercentRemaining { get { return 1f - PercentCompleted; } }
        public bool Running;
        public bool Finished { get { return !Running && _curTime <= 0; } }

        #endregion

        #region Constructor

        public Timer(float timeInSeconds, Action onFinish, bool running)
        {
            _maxTime = timeInSeconds;
            _curTime = _maxTime;
            _onFinish = onFinish;
            Running = running;
        }
        public Timer(float timeInSeconds, Action onFinish) : this(timeInSeconds, onFinish, true) { }
        public Timer(float timeInSeconds) : this(timeInSeconds, null, true) { }

        #endregion

        #region Public Methods

        public void OnAttach(IAttachable parent)
        {
            
        }

        public void Reset(bool running)
        {
            _curTime = _maxTime;
            Running = running;
        }
        public void Reset() { Reset(Running); }

        public void SetFinishAction(Action action)
        {
            _onFinish = action;
        }

        public void Update(GameTime gameTime, bool isPaused)
        {
            if (Running && !isPaused)
            {
                _curTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                _curTime = Helper.ClampValueMin<float>(_curTime, 0);
                if (_curTime == 0)
                {
                    Running = false;
                    if (_onFinish != null)
                        _onFinish.Invoke();
                }
            }
        }

        #endregion
    }
}
