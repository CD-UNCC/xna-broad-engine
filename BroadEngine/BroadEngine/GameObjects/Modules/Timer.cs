using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BroadEngine.GameObjects.Modules
{
    public class Timer : GameObject, IAttaches, IUpdateable, IPercentable
    {
        #region Fields

        float _maxTime;
        float _curTime;

        #endregion

        #region Public Properties

        public float PercentCompleted { get { return Helper.ClampValueBetween<float>(_curTime / _maxTime, 0, 1); } }
        public float PercentRemaining { get { return 1f - PercentCompleted; } }
        public bool Running;
        public bool Finished { get { return !Running && _curTime <= 0; } }
        public bool Countdown { get; private set; }
        public Action OnFinish;

        #endregion

        #region Constructor

        public Timer(float timeInSeconds, bool countsDown)
        {
            _maxTime = timeInSeconds;
            _curTime = _maxTime;
            Running = false;
            Countdown = countsDown;
        }

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

        public void Update(GameTime gameTime, bool isPaused)
        {
            if (Running && !isPaused)
            {
                _curTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                _curTime = Helper.ClampValueMin<float>(_curTime, 0);
                if (_curTime == 0)
                {
                    Running = false;
                    if (OnFinish != null)
                        OnFinish();
                }
            }
        }

        #endregion
    }
}
