using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BroadEngine.GameObjects.Modules
{
    public class Transition : GameObject, IAttaches, IUpdateable, IPercentable
    {
        #region Fields

        protected float _curPercent;
        protected float _transitionSeconds;
        #endregion

        #region Protected Properties

        protected float StepPerSec { get { return 1 / _transitionSeconds; } }

        #endregion

        #region Public Properties

        public Action OnFinish;
        public bool Running;
        public bool Finished { get { return !Running && _curPercent == 1; } }
        public float PercentCompleted { get { return _curPercent; } }
        public float PercentRemaining { get { return 1f - PercentCompleted; } }

        #endregion

        #region Constructors

        public Transition(float duration)
        {            
            _transitionSeconds = duration;
            _curPercent = 0;
            Running = false;
        }
        public Transition()
        {
            _curPercent = 0;
            _transitionSeconds = 1;
            Running = false;
        }

        #endregion

        #region Protected Methods

        protected virtual void ValueChanged() { }

        #endregion

        #region Public Methods

        public void OnAttach(IAttachable parent) { }

        public void Reset(bool running)
        {
            _curPercent = 0;
            Running = running;
        }
        public void Reset() { Reset(Running); }

        public void Update(GameTime gameTime, bool isPaused)
        {
            if (Running)
            {
                _curPercent += StepPerSec * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _curPercent = Helper.ClampValueMax<float>(_curPercent, 1);
                if (_curPercent == 1)
                {
                    Running = false;
                    if (OnFinish != null)
                        OnFinish.Invoke();
                }
                ValueChanged();
            }
        }

        #endregion
    }

    public class FloatTransition : Transition
    {
        #region Fields

        float _startValue;
        float _endValue;

        #endregion

        #region Public Properties

        public Action<float> OnValueChanged;
        public float CurValue { get { return MathHelper.Lerp(_startValue, _endValue, _curPercent); } }

        #endregion

        #region Constructors

        public FloatTransition(float start, float end, float duration) : base(duration)
        {
            _startValue = start;
            _endValue = end;
        }

        #endregion

        #region Protected Methods

        protected override void ValueChanged()
        {
            if (OnValueChanged != null)
                OnValueChanged.Invoke(CurValue);
        }

        #endregion
    }

    public class ColorTransition : Transition
    {
        #region Fields

        Color _startValue;
        Color _endValue;

        #endregion

        #region Public Properties

        public Action<Color> OnValueChanged;
        public Color CurValue { get { return Vector4.Lerp(_startValue.ToVector4(), _endValue.ToVector4(), _curPercent).ToColor(); } }

        #endregion

        #region Constructors

        public ColorTransition(Color start, Color end, float duration) : base(duration)
        {
            _startValue = start;
            _endValue = end;
        }

        #endregion

        #region Protected Methods

        protected override void ValueChanged()
        {
            if (OnValueChanged != null)
                OnValueChanged.Invoke(CurValue);
        }

        #endregion
    }
}
