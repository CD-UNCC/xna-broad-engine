using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BroadEngine.GameObjects.Modules
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type to be transitioned. Currently supports float and Color.</typeparam>
    public class Transition<T> : GameObject, IAttaches, IUpdateable, IPercentable
    {
        protected static readonly Func<T, T, float, T> _valueGetter;

        #region Fields

        protected T _startValue;
        protected T _endValue;
        protected float _transitionSeconds;

        #endregion

        #region Protected Properties

        protected float StepPerSec { get { return 1 / _transitionSeconds; } }

        #endregion

        #region Public Properties

        public Action<T> OnValueChanged;
        public Action OnFinish;
        public bool Running;
        public bool Finished { get { return !Running && CurrentPercent == 1; } }
        public float CurrentPercent { get; protected set; }
        public float PercentCompleted { get { return CurrentPercent; } }
        public float PercentRemaining { get { return 1f - PercentCompleted; } }
        public T CurrentValue { get { return _valueGetter(_startValue, _endValue, CurrentPercent); } }

        #endregion

        #region Constructors

        static Transition()
        {
            _valueGetter = (Func<T, T, float, T>)Delegate.CreateDelegate(typeof(Func<T, T, float, T>), typeof(TransitionValueMethods).GetMethod("GetValue", new Type[] { typeof(T), typeof(T), typeof(float) }));
        }

        public Transition(T start, T end, float duration)
        {
            _startValue = start;
            _endValue = end;
            _transitionSeconds = duration;
            CurrentPercent = 0;
            Running = false;
        }

        #endregion

        #region Protected Methods

        protected virtual void IncrementPercent(float seconds)
        {
            CurrentPercent += StepPerSec * seconds;
            CurrentPercent = Helper.ClampValueMax(CurrentPercent, 1);
        }

        protected virtual bool TransitionComplete()
        {
            return CurrentPercent == 1;
        }

        #endregion

        #region Public Methods

        public void OnAttach(IAttachable parent) { }

        public virtual void Reset(bool running)
        {
            CurrentPercent = 0;
            Running = running;
        }
        public void Reset() { Reset(Running); }

        public virtual void Update(GameTime gameTime, bool isPaused)
        {
            if (Running && !isPaused)
            {
                float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
                IncrementPercent(seconds);
                if (TransitionComplete() && seconds > 0)
                {
                    Running = false;
                    if (OnFinish != null)
                        OnFinish();
                }
                if (OnValueChanged != null)
                    OnValueChanged(CurrentValue);
            }
        }

        #endregion
    }

    public class ReversibleTransition<T> : Transition<T>
    {
        #region Public Properties

        public bool Loops;
        public bool Forward;
        public Action OnLoopReverse;

        #endregion

        #region Constructors

        public ReversibleTransition(T start, T end, float duration) : base(start, end, duration) 
        {
            Loops = false;
            Forward = true;
        }

        #endregion

        #region Protected Methods

        protected override void IncrementPercent(float seconds)
        {
            if (Forward)
                base.IncrementPercent(seconds);
            else
            {
                CurrentPercent -= StepPerSec * seconds;
                CurrentPercent = Helper.ClampValueMin(CurrentPercent, 0);
            }
        }

        protected override bool TransitionComplete()
        {
            if (CurrentPercent == 0 || CurrentPercent == 1)
            {
                Reverse();
                if (Loops)
                {
                    if (OnLoopReverse != null)
                        OnLoopReverse();
                    return false;
                }
                return true;
            }
            return false;
        }

        #endregion

        #region Public Methods

        public override void Reset(bool running)
        {
            CurrentPercent = Forward ? 0 : 1;
            Running = running;
        }

        public void Reverse()
        {
            Forward = !Forward;
        }

        #endregion
    }

    class TransitionValueMethods
    {
        public static float GetValue(float start, float end, float percent) { return MathHelper.Lerp(start, end, percent); }
        public static Vector2 GetValue(Vector2 start, Vector2 end, float percent) { return Vector2.Lerp(start, end, percent); }
        public static Color GetValue(Color start, Color end, float percent) { return Vector4.Lerp(start.ToVector4(), end.ToVector4(), percent).ToColor(); }
    }
}
